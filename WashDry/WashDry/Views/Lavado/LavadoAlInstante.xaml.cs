using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class LavadoAlInstante : ContentPage
    {
        public LavadoAlInstante()
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
    }
}