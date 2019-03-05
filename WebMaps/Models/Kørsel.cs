using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaps.Models
{
    public class Kørsel
    {
        public int IDkørsel { get; set; }

        
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Vælg venligst en dato")]
        public DateTime? Dato { get; set; }


        [Required(ErrorMessage = "Vælg venligst en Start Adresse")]
        public string StartAdresse { get; set; }

        [Required(ErrorMessage = "Vælg venligst en Slut Adresse")]
        public string SlutAdresse { get; set; }

        [Required(ErrorMessage = "Vælg venligst et formål")]
        public string Formål { get; set; }

        [Required(ErrorMessage = "Vælg venligst om det er retur")]
        public string TurRetur { get; set; }

        [Required(ErrorMessage = "Vælg venligst hvor mange gentagelser")]
        public int? Gentagelser { get; set; }

        public string KM { get; set; }


    }
}
