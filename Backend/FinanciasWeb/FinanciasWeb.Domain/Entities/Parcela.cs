using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanciasWeb.Domain.Entities
{
    public class Parcela
    {
        [Key]
        public int Id { get; set; }
        public string NumParcela { get; set; }
        public decimal ValorParcela { get; set; }
        public decimal Reembolso { get; set; }
        public DateTime DataRecebimento { get; set; }
        public decimal Desconto { get; set; }
        public decimal DescontoMulta { get; set; }
        public string DataCompetencia { get; set; }
        public string ValorPendente { get; set; }
        public int Cliente_Id { get; set; }
        public Cliente Cliente { get; set; }
        public int Venda_Id { get; set; }
        public Venda Venda { get; set; }
        public int Produto_Id { get; set; }
        public Produto Produto { get; set; }
    }
}
