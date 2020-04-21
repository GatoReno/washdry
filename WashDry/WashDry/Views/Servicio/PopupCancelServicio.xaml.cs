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
           
        }
        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await CancelarServicio(ids);
        }
        private async Task CancelarServicio(int id) {

           


            if (string.IsNullOrEmpty(Message.Text))
            {
                Message.Focus();
            
            }
            else {

                var cancel_values = new Dictionary<string, string> {
                                { "id_solicitud",id.ToString() },
                                { "comentario", Message.Text}
                            };
                var cancel_content = new FormUrlEncodedContent(cancel_values); //solicitud/agrega
                HttpClient client = new HttpClient();
                var cancelpost = await client.GetAsync("http://www.washdryapp.com/app/public/solicitud/cliente_cancelar/"+id.ToString());

                if (cancelpost.IsSuccessStatusCode)
                {                    
                    userDataBase.DeleteSolicitud(id);

                    await DisplayAlert("Error", "Pudo haber un error. Intente en otro momento.", "ok");
                    await PopupNavigation.PopAsync();

                }
                else
                {
                    await DisplayAlert("Error", "Pudo haber un error. Intente en otro momento.", "ok");
                    await PopupNavigation.PopAsync();

                }

                await Navigation.PopToRootAsync();
            }
        

        }

       
    }
}