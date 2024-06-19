using Enqueuer.OAuth.Core.Models;

namespace Enqueuer.OAuth.Tests;

public class ScopeTests
{
    [Theory]
    [MemberData(nameof(GetStringConstructorTestData))]
    public void Scope_StringConstructor_ShouldRemoveDuplicatesAndSetValues(string input, string expectedValue, string[] expectedValues)
    {
        var scope = new Scope(input);

        Assert.Equal(expectedValue, scope.Value);
        Assert.Equal(expectedValues, scope);
    }

    [Theory]
    [MemberData(nameof(GetCollectionConstructorTestData))]
    public void Scope_CollectionConstructor_ShouldRemoveDuplicatesAndSetValues(string[] input, string expectedValue, string[] expectedValues)
    {
        Scope scope = new Scope(input);

        Assert.Equal(expectedValue, scope.Value);
        Assert.Equal(expectedValues, scope);
    }

    public static IEnumerable<object?[]> GetStringConstructorTestData()
    {
        yield return new object?[]
        {
            "user queue:create user queue:participate",
            "user queue:create queue:participate",
            new string[] { "user", "queue:create", "queue:participate" }
        };
        yield return new object?[]
        {
            "",
            null,
            Array.Empty<string>(),
        };
        yield return new object?[]
        {
            null,
            null,
            Array.Empty<string>(),
        };
        yield return new object?[]
        {
            "user user queue:create queue:create",
            "user queue:create",
            new string[] { "user", "queue:create" }
        };
        yield return new object?[]
        {
            "a b c a b c",
            "a b c",
            new string[] { "a", "b", "c" }
        };
    }

    public static IEnumerable<object?[]> GetCollectionConstructorTestData()
    {
        yield return new object?[]
        {
            new string[] { "user", "queue:create", "user", "queue:participate" },
            "user queue:create queue:participate",
            new string[] { "user", "queue:create", "queue:participate" }
        };
        yield return new object?[]
        {
            Array.Empty<string>(),
            null,
            Array.Empty<string>()
        };
        yield return new object?[]
        {
            null,
            null,
            Array.Empty<string>()
        };
        yield return new object?[]
        {
            new string[] { "user", "user", "queue:create", "queue:create" },
            "user queue:create",
            new string[] { "user", "queue:create" }
        };
        yield return new object?[]
        {
            new string[] { "a", "b", "c", "a", "b", "c" },
            "a b c",
            new string[] { "a", "b", "c" }
        };
    }

}
