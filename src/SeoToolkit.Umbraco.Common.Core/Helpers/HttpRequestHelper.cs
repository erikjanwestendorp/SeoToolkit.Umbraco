using Microsoft.AspNetCore.Http;
using SeoToolkit.Umbraco.Common.Core.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace SeoToolkit.Umbraco.Common.Core.Helpers;

public class HttpRequestHelper
{
    private readonly IDomainService _domainService;

    public HttpRequestHelper(IDomainService domainService)
    {
        _domainService = domainService;
    }

    public IDomain? GetAssignedDomain(HttpRequest request)
    {
        var host = request.Host;

        var name = host.IsLocalHost() ? host.Value : host.Host;

        return _domainService.GetByName(name);
    }
}
