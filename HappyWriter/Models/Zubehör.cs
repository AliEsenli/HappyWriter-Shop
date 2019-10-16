using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Models
{
    public class Zubehör
    {
        [Key]
        public int ZubehörId { get; set; }
        public string ZubehörName { get; set; }
        public decimal ZubehörKosten { get; set; }
    }
}
