using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebMaps.Models;

namespace WebMaps.Pages
{
    public class TableModel : PageModel
    {
        [BindProperty]
        public K�rsel K�rsel { get; set; }

        public static int _id { get; set; } = 1;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostInsertToTabel()
        {
            string km = "";
            if (ModelState.IsValid)
            {
                km = await CalculateKM(K�rsel.StartAdresse, K�rsel.SlutAdresse);
                BrugerInformationer.K�rselListe.Add(new K�rsel { IDk�rsel = _id, Dato = K�rsel.Dato, StartAdresse = K�rsel.StartAdresse, SlutAdresse = K�rsel.SlutAdresse, TurRetur = K�rsel.TurRetur, Form�l = K�rsel.Form�l, Gentagelser = K�rsel.Gentagelser, KM = km });
                _id++;
                ModelState.Clear();
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {

            BrugerInformationer.K�rselListe.RemoveAll(r => r.IDk�rsel == id);
            return RedirectToPage("/Index");
        }


        public async Task<string> CalculateKM(string startadresse, string endadresse)
        {
            string antalkm = "";
            string key = "AIzaSyBM4dBYGewehvlFZyudquC5fQnPmxoblhc";
            WebClient client = new WebClient();
            string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={startadresse}&destinations={endadresse}&mode=Car&language=dk-DK&key={key}";
            string jsonRaw = await client.DownloadStringTaskAsync(url);

            var data = JsonConvert.DeserializeObject<MapRootObject>(jsonRaw);
            foreach (var row in data.rows)
            {
                foreach (var element in row.elements)
                {
                    antalkm = element.distance.text;
                }
            }
            return antalkm;
        }
    }
}