namespace YmtSystem.Domain.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class OrderLine : DbMigration
    {
        public override void Up()
        {           
            AddColumn("ymt_order_line", "ID", c => c.String(name: "ID"));
            AddColumn("ymt_order_line", "ItemPrice", c => c.Decimal());
        }

        public override void Down()
        {
        }
    }
}
