using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnRegistDone_Clicked(object sender, EventArgs e)
        {


            var user = User_.Text;
            var pass = Pass_.Text;

            HttpClient client = new HttpClient();


            var value_check = new Dictionary<string, string>
                         {
                            { "token", ""}
                         };


            var content = new FormUrlEncodedContent(value_check);
            var response = await client.PostAsync("https://trustfundapp.herokuapp.com/m/ensureToken",content);


            ///  Application.Current.MainPage =  new MainPage();
        }
    }
}