namespace Domain.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Passports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lastname = c.String(),
                        Firstname = c.String(),
                        Secondname = c.String(),
                        Sex = c.Int(nullable: false),
                        IssuedBy = c.String(),
                        IssuedOn = c.DateTime(nullable: false),
                        IssuedDepartment = c.String(),
                        Series = c.String(),
                        Number = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Passports");
        }
    }
}
