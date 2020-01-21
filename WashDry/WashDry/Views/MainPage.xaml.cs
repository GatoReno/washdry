using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WashDry.Views.Lavado;
using WashDry.Views.RegistCar;
using Xamarin.Forms;

namespace WashDry
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
         }

        private async void Lavadobtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LavadoAlInstante());
        }

        private void Direccionbtn_Clicked(object sender, EventArgs e)
        {

        }

        private void AddCarbtn_Clicked(object sender, EventArgs e)
        {

        }

        private async void Registbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistCarPage());
        }

        private void Agendarbtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}
