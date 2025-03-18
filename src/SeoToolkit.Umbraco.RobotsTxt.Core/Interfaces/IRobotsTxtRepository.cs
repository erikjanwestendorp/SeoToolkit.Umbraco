using SeoToolkit.Umbraco.Common.Core.Interfaces;
using SeoToolkit.Umbraco.RobotsTxt.Core.Models.Business;
using System;

namespace SeoToolkit.Umbraco.RobotsTxt.Core.Interfaces
{
    public interface IRobotsTxtRepository : IRepository<RobotsTxtModel>
    {
        RobotsTxtModel FirstOrDefault(Func<RobotsTxtModel, bool> predicate);
    }
}
