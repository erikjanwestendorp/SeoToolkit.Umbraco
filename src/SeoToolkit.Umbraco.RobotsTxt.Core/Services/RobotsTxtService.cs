using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using SeoToolkit.Umbraco.Common.Core.Helpers;
using SeoToolkit.Umbraco.RobotsTxt.Core.Interfaces;
using SeoToolkit.Umbraco.RobotsTxt.Core.Models.Business;
using Umbraco.Cms.Core.Models;

namespace SeoToolkit.Umbraco.RobotsTxt.Core.Services
{
    public class RobotsTxtService : IRobotsTxtService
    {
        private readonly IRobotsTxtRepository _robotsTxtRepository;
        private readonly IRobotsTxtValidator _robotsTxtValidator;
        private readonly IRobotsTxtSitemapProvider _sitemapProvider;
        private readonly HttpRequestHelper _requestHelper;

        public RobotsTxtService(IRobotsTxtRepository robotsTxtRepository,
            IRobotsTxtValidator robotsTxtValidator, HttpRequestHelper requestHelper, IRobotsTxtSitemapProvider sitemapProvider = null)
        {
            _robotsTxtRepository = robotsTxtRepository;
            _robotsTxtValidator = robotsTxtValidator;
            _requestHelper = requestHelper;
            _sitemapProvider = sitemapProvider;
        }

        public string GetContent(int? domainId = null)
        {
            if (domainId == null)
            {
                return _robotsTxtRepository.GetAll().FirstOrDefault(x => x.DomainId == 0)?.Content ?? string.Empty;
            }

            return _robotsTxtRepository.FirstOrDefault(x => x.DomainId == domainId)?.Content ?? string.Empty;
        }

        public string GetContentWithSitemaps(HttpRequest request)
        {
            string[] sitemaps = Array.Empty<string>();
            if (_sitemapProvider != null)
                sitemaps = _sitemapProvider.GetSitemapUrls(request).ToArray();

            //This will probably support multiple domains later on, but for now we can just take the only one
            var domain = _requestHelper.GetAssignedDomain(request);
            var content = GetContent(domain);
            
            if (sitemaps.Length > 0)
            {
                var sitemapStringBuilder = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(content)) sitemapStringBuilder.Append("\n");
                foreach (var sitemap in sitemaps)
                {
                    sitemapStringBuilder.Append($"Sitemap: {sitemap}\n");
                }
                content += sitemapStringBuilder.ToString();
            }

            return content;
        }

        public void SetContent(string content, int? domainId = null)
        {
            domainId ??= 0;
            var model = _robotsTxtRepository.FirstOrDefault(x => x.DomainId == domainId);
            if (model is null)
                model = new RobotsTxtModel();

            model.Content = content;
            model.DomainId = domainId ?? 0;

            _robotsTxtRepository.Update(model);
        }

        public IEnumerable<RobotsTxtValidation> Validate(string content)
        {
            return _robotsTxtValidator.Validate(content);
        }

        private string GetContent(IDomain domain)
        {
            if (domain == null)
            {
                return _robotsTxtRepository.GetAll().FirstOrDefault()?.Content ?? string.Empty;
            }

            return _robotsTxtRepository.Get(domain.Id)?.Content ?? string.Empty;
        }
    }
}
