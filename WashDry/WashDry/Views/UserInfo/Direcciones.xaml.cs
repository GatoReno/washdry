using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
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
        public async Task GetVisitasWeb()
        {
          

            try
            {
                HttpClient client = new HttpClient();
                var uri = "http://washdryapp.com/app/public/direccion/listado/10101";
                var responseMsg = await client.GetAsync(uri);
             
                
          
                switch (responseMsg.StatusCode)
                {
                    case System.Net.HttpStatusCode.InternalServerError:
                        Console.WriteLine("----------------------------------------------_____:Here status 500");

                        //xlabel.Text = "error 500";
                        // Cator.IsVisible = false;
                        break;


                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine("----------------------------------------------_____:Here status 200");

                        // ylabel.Text = "Ultimas noticas de proyectos";
                       

                        // var json_ = JsonConvert.DeserializeObject<List<VisitasMod>>(xjson);

                        string xjson = await responseMsg.Content.ReadAsStringAsync();
                        //DireccApiCall

                        HttpContent contentD = responseMsg.Content;
                        var xjsonD = await contentD.ReadAsStringAsync();

                        var json_d = JsonConvert.DeserializeObject<List<DireccionesApiCall>>(xjsonD);
                        ListDirecciones.ItemsSource = json_d;

                        break;

                }
           
            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex.ToString(), "ok");
              //  Cator.IsVisible = false;

              //  CatorT.Text = "Ha habido un error";
                return;
            }


        }
        protected override async void OnAppearing() {


            if (!CrossConnectivity.Current.IsConnected)
            {

                //ErrorBtn.IsVisible = true;
                await DisplayAlert("Error", "Por favor activa tus datos o conectate a una red", "ok");


            }
            else
            {
                _ = GetVisitasWeb();

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

            if (string.IsNullOrEmpty(descx))
            {
                await DisplayAlert("Descripcion", "Ingrese una descripcion", "ok");
                desc.Focus();
            }

            else if (string.IsNullOrEmpty(Latitud.Text))
            {
                await DisplayAlert("Error", "Ha habido un error con el GPS verifique que este cuente con este servicio activado", "ok");
            }

            else if (string.IsNullOrEmpty(Longitud.Text))
            {
                await DisplayAlert("Error", "Ha habido un error con el GPS verifique que este cuente con este servicio activado", "ok");
            }
            else {
                try
                {

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

                var url = "http://washdryapp.com/app/public/direccion/guardar";

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
                            await DisplayAlert("200", "error status 404  ", "ok");
                            break;

                        case System.Net.HttpStatusCode.OK:


                            string xjson = await responseMsg.Content.ReadAsStringAsync();
                            await DisplayAlert("error", "yeah status 200 : " + xjson, "ok");


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
                catch (Exception ex)
                {

                    await DisplayAlert("error", "Error : " + ex.ToString(), "ok");
                }
             

            }


        }

        private async void ListDirecciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var dir = e.Item as DireccionesApiCall;
             await DisplayAlert("direccion","   "+dir.descripcion, "ok");

          //  await Navigation.PushAsync(new ProspectoInfo(content_X.index_prospecto));
        }
    }
}