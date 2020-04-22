using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calificar : ContentPage
    {
        public Calificar()
        {
            InitializeComponent();
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