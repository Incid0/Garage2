namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        RegNr = c.String(nullable: false, maxLength: 6),
                        Type = c.Int(nullable: false),
                        Brand = c.String(),
                        Model = c.String(),
                        Color = c.String(),
                        Wheels = c.Int(nullable: false),
                        EntryTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RegNr);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
        }
    }
}
