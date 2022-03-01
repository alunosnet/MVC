namespace MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quartos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quartos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        piso = c.Int(nullable: false),
                        lotacao = c.Int(nullable: false),
                        custo_dia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        casa_banho = c.Boolean(nullable: false),
                        estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Quartos");
        }
    }
}
