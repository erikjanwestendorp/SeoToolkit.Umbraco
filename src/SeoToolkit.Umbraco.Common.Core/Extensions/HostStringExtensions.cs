using Microsoft.AspNetCore.Http;

namespace SeoToolkit.Umbraco.Common.Core.Extensions;

public static class HostStringExtensions
{
    public static bool IsLocalHost(this HostString hostString)
    {
        return hostString.Host.Contains("localhost");
    }
}
