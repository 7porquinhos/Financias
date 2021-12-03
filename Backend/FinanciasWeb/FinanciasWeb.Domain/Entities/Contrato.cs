using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanciasWeb.Domain.Entities
{
    public class Contrato
    {
        [Key]
        public int Id { get; set; }
        public int Cliente_Id { get; set; }
        public Cliente Cliente { get; set; }
        public int Evento_Id { get; set; }
        public Evento Evento { get; set; }
    }
}
