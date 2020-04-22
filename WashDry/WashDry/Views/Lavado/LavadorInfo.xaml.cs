using LaavorRatingConception;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
using WashDry.SQLiteDb;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LavadorInfo : ContentPage
    {
            public UserDataBase userDataBase;
        public int idx;
        public LavadorInfo()
        {
            InitializeComponent();
            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();
            idx = user_exist[0].id_cliente;

            _ = getSolicitudStatis();


        }
        private async   Task getSolicitudStatis()
        {
            HttpClient client = new HttpClient();
            var getsol = await client.GetAsync("http://www.washdryapp.com/app/public/solicitud/lista_solicitud/"+idx);
            if (getsol.IsSuccessStatusCode)
            {
                var response = await getsol.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<List<Solicitudes>>(response);

                ratingStar.InitialValue = Int32.Parse(json[0].calificacion);
                imgwash.Source = json[0].foto_washer;

                HttpClient client1 = new HttpClient();
                var getw = await client1.GetAsync("http://www.washdryapp.com/app/public/washer/perfil/" + json[0].id_washer);

                if (getw.IsSuccessStatusCode)
                {
                    var responsew = await getsol.Content.ReadAsStringAsync();
                    var jsonw = JsonConvert.DeserializeObject<List<Washer>>(responsew);

                    washername.Text = jsonw[0].nombre + " ";
                }
                else
                {

                    washername.Text = "Nombre y otro datoss de washer no disponible en este momento";

                }

            }
            else
            {


            }
        
        }
        private void RatingConception_Voted(object sender, EventArgs e)
        {
            RatingConception rating = (RatingConception)sender;
            int index = rating.IndexVoted;
            int value = rating.Value;

           
            // rating.InitialValue = 2; sobres así se asigna por default e buen rating

            //http://www.washdryapp.com/app/public/solicitud/lista_solicitud/


        }
    }
}