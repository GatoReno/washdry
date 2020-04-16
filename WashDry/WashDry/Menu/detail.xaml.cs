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
using WashDry.Views.Lavado;
using WashDry.Views.RegistCar;
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
        public detail()
        {
            InitializeComponent();
            _ = CurrentLocation();


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
        private TokenService Tokenservice;
        private Token stripeToken;
        [Obsolete]
        private async void StripeTokenBtn_Clicked(object sender, EventArgs e)
        {

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
                var value_check = new Dictionary<string, string>
                         {
                            { "stripeToken", stripeToken.Id},
                            { "email"  , "pushpoped@gmail.com"}
                         };


                var content = new FormUrlEncodedContent(value_check);
                var response = await client.PostAsync("http://www.washdryapp.com/app/public/make-prueba", content);

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
                        await DisplayAlert("error", "yeah status 200", "ok");
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

            }
            
        }

        private async void btnAgendar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Agendar());
        }
    }
}