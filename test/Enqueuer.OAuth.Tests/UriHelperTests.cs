using Enqueuer.OAuth.Core.Helpers;

namespace Enqueuer.OAuth.Tests;

public class UriHelperTests
{
    [Theory]
    [MemberData(nameof(AppendQueryTestData))]
    public void AppendQuery_ReturnsExpectedUri(Uri uri, IDictionary<string, string> queryParameters, Uri expectedUri)
    {
        var result = UriHelper.AppendQuery(uri, queryParameters);
        Assert.Equal(expectedUri, result);
    }

    [Theory]
    [MemberData(nameof(GetUriWithQueryTestData))]
    public void GetUriWithQuery_ReturnsExpectedUri(string uri, UriKind uriKind, IDictionary<string, string> queryParameters, Uri expectedUri)
    {
        var result = UriHelper.GetUriWithQuery(uri, queryParameters, uriKind);
        Assert.Equal(expectedUri, result);
    }

    public static IEnumerable<object[]> AppendQueryTestData()
    {
        yield return new object[]
        {
            new Uri("http://example.com/path"),
            new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } },
            new Uri("http://example.com/path?key1=value1&key2=value2")
        };

        yield return new object[]
        {
            new Uri("http://example.com/path?existingKey=existingValue"),
            new Dictionary<string, string> { { "key1", "value1" } },
            new Uri("http://example.com/path?key1=value1")
        };

        yield return new object[]
        {
            new Uri("/path", UriKind.Relative),
            new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } },
            new Uri("/path?key1=value1&key2=value2", UriKind.Relative)
        };

        yield return new object[]
        {
            new Uri("/path?existingKey=existingValue", UriKind.Relative),
            new Dictionary<string, string> { { "key1", "value1" } },
            new Uri("/path?key1=value1", UriKind.Relative)
        };
    }

    public static IEnumerable<object[]> GetUriWithQueryTestData()
    {
        yield return new object[]
        {
            "http://example.com/path",
            UriKind.Absolute,
            new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } },
            new Uri("http://example.com/path?key1=value1&key2=value2")
        };

        yield return new object[]
        {
            "http://example.com/path?existingKey=existingValue",
            UriKind.Absolute,
            new Dictionary<string, string> { { "key1", "value1" } },
            new Uri("http://example.com/path?key1=value1")
        };

        yield return new object[]
        {
            "/path",
            UriKind.Relative,
            new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } },
            new Uri("/path?key1=value1&key2=value2", UriKind.Relative)
        };

        yield return new object[]
        {
            "/path?existingKey=existingValue",
            UriKind.Relative,
            new Dictionary<string, string> { { "key1", "value1" } },
            new Uri("/path?key1=value1", UriKind.Relative)
        };
    }
}