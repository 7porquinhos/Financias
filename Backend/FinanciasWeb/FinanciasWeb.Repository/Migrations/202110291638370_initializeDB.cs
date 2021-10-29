namespace FinanciasWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initializeDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(unicode: false),
                        Telefone = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        CEP = c.String(unicode: false),
                        EnderecoCliente = c.String(unicode: false),
                        Cidade = c.String(unicode: false),
                        EnderecoEvento = c.String(unicode: false),
                        DataEvento = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parcela",
                c => new
                    {
                        Cliente_Id = c.Int(nullable: false),
                        Produto_Id = c.Int(nullable: false),
                        Venda_Id = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        NumParcela = c.String(unicode: false),
                        ValorParcela = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Reembolso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataRecebimento = c.DateTime(nullable: false, precision: 0),
                        Desconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DescontoMulta = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataCompetencia = c.String(unicode: false),
                        ValorPendente = c.String(unicode: false),
                    })
                .PrimaryKey(t => new { t.Cliente_Id, t.Produto_Id, t.Venda_Id })
                .ForeignKey("dbo.Cliente", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Produto", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Venda", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Produto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(unicode: false),
                        Valor = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Venda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ValorVenda = c.String(unicode: false),
                        DataVenda = c.DateTime(nullable: false, precision: 0),
                        Cliente_Id = c.Int(nullable: false),
                        Produto_Id = c.Int(nullable: false),
                        Cliente_Id1 = c.Int(),
                        Produto_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.Cliente_Id1)
                .ForeignKey("dbo.Produto", t => t.Produto_Id1)
                .Index(t => t.Cliente_Id1)
                .Index(t => t.Produto_Id1);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(unicode: false),
                        Telefone = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        Senha = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Venda", "Produto_Id1", "dbo.Produto");
            DropForeignKey("dbo.Parcela", "Id", "dbo.Venda");
            DropForeignKey("dbo.Venda", "Cliente_Id1", "dbo.Cliente");
            DropForeignKey("dbo.Parcela", "Id", "dbo.Produto");
            DropForeignKey("dbo.Parcela", "Id", "dbo.Cliente");
            DropIndex("dbo.Venda", new[] { "Produto_Id1" });
            DropIndex("dbo.Venda", new[] { "Cliente_Id1" });
            DropIndex("dbo.Parcela", new[] { "Id" });
            DropTable("dbo.Usuario");
            DropTable("dbo.Venda");
            DropTable("dbo.Produto");
            DropTable("dbo.Parcela");
            DropTable("dbo.Cliente");
        }
    }
}
