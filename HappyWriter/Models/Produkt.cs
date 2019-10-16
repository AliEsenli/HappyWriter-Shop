using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Models
{
    public class Produkt
    {
        [Key]
        public int ProduktId { get; set; }
        public string Name { get; set; }
        public decimal Kosten { get; set; }
    }
}
