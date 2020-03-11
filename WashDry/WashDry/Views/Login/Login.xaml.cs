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

            if (user.Length < 0 || pass.Length < 0)
            {
                User_.Focus();
                Pass_.Focus();
            }else {

                HttpClient client = new HttpClient();


                var value_check = new Dictionary<string, string>
                         {
                            { "usuario", user},
                            { "pass", pass}
                         };


                var content = new FormUrlEncodedContent(value_check);
                var response = await client.PostAsync("http://washdryapp.com/app/public/login/app", content);

            }


         


            ///  Application.Current.MainPage =  new MainPage();
        }
    }
}