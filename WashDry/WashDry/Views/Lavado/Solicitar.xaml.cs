using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Solicitar : ContentPage
    {
        public string StepSelected { get; set; }
        public int idx;

        public User user;
        public UserDataBase userDataBase;

        public Solicitar()
        {
            InitializeComponent();
             stepBar.Opacity = 0;
            stepBar.FadeTo(1,1000,null);
            stepBar.ScaleTo(1, 1000);
            //local: StepProgressBarControl
            frame1.IsVisible = true;
            frame2.IsVisible = false;
            frame3.IsVisible = false;
            frame4.IsVisible = false;
            frame5.IsVisible = false;

            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();

            idx = Int32.Parse(user_exist[0].id);
            _ = getdireccion(); _ = getAutos();
        }


        private void PickerPickerAuto_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                var i = (AutosModel)PickerDirecc.SelectedItem;
                var id = i.id_auto.ToString();
                id_auto.Text = id;
               
             

            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Intente en otro momento _ error: " + ex.ToString() + " _ ", "ok");
            }

        }



        private void PickerProductos_SelectedIndexChanged(System.Object sender, System.EventArgs e) { 
            try
            {
                var i = (DireccionesApiCall)PickerDirecc.SelectedItem;
                var id = i.id_direccion.ToString();
                id_loc.Text = id;
                longitude.Text = i.longitud;
                desc.Text = i.descripcion;
                latitud.Text = i.latitud;
                _ = SetCurrentLocation();

            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Intente en otro momento _ error: " + ex.ToString() + " _ ", "ok");
            }

        }

        private async Task getdireccion()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var uri = "http://washdryapp.com/app/public/direccion/listado/"+ idx;
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(uri);
                var client = new HttpClient();



                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);

                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.InternalServerError:
                            Console.WriteLine("----------------------------------------------_____:Here status 500");


                            break;
                        case System.Net.HttpStatusCode.OK:
                            Console.WriteLine("----------------------------------------------_____:Here status 200");

                            try
                            {
                                HttpContent content = response.Content;
                                string xjson = await content.ReadAsStringAsync();

                                PickerDirecc.TextColor = Color.FromHex("#4E8F75");
                                PickerDirecc.TitleColor = Color.FromHex("#4E8F75");

                                var result = JsonConvert.DeserializeObject<List<DireccionesApiCall>>(xjson);
                                PickerDirecc.ItemsSource = result;
                                PickerDirecc.SelectedIndexChanged += PickerProductos_SelectedIndexChanged;






                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("", "" + ex.ToString(), "ok");
                                var x = ex.ToString();

                                return;
                            }
                            break;
                        case System.Net.HttpStatusCode.NotFound:

                            await DisplayAlert("error 404", "servidor no encontrado ", "ok");
                            break;
                    }
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Error", "Intente en otro momento _ error: " + ex.ToString() + " _ ", "ok");
                }
            }
        }

        private async Task getAutos()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var uri = "http://washdryapp.com/app/public/auto/listadoAutoUser/" + idx;
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(uri);
                var client = new HttpClient();



                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);

                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.InternalServerError:
                            Console.WriteLine("----------------------------------------------_____:Here status 500");


                            break;
                        case System.Net.HttpStatusCode.OK:
                            Console.WriteLine("----------------------------------------------_____:Here status 200");

                            try
                            {
                                HttpContent content = response.Content;
                                string xjson = await content.ReadAsStringAsync();

                                PickerAuto.TextColor = Color.FromHex("#4E8F75");
                                PickerAuto.TitleColor = Color.FromHex("#4E8F75");

                                var result = JsonConvert.DeserializeObject<List<DireccionesApiCall>>(xjson);
                                PickerAuto.ItemsSource = result;
                                PickerAuto.SelectedIndexChanged += PickerPickerAuto_SelectedIndexChanged;






                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("", "" + ex.ToString(), "ok");
                                var x = ex.ToString();

                                return;
                            }
                            break;
                        case System.Net.HttpStatusCode.NotFound:

                            await DisplayAlert("error 404", "servidor no encontrado ", "ok");
                            break;
                    }
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Error", "Intente en otro momento _ error: " + ex.ToString() + " _ ", "ok");
                }
            }
        }

     
        public async Task CurrentLocation()
        {
            Mapx.Pins.Clear();
            Mapx.IsVisible = true;
            var pos = await CrossGeolocator.Current.GetPositionAsync();


            Mapx.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Position(pos.Latitude, pos.Longitude), Distance.FromMiles(1)));


            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(pos.Latitude, pos.Longitude),
                Label = desc.Text,
                Address = " solicitar lavado aqui",

            };
            Mapx.Pins.Add(pin);
        }



        public async Task SetCurrentLocation()
        {
            Mapx.Pins.Clear();
            Mapx.IsVisible = true;
            var pos = await CrossGeolocator.Current.GetPositionAsync();

             
         
            Mapx.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Position(Double.Parse(latitud.Text), Double.Parse(longitude.Text)), Distance.FromMiles(1)));


            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(Double.Parse(latitud.Text), Double.Parse(longitude.Text)),
                Label = "Mi ubicacion",
                Address = "  usted se encuentra aqui",

            };
            Mapx.Pins.Add(pin);
        }

        private void lblmain_TextChanged(object sender, TextChangedEventArgs e)
        {
          
            var id = lblmain.Text;
            var idx = Int32.Parse(id);
            switch (idx)
            {
                default:
                    break;

                case 1:
                    frame1.IsVisible = true;
                    frame2.IsVisible = false;
                    frame3.IsVisible = false;
                    frame4.IsVisible = false;
                    frame5.IsVisible = false;


                    break;


                case 2:
                    frame1.IsVisible = false;
                    frame2.IsVisible = true;
                    frame3.IsVisible = false;
                    frame4.IsVisible = false;
                    frame5.IsVisible = false;
                    break;


                case 3:
                    frame1.IsVisible = false;
                    frame2.IsVisible = false;
                    frame3.IsVisible = true;
                    frame4.IsVisible = false;
                    frame5.IsVisible = false;
                    break;


                case 4:
                    frame1.IsVisible = false;
                    frame2.IsVisible = false;
                    frame3.IsVisible = false;
                    frame4.IsVisible = true;
                    frame5.IsVisible = false;
                    break;


                case 5:
                    frame1.IsVisible = false;
                    frame2.IsVisible = false;
                    frame3.IsVisible = false;
                    frame4.IsVisible = false;
                    frame5.IsVisible = true;
                    break;
            }

        }

        

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            _ = CurrentLocation();
        }

        protected override void OnAppearing() {

            stepBar.Opacity = 0;
            stepBar.FadeTo(1, 1000, null);
            stepBar.ScaleTo(1, 1000);
            //local: StepProgressBarControl
            frame1.IsVisible = true;
            frame2.IsVisible = false;
            frame3.IsVisible = false;
            frame4.IsVisible = false;
            frame5.IsVisible = false;


            _ = getdireccion(); _ = getAutos();

        }

        /*    */
    }





}