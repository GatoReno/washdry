using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WashDry.Views.Lavado;
using WashDry.Views.RegistCar;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class master : ContentPage
    {
        public master()
        {
            InitializeComponent();
        }

        private async   void btnautos_Clicked(object sender, EventArgs e)
        {

            //await Navigation.PushAsync(new NavigationPage(new ListCars()));
            await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ListCars());
            
            //await Navigation.PushModalAsync(new NavigationPage (new ListCars())); //esto fue muy util
        }

        private async void btnlavos_Clicked(object sender, EventArgs e)
        {
            await((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ListLavado());

        }
    }
}