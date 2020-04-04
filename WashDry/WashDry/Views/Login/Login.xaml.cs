using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.DbModels;
using WashDry.SQLiteDb;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public User user;
        private UserDataBase UserDb;
        public SQLiteConnection conn;

        public Login()
        {
            InitializeComponent();

 
        }

      

        private async void btnRegistDone_Clicked(object sender, EventArgs e)
        {


         
             var user = User_.Text;
            var pass = Pass_.Text;

            if (string.IsNullOrEmpty(User_.Text) || string.IsNullOrWhiteSpace(User_.Text)  )
            {
                User_.Focus();
               
            }
            else if(string.IsNullOrEmpty(Pass_.Text) || string.IsNullOrWhiteSpace(Pass_.Text))
            {


                Pass_.Focus();
            }
            
            else {

                HttpClient client = new HttpClient();

                
                var value_check = new Dictionary<string, string>
                         {
                            { "email", user},
                            { "pass", pass}
                         };

                 var contentx = new FormUrlEncodedContent(value_check);

                try
                {

                    var response = await client.PostAsync("http://www.washdryapp.com/app/public/solicitud/login_cliente",contentx);


                    HttpContent content = response.Content;

                    var json = await content.ReadAsStringAsync();
                    var json_ = JsonConvert.DeserializeObject<List<User>>(json);

                    var user_x = new User();
                    var userDataBase = new UserDataBase();


                    user_x.email = json_[0].email;
                    user_x.google_id = json_[0].google_id;
                    user_x.name = json_[0].name;
                    user_x.nombre = json_[0].nombre;
                    user_x.id = json_[0].id;
                    user_x.username = json_[0].username;
                    user_x.remember_token = json_[0].remember_token;//username id



                    userDataBase.AddMember(user_x);


                    Application.Current.MainPage = new MainPage();
                }
                catch (Exception ex)
                {

                    await DisplayAlert("", ""+ex.ToString(), "");
                }
 

            }
      
         


        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("ok", "ok", "ok");
        }
    }
}
 
 