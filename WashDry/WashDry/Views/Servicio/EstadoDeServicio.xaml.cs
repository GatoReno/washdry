using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Stripe;
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

namespace WashDry.Views.Servicio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstadoDeServicio : ContentPage
    {
        public int idsol ;
        public int id_lite;

        public EstadoDeServicio(int id_sol,int idlite)
        {
            InitializeComponent();
            idsol = id_sol;
            id_lite = idlite;
            _ = GetSolicitud(idsol);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
           await GetSolicitud(idsol);
        }
        private async Task GetSolicitud(int id)
        {

            CatorMain.IsVisible = true;
            CatorMain.IsRunning = true;

            HttpClient client = new HttpClient();
            var info = await client.GetAsync("http://washdryapp.com/app/public/solicitud/lista_solicitud/" + id);
            if (info.IsSuccessStatusCode) {
                var content = info.Content;
                var json = await content.ReadAsStringAsync();
                var xjson = JsonConvert.DeserializeObject<List<Solicitudes>>(json);
                UserDataBase udb = new UserDataBase();

                if (xjson.Count > 0)
                {
                    lbltitle.Text = xjson[0].paquete;
                    lbldate.Text = xjson[0].fecha;
                    lblprecio.Text = "$ "+xjson[0].precio+ " Mxn ";
                    lbldestino.Text =  xjson[0].modelo + " " + xjson[0].placas;
                    var status = xjson[0].status;
                    switch (status)
                    {
                        case "1":
                            status = "En proceso";
                            break;
                        case "2":
                            status = "Aceptado";
                            break;
                        case "3":
                            status = "Lavado en proceso";
                            break;
                        case "4":
                            imgxwasher.Source = xjson[0].foto_washer;
                            btnpagartest.IsEnabled = true;
                            btnpagartest.IsVisible = true;
                            imgxwasher.IsVisible = true;
                            status = "En espera de pago";
                            break;
                        case "5":
                            btnCancel.IsVisible = false;
                            
                            status = "Lavado pagado";
                            break;
                        case "6":
                            udb.DeleteSolicitud(id_lite);
                            status = "Cancelado";
                            break;
                        case "7":
                            udb.DeleteSolicitud(id_lite);
                            btnCancel.IsVisible = false;
                            status = "Terminado";
                            break;
                    }
                    lblstatus.Text = status;

                    if (string.IsNullOrEmpty(xjson[0].foto))
                    {
                        lblimg.IsVisible = true;
                        lblimg.Text = "Iamgen no disponible";
                    }
                    else
                    {
                        imgxwasher.IsVisible = true;
                        imgxwasher.Source = xjson[0].foto;

                    }

                }
                else {
                    btnCancel.IsVisible = true;

                    await DisplayAlert("Pudo haber un error","Intenta otra ves mas tarde, recuerda que siempre puedes cancelar tus servicios.","ok");
                }

            }
            else {

                await Navigation.PopModalAsync();
            }

            CatorMain.IsVisible = false;
            CatorMain.IsRunning = false;
        }
     
        [Obsolete]
        private async void StripeTokenBtn_Clicked(object sender, EventArgs e)
        {
            btnpagartest.IsEnabled = false;
            Cator.IsRunning = true;

            await PopupNavigation.PushAsync(new PopupPagar(idsol));

            /*
            try
            {
                StripeConfiguration.SetApiKey("pk_test_HQOqIXmo6C3MyZ2h9bBAcWKs00ngt4dRKC");
                var service = new ChargeService();
                var Tokenoptions = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {
                        Number = "4242424242424242",
                        ExpYear = 22,
                        ExpMonth = 11,
                        Cvc = "111",
                        Name = "Sonu Sharma",
                        AddressLine1 = "18",
                        AddressLine2 = "SpringBoard",
                        AddressCity = "Gurgoan",
                        AddressZip = "284005",
                        AddressState = "Haryana",
                        AddressCountry = "India",
                        Currency = "inr",
                    }
                };

                Tokenservice = new TokenService();
                stripeToken = Tokenservice.Create(Tokenoptions);
                // StripeLbl.Text = stripeToken.Id;


                HttpClient client = new HttpClient();
                var value_check = new Dictionary<string, string>{ 
                            { "stripeToken", stripeToken.Id},
                            { "email"  , "pushpoped@gmail.com"},
                            { "id_usuario"  , "2"},
                            { "id_washer"  , "11"},
                            { "monto"  , "300"},
                            { "cambio"  , "0"},
                            { "tipo_pago"  , "1"},
                            { "id_solicitud"  , "10101"}
                         };
                var content = new FormUrlEncodedContent(value_check);
                var response = await client.PostAsync("http://www.washdryapp.com/app/public/pagos/generar", content);
                

                switch (response.StatusCode)
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
                        await DisplayAlert("Pago exitoso", "yeah status 200", "ok");
                        string xjson = await response.Content.ReadAsStringAsync();
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
                var x = ex.ToString();
                //  StripeLbl.Text = ex.ToString() ;
                Cator.IsRunning = false;
            }  */
            CatorMain.IsRunning = false;
            btnpagartest.IsEnabled = true;


        }


        private async Task btnpagartest_Clicked(object sender, EventArgs e)
        {
           
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            CatorCancel.IsRunning = true;
            btnCancel.IsEnabled = false;

            await PopupNavigation.PushAsync(new PopupCancelServicio(idsol));
            // lblestados

            CatorCancel.IsRunning = false;
            btnCancel.IsEnabled = true;
        }
    }
}