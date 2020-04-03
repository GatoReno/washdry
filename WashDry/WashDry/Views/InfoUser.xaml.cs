using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WashDry.Views.UserInfo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
 using WashDry.Models.DbModels;
using WashDry.SQLiteDb;
using System.Net.Http;
using Newtonsoft.Json;

namespace WashDry.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoUser : ContentPage
    {
        public InfoUser()
        {
            InitializeComponent();
        }
        public async Task GetInfoUser()
        {


            try
            {

                userDataBase = new UserDataBase();
                var user_exist = userDataBase.GetMembers().ToList();
                HttpClient client = new HttpClient();
                var uri = "http://www.washdryapp.com/app/public/washer/loginChema"; //+ user_exist[0].id;



                var responseMsg = await client.GetAsync(uri);



                switch (responseMsg.StatusCode)
                {
                    case System.Net.HttpStatusCode.InternalServerError:
                        Console.WriteLine("----------------------------------------------_____:Here status 500");

                        //xlabel.Text = "error 500";
                        // Cator.IsVisible = false;
                        break;


                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine("----------------------------------------------_____:Here status 200");

                      

                   

                        HttpContent contentD = responseMsg.Content;
                        var xjsonD = await contentD.ReadAsStringAsync();


                      
                        var json_d = JsonConvert.DeserializeObject<List<User>>(xjsonD);


                        lbl3.Text = json_d[0].password;
                        


                        break;

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex.ToString(), "ok");
                //  Cator.IsVisible = false;

                //  CatorT.Text = "Ha habido un error";
                return;
            }


        }

        private async void Direccionbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Direcciones());
        }
        public User user;
        public UserDataBase userDataBase;
        protected override void OnAppearing() {

            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();
            if (user_exist.Count() > 0)
            {
                lbl1.Text = user_exist[0].nombre;
                lbl2.Text = user_exist[0].email;



                _ = GetInfoUser();




            }
            else
            {

                lbl1.Text = "Error. No hay info de usuario en SQLite";

            }

        }
    }
}