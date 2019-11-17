using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HappyWriter.Models;
using HappyWriter.Data;
using HappyWriter.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HappyWriter.Controllers
{
    // Static SessionExtension Helper Class
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<Produkt>("Artikel");
            if (cart != null)
            {
                ViewBag.cart = cart;
            }

            var zubehör = HttpContext.Session.GetObjectFromJson<List<KundeZubehör>>("Zubehör");
            if (zubehör != null)
            {
                ViewBag.zubehör = zubehör.ToList();
            }

            var viewModel = new ProdukteViewModel(context);
            return View(viewModel);
        }

        // GET: Home/Produkt/1
        public async Task<IActionResult> Produkt(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await context.Produkte
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produkt == null)
            {
                return NotFound();
            }

            // Create a View Model pass the product and context
            var ProduktViewModel = new ProduktViewModel(produkt, context);

            // retrun View Model
            return View(ProduktViewModel);
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            var produkt = new Produkt
            {
                ProduktId = id,
                Kosten = context.Produkte.FirstOrDefault(p => p.ProduktId == id).Kosten,
                Name = context.Produkte.FirstOrDefault(p => p.ProduktId == id).Name
            };

            // Serialize Object
            HttpContext.Session.SetObjectAsJson("Artikel", produkt);

            // Redirect to "BuyZubehör",  Controller = HomeController
            return RedirectToAction("BuyZubehör", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> BuyZubehör(int id)
        {
            // Produktliste laden um die Auswahl zu ermöglichen
            var zubehörListe = await context.Zubehöre.ToListAsync();
            ViewBag.ZubehörListe = zubehörListe;

            return View();
        }

        [HttpPost]
        public IActionResult BuyZubehör([FromForm] ICollection<int> zubehörIds)
        {
            var ausgewählteZubehöre = new List<KundeZubehör>();

            foreach (var zubehörId in zubehörIds)
            {
                var selectedZubehör = new Zubehör
                {
                    ZubehörId = zubehörId,
                    ZubehörName = context.Zubehöre.FirstOrDefault(z => z.ZubehörId == zubehörId).ZubehörName,
                    ZubehörKosten = context.Zubehöre.FirstOrDefault(z => z.ZubehörId == zubehörId).ZubehörKosten
                };

                var ausgewählterZubehör = new KundeZubehör { Zubehör = selectedZubehör };

                ausgewählteZubehöre.Add(ausgewählterZubehör);
            }

            HttpContext.Session.SetObjectAsJson("Zubehör", ausgewählteZubehöre);

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Bestellübersicht(int id)
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);

            var produkt = HttpContext.Session.GetObjectFromJson<Produkt>("Artikel");
            var zubehörListe = HttpContext.Session.GetObjectFromJson<List<KundeZubehör>>("Zubehör");

            var viewModel = new CheckoutViewModel(user, produkt, zubehörListe);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Checkout(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);

            var produkt = HttpContext.Session.GetObjectFromJson<Produkt>("Artikel");
            var zubehörListe = HttpContext.Session.GetObjectFromJson<List<KundeZubehör>>("Zubehör");

            // Struktur erstellen welche von EF verstanden wird
            var selectedProducts = new List<KundeProdukt>();
            var selectedZubehöre = new List<KundeZubehör>();

            // ein Verbindungsobjekt mit der KundenId und der selektierten ProduktId erstellen
            var selectedProduct = new KundeProdukt { UserId = user.Id, ProduktId = produkt.ProduktId };
            selectedProducts.Add(selectedProduct);

            if (zubehörListe != null)
            {
                foreach (var zubehör in zubehörListe)
                {
                    var selectedZubehör = new KundeZubehör { UserId = user.Id, ZubehörId = zubehör.Zubehör.ZubehörId };
                    selectedZubehöre.Add(selectedZubehör);
                }
            }
            // die vorbereitete Struktur dem Datenbankkontext übergeben (ab jetzt ist es im Speicher)
            await context.AddRangeAsync(selectedProducts);
            await context.AddRangeAsync(selectedZubehöre);

            // die Veränderungen, welche dem Datenbankkontext bekannt sind, in die DB abspeichern
            await context.SaveChangesAsync();

            // Session beenden und Daten löschen
            HttpContext.Session.Remove("Artikel");
            HttpContext.Session.Remove("Zubehör");
            // auf Checkout-Seite weiterleiten
            return RedirectToAction("Dankeschön", "Home");
        }

        [HttpPost]
        public IActionResult CancelOrder()
        {
            HttpContext.Session.Remove("Artikel");
            HttpContext.Session.Remove("Zubehör");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Dankeschön()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
