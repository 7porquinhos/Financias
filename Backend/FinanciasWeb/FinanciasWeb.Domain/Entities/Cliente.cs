using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanciasWeb.Domain.Entities
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CEP { get; set; }
        public string EnderecoCliente { get; set; }
        public string Cidade { get; set; }
        public string EnderecoEvento { get; set; }
        public DateTime DataEvento { get; set; }
    }
}
