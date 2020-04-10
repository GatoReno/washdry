using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Servicio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstadoDeServicio : ContentPage
    {
        public EstadoDeServicio()
        {
            InitializeComponent();
        }

        private TokenService Tokenservice;
        private Token stripeToken;
        [Obsolete]
        private async void StripeTokenBtn_Clicked(object sender, EventArgs e)
        {
            Cator.IsRunning = true;
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
            }

            Cator.IsRunning = false;

        }


        private async Task btnpagartest_Clicked(object sender, EventArgs e)
        {
           
        }
    }
}