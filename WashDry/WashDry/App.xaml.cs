using System;
using WashDry.Splash;
using WashDry.Views.Lavado;
 
using Xamarin.Forms;
 
namespace WashDry
{
    public partial class App : Application
    {
        public static MasterDetailPage MasterD { get; set; }

        public App()
        {   
            InitializeComponent();


            //MainPage = new LavadorInfo(); 
            MainPage = new NavigationPage(new SplashScreen());
            //  este la vista de test de sqlite  // MainPage = new Test();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
