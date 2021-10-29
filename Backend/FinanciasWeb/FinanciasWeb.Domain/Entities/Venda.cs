using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanciasWeb.Domain.Entities
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }
        public string ValorVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public int Cliente_Id { get; set; }
        public Cliente Cliente { get; set; }
        public int Produto_Id { get; set; }
        public Produto Produto { get; set; }
        public List<Parcela> Parcelas { get; set; }
    }
}
