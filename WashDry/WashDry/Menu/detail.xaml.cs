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
    public partial class detail : ContentPage
    {
        public detail()
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

        private async void Agendarbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Agendar());
        }
    }
}