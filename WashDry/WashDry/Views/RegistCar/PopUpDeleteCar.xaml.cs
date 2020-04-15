using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.RegistCar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpDeleteCar : PopupPage
    {
        public string urlx;
        public PopUpDeleteCar(string url)
        {
            InitializeComponent();
            urlx = url;
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {

            var httpClient1 = new System.Net.Http.HttpClient();
            var responseMsg1 = await httpClient1.GetAsync(urlx);

            if (responseMsg1.IsSuccessStatusCode)
            {
                await PopupNavigation.PopAsync();
            }
            else {

                await DisplayAlert("Error!","Pudo haber un error intente mas tarde","ok");
            }


           
        }
    }
}