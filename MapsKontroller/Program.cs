using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace MapsKontroller
{
    class Program
    {

        static string url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=Vancouver+BC&destinations=San+Francisco&mode=Car&language=dk-DK&key=AIzaSyBM4dBYGewehvlFZyudquC5fQnPmxoblhc";
        static void Main(string[] args)
        {
            GoogleMatrix google = new GoogleMatrix("Vejle", "Kalundborg");
            google.GetData();

            //CallGoggleJson();
            //CallGoggle();
            Console.ReadLine();

            
            
        }

        static void CallGoggleJson()
        {
            dynamic result = JsonConvert.DeserializeObject<dynamic>(url);
            Console.WriteLine();
            
        }

        static void CallGoggle()
        {
            WebClient mapsclient = new WebClient();
            string stream = mapsclient.DownloadString(url);
            Console.WriteLine(stream);
        }
    }
}
