using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MapsKontroller
{
    class GoogleMatrix
    {
        public string StartAdresse { get; set; }
        public string EndAdresse { get; set; }

        public GoogleMatrix(string startadresse, string endadresse)
        {
            StartAdresse = startadresse;
            EndAdresse = endadresse;
        }

        public void GetData()
        {
            string key = "";
            WebClient client = new WebClient();
            string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={StartAdresse}&destinations={EndAdresse}&mode=Car&language=dk-DK&key={key}";
            string jsonRaw = client.DownloadString(url);

            var data = JsonConvert.DeserializeObject<MapRootObject>(jsonRaw);
            Console.WriteLine(jsonRaw);
            //string km = data.rows.Select(x => x.elements.Select(y => y.distance.text)).ToString();
            //Console.WriteLine(km);


            foreach (var row in data.rows)
            {
                foreach (var element in row.elements)
                {
                    Console.WriteLine(element.distance.text);
                }

            }

        }
    }
}
