using System;
using System.Collections.Generic;
using System.Text;

namespace WashDry.Models.ApiModels
{
   public class Washer
    {
        public string id_washer { get; set; }
        public string id_usuario { get; set; }
        public string nombre { get; set; }
        public string app { get; set; }
        public string apm { get; set; }
        public string telefono { get; set; }
        public string foto_ine { get; set; }
        public string fca_nacimiento { get; set; }
        public string email { get; set; }
        public string foto { get; set; }
        public string calificacion { get; set; }
    }
}
