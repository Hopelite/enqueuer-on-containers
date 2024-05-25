namespace Enqueuer.Identity.OAuth.Storage;

public interface IScopeStorage
{
    /// <summary>
    /// Checks whether the <paramref name="scope"/> is existing and valid scope.
    /// </summary>
    ValueTask<bool> CheckIfExistsAsync(string scope, CancellationToken cancellationToken);
}
