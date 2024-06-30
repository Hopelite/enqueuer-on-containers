using System;
using Npgsql;
using NpgsqlTypes;

namespace Enqueuer.Identity.Persistence.Procedures;

public static class CheckUserAccessProcedure
{
    public static NpgsqlCommand GetCommand(Uri resourceId, long userId, string scopeName)
    {
        return new NpgsqlCommand("SELECT check_user_access($1, $2, $3)")
        {
            Parameters =
            {
                new () { Value = resourceId.ToString(), NpgsqlDbType = NpgsqlDbType.Text },
                new () { Value = userId, NpgsqlDbType = NpgsqlDbType.Bigint },
                new () { Value = scopeName, NpgsqlDbType = NpgsqlDbType.Text },
            }
        };
    }

    public const string Definition =
        """
        CREATE OR REPLACE FUNCTION check_user_access(
        	resource_uri		TEXT,
        	user_id_to_check	BIGINT,
        	required_scope		TEXT
        ) RETURNS BOOLEAN AS
        $$
        DECLARE
        	current_resource_id INT;
        	current_user_id 	INT;
        	has_access			BOOLEAN := FALSE;
        BEGIN

        	SELECT id INTO current_resource_id FROM resources WHERE uri = resource_uri;
        	SELECT id INTO current_user_id FROM users WHERE user_id = user_id_to_check;

        	-- rename role_scope columns 
        	SELECT EXISTS (
        		SELECT 1 FROM user_resource_roles AS urr
        		JOIN roles		 AS r	ON urr.role_id = r.id
        		JOIN role_scope AS rs	ON rs.roles_id = r.id
        		JOIN scopes		 AS s	ON s.id = rs.scopes_id
        		WHERE urr.user_id = current_user_id
        			AND urr.resource_id = current_resource_id
        			AND (s.name = required_scope OR s.id IN (
        				WITH RECURSIVE scope_parents AS (
                        	SELECT	id, name, parent_id FROM scopes
        					WHERE	name = required_scope
                        	UNION ALL
        						SELECT s.id, s.name, s.parent_id FROM scopes s
        						JOIN scope_parents AS sp ON s.id = sp.parent_id
                    ) SELECT id FROM scope_parents))
        	) INTO has_access;

        	RETURN has_access;

        END
        $$ LANGUAGE plpgsql;
        """;
}
