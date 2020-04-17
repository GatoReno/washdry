using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.DbModels;
using WashDry.SQLiteDb;
using WashDry.Views;
using WashDry.Views.Lavado;
using WashDry.Views.Login;
using WashDry.Views.RegistCar;
using WashDry.Views.Servicio;
using WashDry.Views.UserInfo;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace WashDry.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class master : ContentPage
    {

        public User user;
        public UserDataBase userDataBase;
       
        public  master()
        {
            InitializeComponent();
            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();

            namelbl.Text = user_exist[0].name;//+ user_exist[0].token;
            InfoUserweb(user_exist[0].id);



        }

        private async void InfoUserweb(int idx) {

            HttpClient client = new HttpClient();
            var uri = "http://washdryapp.com/app/public/cliente/getPerfil/" + idx; //+ user_exist[0].id;



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


                    if (json_d[0].foto.Length > 1)
                    {
                        logo.Source = json_d[0].foto;
                    }
                    else { logo.Source = "iko.png"; }





                    break;

            }
        }

        private async   void btnautos_Clicked(object sender, EventArgs e)
        {
            App.MasterD.IsPresented = false;

            //await Navigation.PushAsync(new NavigationPage(new ListCars()));
            await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ListCars());
            
            //await Navigation.PushModalAsync(new NavigationPage (new ListCars())); //esto fue muy util
        }
        private async void Direccionbtn_Clicked(object sender, EventArgs e)
        {
            App.MasterD.IsPresented = false;
            await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new Direcciones());
        }
        private async void  btnlavos_Clicked(object sender, EventArgs e)
        {
            App.MasterD.IsPresented = false;

            await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ListLavado());

        }

        private async void btnconfiguracion_Clicked(object sender, EventArgs e)
        {

            App.MasterD.IsPresented = false;

            await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new InfoUser());
        }

        private async void btnedoserv_Clicked(object sender, EventArgs e)
        {
            
                 App.MasterD.IsPresented = false;

            await((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new EstadoDeServicio());
        }

        private async void btnsolicitardserv_Clicked(object sender, EventArgs e)
        {
            App.MasterD.IsPresented = false;

            await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new Solicitar());

        }

        private async void contratarbtn_Clicked(object sender, EventArgs e)
        {
            App.MasterD.IsPresented = false;

            await((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new Contratar());

        }

        private void btnCerrarSession_Clicked(object sender, EventArgs e)
        {
            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();
            var idx = user_exist[0].id;
            var tok = user_exist[0].token;

            var userW = new User();
            
            userW.id_cliente = 0;

            userW.name = "";
            userW.nombre = "";
            userW.password = "";
            userW.username = "";
            userW.remember_token = "";
            userW.google_id = "";
            userW.email = "";
            userW.token = tok;
            userW.status = 0;
            userDataBase.DeleteMember(idx);
            userDataBase.AddMember(userW);

           

           
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}