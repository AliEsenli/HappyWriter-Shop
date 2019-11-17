using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Models.ViewModels
{
    public class CheckoutViewModel
    {
        private readonly ApplicationUser user;
        private readonly Produkt produkt;

        public CheckoutViewModel(ApplicationUser user, Produkt produkt, List<KundeZubehör> zubehöre)
        {
            this.user = user;
            this.produkt = produkt;

            if (zubehöre != null)
            {
                this.Zubehöre = zubehöre;
                TotalKostenZubehör = Zubehöre.Sum(z => z.Zubehör.ZubehörKosten);
            }
        }

        public string UserName => user.Name;
        public string UserVorname => user.Vorname;
        public string Strasse => user.Strasse;
        public int Plz => user.Postleitzahl;
        public string Ort => user.Wohnort;
        public string ProduktName => produkt.Name;
        public decimal ProduktKosten => produkt.Kosten;
        public decimal TotalKostenZubehör;
        public List<KundeZubehör> Zubehöre { get; }
    }
}
