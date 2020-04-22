using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
using WashDry.SQLiteDb;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Servicio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupCancelServicio : PopupPage
    {
        public int ids;
        public UserDataBase userDataBase;
        public PopupCancelServicio(int sol)
        {
            InitializeComponent();
            ids = sol;
            btnCancel.IsEnabled = true;
        }
        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await CancelarServicio(ids);
        }
        private async Task CancelarServicio(int id) {
            btnCancel.IsEnabled = false;
            Cator.IsRunning = true;
            Cator.IsVisible = true;
            var msn = Message.Text;

            if (string.IsNullOrEmpty(msn))
            {
                Message.Focus();
            
            }
            else {
                HttpClient client = new HttpClient();
                var cancel_values = new Dictionary<string, string> {
                                { "id_solicitud",id.ToString() } ,
                                  { "comentario", msn.ToString()}
                            };
                var cancel_content = new FormUrlEncodedContent(cancel_values); //solicitud/agrega
        
                var cancelpost = await client.PostAsync("http://www.washdryapp.com/app/public/solicitud/cliente_cancelar", cancel_content);

                if (cancelpost.IsSuccessStatusCode)
                {
                    userDataBase = new UserDataBase();
                    userDataBase.DeleteSolicitud(id);
 

                    await DisplayAlert("Lavado cancelado", "Solicitud de servicio cancelada.", "ok");
                    await PopupNavigation.PopAsync();

                }
                else
                {
                    HttpContent contentx = cancelpost.Content;
                    var respx = await contentx.ReadAsStringAsync();

                    await DisplayAlert("Error", "Pudo haber un error. Intente en otro momento . "+ respx, "ok");
                    await PopupNavigation.PopAsync();

                }

                await Navigation.PopToRootAsync();
            }

            Cator.IsRunning = false;
            Cator.IsVisible = false;
        }

       
    }
}