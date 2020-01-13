using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstPage : ContentPage
    {
        public FirstPage()
        {
            InitializeComponent();
        }

        private async void btnRegist_Clicked(object sender, EventArgs e)
        {
           // Application.Current.MainPage = new Login();

            await Navigation.PushAsync(new SignUp());
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
           // Application.Current.MainPage = new SignUp();
            await Navigation.PushAsync(new Login());
        }
    }
}