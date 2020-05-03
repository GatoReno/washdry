using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
using WashDry.SQLiteDb;
using WashDry.Views.Lavado;
using WashDry.Views.RegistCar;
using WashDry.Views.Servicio;
using WashDry.Views.UserInfo;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WashDry.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]

    public partial class detail : ContentPage
    {
        public UserDataBase userDataBase;
        public detail()
        {
            InitializeComponent();
            _ = GetSolicitudesfromWeb();

        
          


        }
        private async Task GetSolicitudesfromWeb()
        {
            try
            {
                userDataBase = new UserDataBase();
                HttpClient client = new HttpClient();
                var id = userDataBase.GetMembers().ToList();
                
                var getsol = await client.GetAsync("http://www.washdryapp.com/app/public/solicitud/listado/"+id[0].id_cliente);
                if (getsol.IsSuccessStatusCode)
                {
                    HttpContent respx = getsol.Content;
                    var res = await respx.ReadAsStringAsync();
                    var respjson_sol = JsonConvert.DeserializeObject<List<Solicitudes>>(res);

                    if (respjson_sol.Count > 0)
                    {
                        foreach (var item in respjson_sol)
                        {

                            var exist = userDataBase.GetSolicitud(Int32.Parse(item.id_solicitud));
                            if (exist.Count() > 0)
                            {

                            }
                            else { 

                            Solicitudes solicitudx = new Solicitudes();
                            solicitudx = item;
                            solicitudx.calificacion = "0";
                            solicitudx.ann = "";
                            solicitudx.id_washer = "";
                            solicitudx.id_usuario = "";
                            solicitudx.cambio = "";
                            solicitudx.forma_pago = "";
                            solicitudx.foto_washer = "";
                            solicitudx.comentario = "";
                            solicitudx.modelo = "";
                            solicitudx.placas = "";
                            solicitudx.paquete = "";
                            solicitudx.usuario = "";

                            solicitudx.precio = "";

                            userDataBase.AddSolicitudes(solicitudx);
                            } 

                        }

                    }


                }
                else
                {
                    await DisplayAlert("Error", "Error con las solicitudess, intenten en otro momento. Verifique sus datos o wifi", "ok");
                }

                userDataBase = new UserDataBase();
                var solicitudes = userDataBase.GetSolicitudes().ToList();

                if (solicitudes.Count() > 0)
                {
                    ListSolicitudes.ItemsSource = solicitudes;
                    ListSolicitudes.IsVisible = true;
                }
                else
                {

                    ListSolicitudes.IsVisible = false;
                }
            }
            catch (Exception es)
            {

                await DisplayAlert("", es.ToString(), "ok");
            }
         

        }
        protected override async void OnAppearing()
        {

            if (!CrossConnectivity.Current.IsConnected)
            {

                //ErrorBtn.IsVisible = true;
                await DisplayAlert("Error", "Por favor activa tus datos o conectate a una red", "ok");

            }
            if (!CrossGeolocator.IsSupported)
            {
                await DisplayAlert("Error", "Ha habido un error con el plugin", "ok");

            }
            userDataBase = new UserDataBase();
            var solicitudes = userDataBase.GetSolicitudes().ToList();
            Mapx.Pins.Clear();
            if (solicitudes.Count() > 0)
            {
                ListSolicitudes.ItemsSource = solicitudes;
                _ = CurrentLocation();
                ListSolicitudes.IsVisible = true;
            }
            else
            {
                _ = CurrentLocation();
                ListSolicitudes.IsVisible = false;
            }

        }


        public async Task CurrentLocation()
        {

            var pos = await CrossGeolocator.Current.GetPositionAsync();


            Mapx.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Position(pos.Latitude, pos.Longitude), Distance.FromMiles(1)));


            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(pos.Latitude, pos.Longitude),
                Label = "Mi ubicacion",
                Address = "  usted se encuentra aqui",

            };
            Mapx.Pins.Add(pin);
        }
        private async void Lavadobtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Solicitar());
            //   await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new LavadoAlInstante());
        }

        private async void Direccionbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Direcciones());
        }
      


        private async void AddCarbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistCarPage());
        }

        private async void Registbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistCarPage());
        }

        private async void Agendarbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Solicitar());
        }

        private async void CarListbtrn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListCars());
        }
     
 
        private async void btnAgendar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Agendar());
        }

        private async void ListSolicitudes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Solicitudes;
            await Navigation.PushAsync(new EstadoDeServicio(Int32.Parse(content.id_solicitud), content.id));
        }
    }
}