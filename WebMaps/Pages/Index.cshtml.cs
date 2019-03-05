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
        public Kørsel Kørsel { get; set; }

        public static int _id { get; set; } = 1;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostInsertToTabel()
        {
            string km = "";
            if (ModelState.IsValid)
            {
                km = await CalculateKM(Kørsel.StartAdresse, Kørsel.SlutAdresse);
                BrugerInformationer.KørselListe.Add(new Kørsel { IDkørsel = _id, Dato = Kørsel.Dato, StartAdresse = Kørsel.StartAdresse, SlutAdresse = Kørsel.SlutAdresse, TurRetur = Kørsel.TurRetur, Formål = Kørsel.Formål, Gentagelser = Kørsel.Gentagelser, KM = km });
                _id++;
                ModelState.Clear();
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {

            BrugerInformationer.KørselListe.RemoveAll(r => r.IDkørsel == id);
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