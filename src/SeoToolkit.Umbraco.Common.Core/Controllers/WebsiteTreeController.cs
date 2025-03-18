using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeoToolkit.Umbraco.Common.Core.Constants;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Trees;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.BackOffice.Trees;

namespace SeoToolkit.Umbraco.Common.Core.Controllers
{
    [Tree("SeoToolkit", Alias, TreeTitle = Title, TreeGroup = TreeGroupAlias, SortOrder = 5)]

    public class WebsiteTreeController : TreeController
    {
        public const string Alias = TreeControllerConstants.Website.Alias;
        public const string Title = TreeControllerConstants.Website.Title;
        public const string TreeGroupAlias = TreeControllerConstants.SeoToolkitTreeGroupAlias;

        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IDomainService _domainService;
        

        public WebsiteTreeController(
            ILocalizedTextService localizedTextService,
            UmbracoApiControllerTypeCollection umbracoApiControllerTypeCollection,
            IEventAggregator eventAggregator,
            IUmbracoContextAccessor umbracoContextAccessor, IDomainService domainService)
            : base(localizedTextService, umbracoApiControllerTypeCollection, eventAggregator)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
            _domainService = domainService;
        }

        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, FormCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();
            
            _umbracoContextAccessor.TryGetUmbracoContext(out var context);

            if (id == "-1")
            {
                var domains = _domainService.GetAll(false).GroupBy(x => x.RootContentId).Select(x => x.First());
                foreach (var domain in domains)
                {
                    var node = context?.Content?.GetById(domain.RootContentId ?? -1);
                    if (node is null)
                    {
                        continue;
                    }

                    var newTreeItem = CreateTreeNode(domain.Id.ToString(), "-1", queryStrings, node.Name, "icon-globe", true, "/SeoToolkit");
                    newTreeItem.MenuUrl = null;

                    nodes.Add(newTreeItem);
                }
            }
            else
            {
                var robotsTxtTree = CreateTreeNode($"{id}_robotsTxt", id, queryStrings, TreeControllerConstants.RobotsTxt.Title, "icon-cloud", false);

                robotsTxtTree.RoutePath = $"/SeoToolkit/{TreeControllerConstants.RobotsTxt.Alias}/detail/{robotsTxtTree.ParentId}";
                robotsTxtTree.MenuUrl = null;
                nodes.Add(robotsTxtTree);
            }

            return nodes;
        }

        protected override ActionResult<TreeNode?> CreateRootNode(FormCollection queryStrings)
        {
            var rootResult = base.CreateRootNode(queryStrings);
            if (!(rootResult.Result is null))
            {
                return rootResult;
            }

            var root = rootResult.Value;

            root.Icon = "icon-globe";
            root.HasChildren = true;
            root.MenuUrl = null;

            return root;
        }

        protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, FormCollection queryStrings)
        {
            return new ActionResult<MenuItemCollection>(new EmptyResult());
        }
    }
}