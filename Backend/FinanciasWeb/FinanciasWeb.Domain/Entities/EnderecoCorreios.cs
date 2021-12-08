using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanciasWeb.Domain.Entities
{
    public class EnderecoCorreios
    {
        public int Id { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }

        public string KeyCEP { get; set; }

    }
}
