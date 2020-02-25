using Plugin.Connectivity;
using Plugin.Geolocator;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WashDry.Views.Lavado;
using WashDry.Views.RegistCar;
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

            /*
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
            Mapx.Pins.Add(pin);*/

        }
        private async void Lavadobtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LavadoAlInstante());
            //   await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new LavadoAlInstante());
        }

        private void Direccionbtn_Clicked(object sender, EventArgs e)
        {

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
            await Navigation.PushAsync(new Agendar());
        }

        private async void CarListbtrn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListCars());
        }
        private TokenService Tokenservice;
        private Token stripeToken;
        [Obsolete]
        private void StripeTokenBtn_Clicked(object sender, EventArgs e)
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
                StripeLbl.Text = stripeToken.Id;
            }
            catch (Exception ex)
            {
                StripeLbl.Text = ex.ToString() ;

            }
            
        }
    }
}