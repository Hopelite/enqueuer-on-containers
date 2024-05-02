using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Authorization.Tests.Resources;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.Authorization.Validation.Exceptions;

namespace Enqueuer.Identity.Authorization.Tests.Validators;

public class ScopeValidatorTests
{
    private readonly IScopeValidator _scopeValidator;

    public ScopeValidatorTests()
    {
        _scopeValidator = new ScopeValidator();
    }

    [Fact]
    public void Validate_ScopeNameIsTooLong_ThrowsException()
    {
        var scope = new Scope("ScopeWithQuiteLongNameThatIsDefinitelyLongerThan64CharactersLikeThisOne", childScopes: null);

        Assert.Throws<InvalidScopeNameException>(() => _scopeValidator.Validate(scope));
    }

    [Fact]
    public void Validate_ScopeNameIsTooShort_ThrowsException()
    {
        var scope = new Scope("S", childScopes: null);

        Assert.Throws<InvalidScopeNameException>(() => _scopeValidator.Validate(scope));
    }

    [Theory]
    [InlineData("my scope")]
    [InlineData("UserInfo")]
    [InlineData("User:Info")]
    [InlineData("user-info")]
    public void Validate_ScopeNameContainsInvalidCharacters_ThrowsException(string scopeName)
    {
        var scope = new Scope(scopeName, childScopes: null);

        Assert.Throws<InvalidScopeNameException>(() => _scopeValidator.Validate(scope));
    }

    [Theory]
    [InlineData("que")]
    [InlineData("queue")]
    [InlineData("group:12")]
    [InlineData("queue:create")]
    public void Validate_ValidScopeName_PassesValidation(string scopeName)
    {
        var scope = new Scope(scopeName, childScopes: null);

        _scopeValidator.Validate(scope);
    }

    [Fact]
    public void Validate_ChildScopeNestingIsTooDeep_ThrowsException()
    {
        var scope = ResourceHelper.ReadResource<Scope>("Scope_Invalid_DeepNesting.json");

        Assert.Throws<NestingIsTooDeepException>(() => _scopeValidator.Validate(scope));
    }
}
