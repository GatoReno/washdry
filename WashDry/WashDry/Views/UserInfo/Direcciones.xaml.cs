using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.UserInfo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Direcciones : ContentPage
    {
        public Direcciones()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing() {


            if (!CrossConnectivity.Current.IsConnected)
            {

                //ErrorBtn.IsVisible = true;
                await DisplayAlert("Error", "Por favor activa tus datos o conectate a una red", "ok");

            }
            if (!CrossGeolocator.IsSupported)
            {
                await DisplayAlert("Error", "Ha habido un error con el GPS verifique que este cuente con este servicio activado", "ok");

            }


            var pos = await CrossGeolocator.Current.GetPositionAsync();

            Latitud.Text = pos.Latitude.ToString();
            Longitud.Text = pos.Longitude.ToString();

        }

        private async void btnAddDirecc_Clicked(object sender, EventArgs e)
        {
            var descx = desc.Text;

            if (descx.Length <= 0)
            {
                desc.Focus();
            }

            else if (Latitud.Text.Length <= 0)
            {
                await DisplayAlert("Error", "Ha habido un error con el GPS verifique que este cuente con este servicio activado", "ok");
            }

            else if (Longitud.Text.Length <= 0)
            {
                await DisplayAlert("Error", "Ha habido un error con el GPS verifique que este cuente con este servicio activado", "ok");
            }
            else {


                StringContent id_usuario = new StringContent("10101");
                StringContent latitude = new StringContent(Latitud.Text);
                StringContent longitude = new StringContent(Longitud.Text);
                StringContent descripcion = new StringContent(descx);

                var content = new MultipartFormDataContent();
                content.Add(id_usuario, "id_usuario");
                content.Add(latitude, "latitude");
                content.Add(longitude, "longitude");
                content.Add(descripcion, "descripcion");

                var httpClient = new System.Net.Http.HttpClient();

                var url = "http://www.washdryapp.com/app/public/direccion/guardar";

                var responseMsg = await httpClient.PostAsync(url, content);
                // ... subir a internet


                switch (responseMsg.StatusCode)
                {

                    case System.Net.HttpStatusCode.BadRequest:
                        await DisplayAlert("error", "error status 400 Unauthorized", "ok");
                        break;

                    case System.Net.HttpStatusCode.Forbidden:
                        await DisplayAlert("error", "error status 403  ", "ok");
                        break;

                    case System.Net.HttpStatusCode.NotFound:
                        await DisplayAlert("error", "error status 404  ", "ok");
                        break;

                    case System.Net.HttpStatusCode.OK:
                     
                        string xjson = await responseMsg.Content.ReadAsStringAsync();
                        await DisplayAlert("error", "yeah status 200 : "+xjson, "ok");
                        break;

                    case System.Net.HttpStatusCode.RequestEntityTooLarge:
                        await DisplayAlert("error", "error status 413  ", "ok");
                        break;
                    case System.Net.HttpStatusCode.RequestTimeout:
                        await DisplayAlert("error", "error status 408  ", "ok");
                        break;

                    case System.Net.HttpStatusCode.Unauthorized:
                        await DisplayAlert("error", "yeah status 401 Unauthorized", "ok");
                        break;

                }

            }


        }
    }
}