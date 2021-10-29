using FinanciasWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanciasWeb.Repository.Data.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext()
            : base("name=FinaciasWeb")
        {
            //Configuration.LazyLoadingEnabled = true;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Parcela>()
                .HasKey(parcela => new { parcela.Cliente_Id, parcela.Produto_Id, parcela.Venda_Id });
        }
    }
}
