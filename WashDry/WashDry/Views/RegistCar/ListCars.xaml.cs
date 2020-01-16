using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.RegistCar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCars : ContentPage
    {
        public ListCars()
        {
            InitializeComponent();
        }

        private async void ListVisitas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new CarInfo());
        }

    }
}