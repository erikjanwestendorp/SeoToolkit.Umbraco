using Umbraco.Cms.Infrastructure.Migrations;

namespace SeoToolkit.Umbraco.RobotsTxt.Core.Migrations
{
    public class AddDomainIdToRobotsTxtTable : MigrationBase
    {
        public AddDomainIdToRobotsTxtTable(IMigrationContext context) : base(context)
        {
        }

        protected override void Migrate()
        {
            if (!ColumnExists("SeoToolkitRobotsTxt", "domainId"))
            {
                Create.Column("DomainId")
                    .OnTable("SeoToolkitRobotsTxt")
                    .AsInt32()
                    .Nullable()
                    .Do();
            }
        }
    }
}
