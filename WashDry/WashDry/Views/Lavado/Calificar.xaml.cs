using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calificar : ContentPage
    {
        private int sid;
        public Calificar(int id)
        {
            InitializeComponent();
            sid = id;
        }

        protected override async void OnAppearing()
        {

            await GetWasherInfo();
        }

        private async Task GetWasherInfo()
        {
            HttpClient client1 = new HttpClient();
            var info = await client1.GetAsync("http://washdryapp.com/app/public/solicitud/lista_solicitud/" + sid);

            if (info.IsSuccessStatusCode)
            {
                var responsew = await info.Content.ReadAsStringAsync();
                var jsonw = JsonConvert.DeserializeObject<List<Solicitudes>>(responsew);

                var idw = jsonw[0].id_washer;

                HttpClient client = new HttpClient();
                var wresinfo = await client.GetAsync("http://www.washdryapp.com/app/public/washer/perfil/" + jsonw[0].id_washer);

                if (wresinfo.IsSuccessStatusCode)
                {
                    var responseW2 = await wresinfo.Content.ReadAsStringAsync();
                    if (responseW2 == "[]")
                    {
                        winfo.Text = "Nombre y otro datos de washer no disponible en este momento";

                    }
                    else { 
                    var jsonw2 = JsonConvert.DeserializeObject<List<Washer>>(responseW2);
                        winfo.Text = "Tu servicio fue atendido por : "+ jsonw2[0].nombre+ " "+jsonw2[0].app+" " + " " + jsonw2[0].apm + " ";
                            }
                }
                else
                {
                    winfo.Text = "Nombre y otro datos de washer no disponible en este momento";
                }
            }
            else
            {

                winfo.Text = "Error infode washer no disponible en este momento";

            }
        }

        //solicitud/califica
        private void RatingConception_Voted(object sender, EventArgs e)
        {
            
        }


        private async Task CalificarServicio()
        {
            HttpClient client = new HttpClient();

            var calif = ratingStar.Value;
            var commentarios = Message.Text;
            var idx = 0;

            var value_check = new Dictionary<string, string>
                         {
                            { "id_solicitud","" },
                            { "id_solicitud","" }
                         };

            var contentx = new FormUrlEncodedContent(value_check);
            var calificar = await client.PostAsync("http://washdryapp.com/app/public/solicitud/califica", contentx);

            if (calificar.IsSuccessStatusCode) {
                await DisplayAlert("Mucgas gracias por tus comentarios", "Nos vemos en tu proximo lavado 🧼🚗.", "ok");
                Application.Current.MainPage = new MainPage();
            }
            else
            {

                await DisplayAlert("Error", "Pudo haber un error intente mas tarde.", "ok");

            }



        }
    }
}