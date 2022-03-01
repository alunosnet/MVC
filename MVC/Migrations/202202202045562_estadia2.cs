namespace MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estadia2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Estadias",
                c => new
                    {
                        EstadiaID = c.Int(nullable: false, identity: true),
                        data_entrada = c.DateTime(nullable: false),
                        data_saida = c.DateTime(nullable: false),
                        valor_pago = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClienteID = c.Int(nullable: false),
                        QuartoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EstadiaID)
                .ForeignKey("dbo.Clientes", t => t.ClienteID, cascadeDelete: true)
                .ForeignKey("dbo.Quartos", t => t.QuartoID, cascadeDelete: true)
                .Index(t => t.ClienteID)
                .Index(t => t.QuartoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Estadias", "QuartoID", "dbo.Quartos");
            DropForeignKey("dbo.Estadias", "ClienteID", "dbo.Clientes");
            DropIndex("dbo.Estadias", new[] { "QuartoID" });
            DropIndex("dbo.Estadias", new[] { "ClienteID" });
            DropTable("dbo.Estadias");
        }
    }
}
