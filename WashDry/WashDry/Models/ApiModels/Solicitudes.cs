using System;
using System.Collections.Generic;
using System.Text;

namespace WashDry.Models.ApiModels
{
   public class Solicitudes
    {
        public string id_solicitud { get; set; }
        public string id_paquete { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string foto { get; set; }
        public string fecha { get; set; }
        public string calificacion { get; set; }
        public string comentario { get; set; }
        public string status { get; set; }
        public string nombre { get; set; }
        public string app { get; set; }
        public string apm { get; set; }
        public string foto_ine { get; set; }
    }
}
