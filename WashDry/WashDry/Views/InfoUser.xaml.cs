using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WashDry.Views.UserInfo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoUser : ContentPage
    {
        public InfoUser()
        {
            InitializeComponent();
        }

        private async void Direccionbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Direcciones());
        }
    }
}