using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.DbModels;
using WashDry.SQLiteDb;
using WashDry.Views;
using WashDry.Views.Lavado;
using WashDry.Views.RegistCar;
using WashDry.Views.Servicio;
using WashDry.Views.UserInfo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class master : ContentPage
    {

        public User user;
        public UserDataBase userDataBase;
       
        public master()
        {
            InitializeComponent();
            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();

            namelbl.Text = user_exist[0].name;
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
             await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new Direcciones());
        }
        private async void btnlavos_Clicked(object sender, EventArgs e)
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
    }
}