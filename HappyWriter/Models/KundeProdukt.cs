using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Models
{
    public class KundeProdukt
    {
        [Key]
        public int BestellungId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProduktId { get; set; }
        public Produkt Produkt { get; set; }
    }
}
