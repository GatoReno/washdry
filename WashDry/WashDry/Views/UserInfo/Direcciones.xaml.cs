using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Plugin.Connectivity;
using Plugin.Geolocator;
using QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
using WashDry.Models.DbModels;
using WashDry.SQLiteDb;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.UserInfo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Direcciones : ContentPage
    {


        public User user;
        public UserDataBase userDataBase;
        public Direcciones()
        {
            InitializeComponent();
            _ = CurrentLocation();
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

            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error de conexion", "Asegurece de estar conectado a una red wifi u otro acceso a internet", "ok");
                await Navigation.PopToRootAsync();
            }
            else
            {
                HttpClient client1 = new HttpClient();
                var k = "AIzaSyBoKd3QoJ73KevoPbIgmixgU0Q5hvoK7PI";
                var gv = await client1.GetAsync("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location="+
                    pos.Latitude + ","+ pos.Longitude + "&radius=5&type=formatted_address&keyword=street&key=" + k);

 

                if (gv.IsSuccessStatusCode)
                {
                    var strjsn = await gv.Content.ReadAsStringAsync();
                    var jsond = JsonConvert.DeserializeObject<GooglePlaces>(strjsn);

                    if (jsond.Status == "ZERO_RESULTS")
                    {
                        var gv2 = await client1.GetAsync("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" +
                            pos.Latitude + "," + pos.Longitude + "&radius=15&type=formatted_address&keyword=street&key=" + k);
                        var str2 = await gv2.Content.ReadAsStringAsync();
                        var jsond2 = JsonConvert.DeserializeObject<GooglePlaces>(str2);
                        if (jsond2.Status == "ZERO_RESULTS")
                        {
                            var gv3 = await client1.GetAsync("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" +
                                                        pos.Latitude + "," + pos.Longitude + "&radius=151&type=formatted_address&keyword=street&key=" + k);
                            var str3 = await gv3.Content.ReadAsStringAsync();
                            var jsond3 = JsonConvert.DeserializeObject<GooglePlaces>(str3);
                            if (jsond3.Status == "ZERO_RESULTS")
                            {
                                await DisplayAlert("Error de conexion", "El servidor esta teniendo problemas para encontrar su direccion, intente mas tarde", "ok");
                                await Navigation.PopToRootAsync();
                            }
                            else
                            {
                                var vi = jsond3.Results[0].Vicinity;
                                desc.Text = vi;
                                gp.Text = vi;
                            }
                        }
                        else
                        {
                            var vi = jsond2.Results[0].Vicinity;
                            desc.Text = vi;
                            gp.Text = vi;
                        }
                    }

                    
                    else { 
                    var vi =  jsond.Results[0].Vicinity;
                    desc.Text = vi;
                        gp.Text = vi;
                    }
                }
                else
                {

                    await DisplayAlert("Error de conexion", "El servidor esta teniendo problemas para encontrar su direccion, intente mas tarde", "ok");
                    await Navigation.PopToRootAsync();
                }
            }
          
           
        }

        public async Task GetVisitasWeb()
        {
           

            try
            {

                userDataBase = new UserDataBase();
                var user_exist = userDataBase.GetMembers().ToList();
                HttpClient client = new HttpClient();
                var uri = "http://www.washdryapp.com/app/public/direccion/listado/" + user_exist[0].id;
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

            var gpx = gp.Text;
            if (String.IsNullOrEmpty(gp.Text))
            {
                gpx = "Referencia no encontrada";
            }

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
                    userDataBase = new UserDataBase();
                    var user_exist = userDataBase.GetMembers().ToList();
                    StringContent id_usuario = new StringContent(user_exist[0].id_cliente.ToString());
                    StringContent latitude = new StringContent(Latitud.Text);
                    StringContent longitude = new StringContent(Longitud.Text);
                    StringContent descripcion = new StringContent(descx); 
                        StringContent direccion_gp = new StringContent(descx);
                    var content = new MultipartFormDataContent();
                content.Add(id_usuario, "id_usuario");
                content.Add(latitude, "latitude");
                content.Add(longitude, "longitude");
                content.Add(descripcion, "descripcion");
                    content.Add(direccion_gp, "direccion_gp");

                    var httpClient = new System.Net.Http.HttpClient();


               
                    HttpClient client = new HttpClient();
 
                    var url = "http://www.washdryapp.com/app/public/direccion/guardar" ;
                    //  IsSuccessStatusCode = false


                    var responseMsg = await httpClient.PostAsync(url, content);
                    // ... subir a internet


                    if (responseMsg.IsSuccessStatusCode == false)
                    {
                        await DisplayAlert("error", "error status 419 Probelmas con respuesta del server, intente mas tarde o reinicie la aplicacion", "ok");
                    }
                    else
                    {
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

                                GetVisitasWeb();
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
                catch (Exception ex)
                {

                    await DisplayAlert("error", "Error : " + ex.ToString(), "ok");
                }
             

            }


        }

        private  void ListDirecciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var dir = e.Item as DireccionesApiCall;
           // await DisplayAlert("direccion","   "+dir.descripcion, "ok");
            _ = SetCurrentLocation(dir.latitud,dir.longitud,dir.descripcion);
        }


        public async Task SetCurrentLocation(string latitud, string longitude,string desc)
        {
            Mapx.Pins.Clear();
            Mapx.IsVisible = true;
            var pos = await CrossGeolocator.Current.GetPositionAsync();



            Mapx.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Position(Double.Parse(latitud ), Double.Parse(longitude )), Distance.FromMiles(1)));


            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(Double.Parse(latitud), Double.Parse(longitude)),
                Label = "Mi ubicacion",
                Address = desc,

            };
            Mapx.Pins.Add(pin);
        }

        private void ListDirecciones_ItemTapped_1(object sender, ItemTappedEventArgs e)
        {

        }
    }
}