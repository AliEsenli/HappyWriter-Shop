using HappyWriter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Models.ViewModels
{
    public class ProdukteViewModel
    {
        private readonly ApplicationDbContext context;
        public ProdukteViewModel(ApplicationDbContext context)
        {
            this.context = context;

            this.Produkte = context.Produkte.ToList();
        }

        public ICollection<Produkt> Produkte { get; }
    }
}
