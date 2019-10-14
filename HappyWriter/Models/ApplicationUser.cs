using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Vorname { get; set; }
        public string Name { get; set; }
        public string Strasse { get; set; }
        public string Wohnort { get; set; }
        public int Postleitzahl { get; set; }
    }
}
