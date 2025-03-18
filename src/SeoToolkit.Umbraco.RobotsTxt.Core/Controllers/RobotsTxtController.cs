using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.ActionsResults;
using Umbraco.Cms.Web.Common.Attributes;
using SeoToolkit.Umbraco.RobotsTxt.Core.Interfaces;
using SeoToolkit.Umbraco.RobotsTxt.Core.Models.PostModel;
using SeoToolkit.Umbraco.RobotsTxt.Core.Models.ViewModels;
using SeoToolkit.Umbraco.RobotsTxt.Core.Services;

namespace SeoToolkit.Umbraco.RobotsTxt.Core.Controllers
{
    [PluginController("SeoToolkit")]
    public class RobotsTxtController : UmbracoAuthorizedApiController
    {
        private readonly IRobotsTxtService _robotsTxtService;

        public RobotsTxtController(IRobotsTxtService robotsTxtService)
        {
            _robotsTxtService = robotsTxtService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(_robotsTxtService.GetContent());
        }

        [HttpGet]
        public IActionResult GetByDomainId(int domainId)
        {
            var content = _robotsTxtService.GetContent(domainId);
            return new JsonResult(content);
        }

        [HttpPost]
        public IActionResult Save(RobotsTxtSavePostModel model)
        {
            var content = model.Content ?? string.Empty;
            
            // After saving the content with any validation errors, the user gets the option to save without validation.
            if (!model.SkipValidation)
            {
                var validationErrors = _robotsTxtService.Validate(content).ToArray();
                if (validationErrors.Length > 0)
                {
                    return BadRequest(validationErrors.Select(it => new RobotsTxtValidationViewModel(it)));
                }
            }

            if (model.DomainId.HasValue)
            {
                _robotsTxtService.SetContent(content, model.DomainId.Value);
                return GetByDomainId(model.DomainId.Value);
            }

            _robotsTxtService.SetContent(content);
            return Get();
        }
    }
}
