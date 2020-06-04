namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnaApellido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Apellido", c => c.String(maxLength: 120));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Apellido");
        }
    }
}
