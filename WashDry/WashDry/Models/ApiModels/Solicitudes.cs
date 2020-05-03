using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WashDry.Models.ApiModels
{
   public class Solicitudes
    {

        [PrimaryKey,AutoIncrement]
        public int id { get; set; }
        public string id_solicitud { get; set; }
        public string id_usuario { get; set; }
        public string id_washer { get; set; }
        public string id_paquete { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string fecha { get; set; }
        public string precio { get; set; }
        public string forma_pago { get; set; }
        public string cambio { get; set; }
        public string calificacion { get; set; }
        public string comentario { get; set; }
        public string foto_washer { get; set; }
        public string status { get; set; }
        public string placas { get; set; }
        public string modelo { get; set; }
        public string ann { get; set; }
        public string paquete { get; set; }
        public string usuario { get; set; }
        public string foto { get; set; }
         
    }
}
