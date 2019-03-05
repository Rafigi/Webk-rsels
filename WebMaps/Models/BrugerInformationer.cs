using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaps.Models
{
    public class BrugerInformationer
    {
        [Required(ErrorMessage = "Indtast venligst et {0}")]
        public string Navn { get; set; }

        [Required(ErrorMessage = "Indtast venligst en {0}")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Indtast venligt et Postnr og By")]
        public string PostBy { get; set; }

        [Required(ErrorMessage = "Vælg venligst en {0}")]
        public string Afdeling { get; set; }

        [Required(ErrorMessage = "Indtast venligst en Mail")]
        [EmailAddress]
        public string Mail { get; set; }


        [Required(ErrorMessage = "Indtast venligst et registringsnummer")]
        public int BankRegnr { get; set; }


        [Required(ErrorMessage = "Indtast venligst et konto nummer")]
        public string BankKontoNr { get; set; }

        public static List<Kørsel> KørselListe = new List<Kørsel>();
    }
}
