using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private async void btnautos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistCarPage());

        }
    }
}