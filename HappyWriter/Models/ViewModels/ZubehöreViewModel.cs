using HappyWriter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Models.ViewModels
{
    public class ZubehöreViewModel
    {
        private readonly Produkt produkt;
        private readonly ApplicationDbContext context;
        public ZubehöreViewModel(Produkt produkt, ApplicationDbContext context)
        {
            this.produkt = produkt;
            this.context = context;
            this.Zubehör = context.Zubehöre.ToList();
        }

        public ICollection<Zubehör> Zubehör { get; }
        public string Name => produkt.Name;
        public int ProduktId => produkt.ProduktId;
        public decimal Kosten => produkt.Kosten;
    }
}
