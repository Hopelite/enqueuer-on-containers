using Enqueuer.Identity.Contract.V1.Models.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class CreateOrUpdateUserRequest
    {
        public CreateOrUpdateUserRequest(long userId, string firstName, string? lastName)
        {
            UserId = userId;
            FirstName = ValidateFirstName(firstName);
            LastName = lastName;
        }

        public long UserId { get; }

        public string FirstName { get; }

        public string? LastName { get; }

        internal HttpContent GetBody()
        {
            var body = new CreateOrUpdateUserBody(FirstName, LastName);
            return JsonContent.Create(body, options: JsonSerialization.Options);
        }

        private static string ValidateFirstName(string firstName)
            => string.IsNullOrWhiteSpace(firstName)
                ? throw new ArgumentNullException()
                : firstName;

        private class CreateOrUpdateUserBody
        {
            public CreateOrUpdateUserBody(string firstName, string? lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public string FirstName { get; }

            public string? LastName { get; }
        }
    }
}
