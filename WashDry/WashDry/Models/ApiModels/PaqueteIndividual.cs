using System;
using System.Collections.Generic;
using System.Text;

namespace WashDry.Models.ApiModels
{
    public class PaqueteIndividual
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string tipo_vehiculo { get; set; }
        public string id { get; set; }
        public string duracion { get; set; }
        public string precio { get; set; }
    }
}
