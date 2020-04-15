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
        public Boolean Efectivo = false;
        public Boolean Tarjeta = false;
        public User user;
        public UserDataBase userDataBase;

        public Solicitar()
        {
            InitializeComponent();
            stepBar.Opacity = 0;
            stepBar.FadeTo(1, 1000, null);
            stepBar.ScaleTo(1, 1000);
            //local: StepProgressBarControl
            frame1.IsVisible = true;
            frame2.IsVisible = false;
            frame3.IsVisible = false;
            frame4.IsVisible = false;
            frame5.IsVisible = false;

            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();

            idx = user_exist[0].id_cliente;
            _ = getdireccion(); _ = getAutos(); _ = CurrentLocation();
            _ = getPaquetes(); _ = getWashers();

            string hour = DateTime.Now.ToString("HH");
            string minute = DateTime.Now.ToString("mm");
            string sencond = DateTime.Now.ToString("ss");

            _timePicker.Time = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(sencond));


          FDPPicker.SelectedIndexChanged += FDPPickerSelectedIndexChanged;
            pickertamanio.SelectedIndexChanged += TamPickerSelected;
        }

        private void TamPickerSelected(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedItem = picker.SelectedItem;
            _ = GetPrice(selectedItem.ToString());
        }

        protected override void OnAppearing()
        {

            stepBar.Opacity = 0;
            stepBar.FadeTo(1, 1000, null);
            stepBar.ScaleTo(1, 1000);
            //local: StepProgressBarControl
            frame1.IsVisible = true;
            frame2.IsVisible = false;
            frame3.IsVisible = false;
            frame4.IsVisible = false;
            frame5.IsVisible = false;



            string hour = DateTime.Now.ToString("HH");
            string minute = DateTime.Now.ToString("mm");
            string sencond = DateTime.Now.ToString("ss");

            _timePicker.Time = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(sencond));
            getWashers();

        }
        private async Task GetPrice(string tipo) {
            var idp = id_paquete.Text;
            if (string.IsNullOrEmpty(idp))
            {
                await DisplayAlert("Elija un paquete", "Por favor elija un paquete para continuar", "ok");

            }
            else {

                var uri = "http://www.washdryapp.com/app/public/paquete/precio_individual";
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(uri);
                var client = new HttpClient();
                var value_check = new Dictionary<string, string>
                         {
                            { "id_paquete", id_paquete.Text},
                            { "tipo" , tipo },
                         

                         };
                var body = new FormUrlEncodedContent(value_check);
                try
                {

                    HttpResponseMessage  response = await client.PostAsync(uri, body);


                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.InternalServerError:
                            Console.WriteLine("----------------------------------------------_____:Here status 500");


                            break;
                        case System.Net.HttpStatusCode.OK:
                            //                                Console.WriteLine("----------------------------------------------_____:Here status 200");

                            try
                            {
                                HttpContent content = response.Content;
                                string xjson = await content.ReadAsStringAsync();


                                var result = JsonConvert.DeserializeObject<List<Precios>>(xjson);
                                var price = result[0].precio;
                                FinalPrice.Text = "$ "+ price+" Mx";
                                lblprice.Text = "cargo : $ " + price + " Mx";
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


        private void FDPPickerSelectedIndexChanged(object sender, EventArgs e)
            {
            var picker = (Picker)sender;

            var itemSelect = picker.SelectedItem;

             
            
            if (itemSelect == "Tarjeta")
                 {
                    modotarjeta.IsVisible = true;
                    lblpayoption.Text = "Modo de pago";
                    vapagarcon.IsVisible = false;

                Tarjeta = true;
                }
             else if (itemSelect == "Efectivo")
             {
                lblpayoption.IsVisible = true;
                lblpayoption.Text = "Con cuanto va a pagar?"; 
                vapagarcon.IsVisible = true;
                Efectivo = true;
            }

        }
        private void PickerPickerAuto_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                var i = (AutosModel)PickerAuto.SelectedItem;
                var id = i.id_auto.ToString();
                id_auto.Text = id;



            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Intente en otro momento _ error: " + ex.ToString() + " _ ", "ok");
            }


        }


        private void PickerPaquetes_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                var i = (Paquete)PickerPaq.SelectedItem;
                var id = i.id_paquete.ToString();
                id_paquete.Text = id;
              



            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Intente en otro momento _ error: " + ex.ToString() + " _ ", "ok");
            }

        }


        private void PickerProductos_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
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

        private async Task getWashers()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var uri = "http://www.washdryapp.com/app/public/washer/lista";
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
//                                Console.WriteLine("----------------------------------------------_____:Here status 200");

                                try
                                {
                                    HttpContent content = response.Content;
                                    string xjson = await content.ReadAsStringAsync();


                                    var result = JsonConvert.DeserializeObject<List<Washer>>(xjson);

                                    if (result.Count() > 0)
                                    {
                                        WasherList.ItemsSource = result;
                                        WasherList.ItemTapped += PickerWasher_ItemTapped;
                                    }
                                    else
                                    {

                                        wlbl.Text = "Lo sentimos no hay washers cerca de momento, intenta mas tarde";

                                        WasherList.IsVisible = false;
                                    }

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
            catch (Exception exc)
            {

                await DisplayAlert("Error",""+exc.ToString(), "ok");
            }
           
        }

        private void PickerWasher_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content_X = e.Item as Washer;
            wlbl.Text = "atendido por : " + content_X.nombre;
            var idw = content_X.id_washer;
            id_washer.Text = idw.ToString();

        }

        private async Task getPaquetes()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var uri = "http://www.washdryapp.com/app/public/paquete/listado";
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

                                PickerPaq.TextColor = Color.FromHex("#4E8F75");
                                PickerPaq.TitleColor = Color.FromHex("#4E8F75");
                                pickertamanio.TitleColor = Color.FromHex("#4E8F75");

                                var result = JsonConvert.DeserializeObject<List<Paquete>>(xjson);

                                if (result.Count() > 0)
                                {
                                    PickerPaq.ItemsSource = result;
                                    PickerPaq.SelectedIndexChanged += PickerPaquetes_SelectedIndexChanged;
                                }
                                else
                                {


                                    PickerPaq.IsVisible = false;
                                }

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
        private async Task getdireccion()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var uri = "http://washdryapp.com/app/public/direccion/listado/" + idx;
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

                                if (result.Count() > 0)
                                {
                                    PickerDirecc.ItemsSource = result;
                                    PickerDirecc.SelectedIndexChanged += PickerProductos_SelectedIndexChanged;
                                }
                                else
                                {

                                    lbldirecc_error.IsVisible = true;
                                    PickerDirecc.IsVisible = false;
                                }

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
                var uri = "http://www.washdryapp.com/app/public/auto/listadoAutoUser/" + idx;
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

                                var result = JsonConvert.DeserializeObject<List<AutosModel>>(xjson);

                                if (result.Count() > 0)
                                {
                                    PickerAuto.ItemsSource = result;
                                    PickerAuto.SelectedIndexChanged += PickerPickerAuto_SelectedIndexChanged;
                                }
                                else
                                {

                                    await DisplayAlert("No has registrado autos", "Lo sentimos pero para poder solicitar un servicio primero necesitas registrar un auto", "ok");
                                    //await Navigation.PopToRootAsync();

                                }








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

            var x = 0000000;
            id_loc.Text = x.ToString();
            longitude.Text = pos.Latitude.ToString();
            desc.Text = "ubicacion actual sin id";
            latitud.Text = pos.Longitude.ToString();
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

        

        private void Cancelbtn_Clicked(object sender, EventArgs e)
        {
            frame1.IsVisible = true;
            stepBar.IsVisible = true;
            LoadinImg.IsVisible = false;
            Cator.IsVisible = false;
            Cancelbtn.IsVisible = false;
            loadinglbl.IsVisible = false;
            /*    */
        }

       

        private void ConfirmarServicio()
        {
            stepBar.IsVisible = false;
            frame1.IsVisible = false;
            frame2.IsVisible = false;
            frame3.IsVisible = false;
            frame4.IsVisible = false;
            frame5.IsVisible = true;

            stepBar.IsVisible = false;
            LoadinImg.IsVisible = false;
            Cator.IsVisible = false;
            Cancelbtn.IsVisible = false;
            loadinglbl.IsVisible = false;

        }

        private void btnSolicitar_Clicked(object sender, EventArgs e)
        {
            _ = btnSolicitar_Clickedx();
        }

        private async Task btnSolicitar_Clickedx()
        {
            frame1.IsVisible = false;
            frame2.IsVisible = false;
            frame3.IsVisible = false;
            frame4.IsVisible = false;
            frame5.IsVisible = false;

            LoadinImg.IsVisible = true;
            Cator.IsVisible = true;
            Cancelbtn.IsVisible = true;
            loadinglbl.IsVisible = true;

            await Task.Delay(2800);
            Cator.IsRunning = false;
            await LoadinImg.ScaleTo(0.6, 1500, Easing.BounceOut);
            await LoadinImg.FadeTo(0, 270, null);

            //LoadinImg


            ConfirmarServicio();
        }

        private async void btnpedirservicio_Clicked(object sender, EventArgs e)
        {
                    
            var idauto = id_auto.Text;
            var vaapgar = "";
            var forma = "";
            var idwasher = id_washer.Text;
            var idloc = id_loc.Text;
            var lon = longitude.Text;
            var lat = latitud.Text;
            var idpaq = id_paquete.Text;

            if (Tarjeta)
            {
                  vaapgar = vapagarcon.Text;
                forma = "Tarjeta";
            }
            else if (Efectivo)
            {
                forma = "Efectivo";
                vaapgar = vapagarcon.Text;
            }
            else {

                errorlblconfir.IsVisible = true;
                errorlblconfir.Text = "Selecione un metodo de pago";
            }


            if ( lon.Length < 5 || lat.Length < 5)
            {
                errorlblconfir.IsVisible = true;
                errorlblconfir.Text = "Error con la ubicacion, intente nuevamente";
            }

            if (idauto.Length < 1)
            {
                errorlblconfir.IsVisible = true;
                errorlblconfir.Text = "Error en los datos de su Auto";
            }
            if (idwasher.Length < 1)
            {
                errorlblconfir.Text = "Error en los datos de su Washer";
            }


            HttpClient client = new HttpClient();

            // pos.Latitude 

            var value_check = new Dictionary<string, string>
                         {
                            
                            {"id_washer" ,  idwasher},
                            {"id_usuario" ,  idx.ToString()},
                            {"id_paquete" ,  idpaq},
                            {"id_auto" ,  idauto},
                            {"latitud" ,  lat},
                            {"longitud" ,  lon},
                            {"foto","null" },
                            {"fecha",_datePicker.Date.ToString("ddd, d:e MMMM") },
                            {"calificacion","null" },
                            {"comentario","" },
                            {"cambio",vaapgar },
                            {"forma_pago",forma },



                };
            var content = new FormUrlEncodedContent(value_check); //solicitud/agrega
            var response = await client.PostAsync("http://www.washdryapp.com/app/public/solicitud/agrega", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Exito", "solcitud hecha", "ok");
            }
            else {
                await DisplayAlert("Error", "intente en otro momento", "ok");
            }

        }
    }
}