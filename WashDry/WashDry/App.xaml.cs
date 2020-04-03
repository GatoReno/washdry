using Plugin.LocalNotifications;
using System;
using System.Threading.Tasks;
using WashDry.Splash;
using WashDry.Views.Lavado;
using WashDry.Views.Login;
using WashDry.Views.Servicio;
using WashDry.Views.UserInfo;
using Xamarin.Forms;
 
namespace WashDry
{
    public partial class App : Application
    {
        public static MasterDetailPage MasterD { get; set; }
        private bool sleepin ;
        public App()
        {   
            InitializeComponent();



            /// MainPage = new Solicitar(); SplashScreen
            MainPage = new NavigationPage(new Direcciones());
            //  este la vista de test de sqlite  // MainPage = new Test();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
           
      
        }

        protected override void   OnSleep()
        {

            // Handle when your app sleeps

            Device.StartTimer(TimeSpan.FromMinutes(1), () => //Will start after 1 min
            {
                Task.Run(() =>
                {
                    CrossLocalNotifications.Current.Show("Washdry", "Estas contectado a washdry  🚗");


                    // do something with time...
                });

                return false; // To repeat timer,always return true.If you want to stop the timer,return false
            });

            //if (!sleepin)
            //{
            //    Device.StartTimer(TimeSpan.FromMinutes(1), () => //Will start after 1 min
            //    {
            //        Task.Run(() =>
            //        {
            //            CrossLocalNotifications.Current.Show("Washdry", "Estas contectado a washdry");


            //            // do something with time...
            //        });

            //        return true; // To repeat timer,always return true.If you want to stop the timer,return false
            //    });

            //    sleepin = true;
            //}
            //else
            //{
            //    Device.StartTimer(TimeSpan.FromMinutes(1), () => //Will start after 1 min
            //    {
            //        Task.Run(() =>
            //        {
            //            CrossLocalNotifications.Current.Show("Washdry", "Estas contectado a washdry");


            //            // do something with time...
            //        });

            //        return true; // To repeat timer,always return true.If you want to stop the timer,return false
            //    });

            //}

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
       
                    CrossLocalNotifications.Current.Show("Washdry", "Pide tu proximo lavado ya 🚗");
            

        }
    }
}
