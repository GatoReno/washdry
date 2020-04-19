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
using WashDry.SQLiteDb;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Agendar : ContentPage
    {
        public int idx;
        public UserDataBase db;
        public UserDataBase usr;
        public Agendar()
        {
            InitializeComponent();


            db = new UserDataBase();
            var user_exist = db.GetMembers().ToList();
            idx = user_exist[0].id_cliente;


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

            await GetDirecciones();
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


        public async Task GetDirecciones()
        {

            HttpClient client = new HttpClient();
            var uri = "http://washdryapp.com/app/public/direccion/listado/" + idx;

            try
            {

                var response = await client.GetAsync(uri);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.InternalServerError:
                        Console.WriteLine("----------------------------------------------_____:Here status 500");

                        //xlabel.Text = "error 500";
                        // Cator.IsVisible = false;
                        break;


                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine("----------------------------------------------_____:Here status 200");

                        // ylabel.Text = "Ultimas noticas de proyectos";
                        HttpContent content = response.Content;
                        var xjson = await content.ReadAsStringAsync();

                        if (xjson == "[]" || xjson == null)
                        {
                            direccionesPicker.IsVisible = false;    
                        }
                        else{
                            var result = JsonConvert.DeserializeObject<List<DireccionesApiCall>>(xjson);
                            direccionesPicker.IsVisible = true;


                            direccionesPicker.ItemsSource = result;
                        }

                        //  var json_ = JsonConvert.DeserializeObject<List<VisitasMod>>(xjson);
                        // direccionesPicker.ItemsSource = json_;
                        // Cator.IsVisible = false;



                        break;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error - Intente en otro momento", "" + ex.ToString(), "ok");
                //  Cator.IsVisible = false;

                //  CatorT.Text = "Ha habido un error";
                return;
            }



        }
    }
}