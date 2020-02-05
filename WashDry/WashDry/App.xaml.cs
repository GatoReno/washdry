using System;
using WashDry.Splash;
using WashDry.Views.Lavado;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry
{
    public partial class App : Application
    {
        public static MasterDetailPage MasterD { get; set; }

        public App()
        {   
            InitializeComponent();
            MainPage = new NavigationPage(new SplashScreen());
           // MainPage = new Agendar();
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
