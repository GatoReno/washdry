using Newtonsoft.Json;
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
    public partial class SignUp : ContentPage
    {
        public UserDataBase userDataBase;

        public SignUp()
        {
            InitializeComponent();
        }

        private async void btnRegistDone_Clicked(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new MainPage();
            //

            if (String.IsNullOrEmpty(entnombre.Text))
            {
                entnombre.Focus();
            }
            else if (String.IsNullOrEmpty(entusername.Text))
            {
                entusername.Focus();
            }
            else if (String.IsNullOrEmpty(enttelefono.Text))
            {
                enttelefono.Focus();
            }
            else if (String.IsNullOrEmpty(entemail.Text))
            {
                entemail.Focus();
            }
            else if (Pass.Text != Pass2.Text)
            {
                Pass.Focus();
                Pass2.Focus();
                errorpasslbl.IsVisible = true;
                errorpasslbl.Text = "Contraseñas no coinciden";
            }
            else
            {
                try
                {

                    var httpClient = new System.Net.Http.HttpClient();
                    StringContent nombre = new StringContent(entnombre.Text);
                    StringContent username = new StringContent(entusername.Text);
                    StringContent password = new StringContent(Pass.Text);
                    StringContent email = new StringContent(entemail.Text);

                    var content = new MultipartFormDataContent();
                    content.Add(nombre, "nombre");
                    content.Add(username, "username");
                    content.Add(email, "email");
                    content.Add(password, "password");
                    HttpClient client = new HttpClient();

                    var url = "http://www.washdryapp.com/app/public/solicitud/agrega_cliente";
                    //  IsSuccessStatusCode = false


                    var responseMsg = await httpClient.PostAsync(url, content);
                    // ... subir a internet


                    if (responseMsg.IsSuccessStatusCode == false)
                    {
                        await DisplayAlert("error", "error status 419 Probelmas con respuesta del server, intente mas tarde o reinicie la aplicacion", "ok");
                    }
                    else
                    {
                        switch (responseMsg.StatusCode)
                        {

                            case System.Net.HttpStatusCode.BadRequest:
                                await DisplayAlert("error", "error status 400 Unauthorized", "ok");
                                break;



                            case System.Net.HttpStatusCode.Forbidden:
                                await DisplayAlert("error", "error status 403  ", "ok");
                                break;

                            case System.Net.HttpStatusCode.NotFound:
                                await DisplayAlert("200", "error status 404  ", "ok");
                                break;

                            case System.Net.HttpStatusCode.OK:


                                string xjson = await responseMsg.Content.ReadAsStringAsync();
                                var json_d = JsonConvert.DeserializeObject<User>(xjson);

                                //await DisplayAlert("error", "yeah status 200 : " + xjson, "ok");
                                userDataBase = new UserDataBase();

                                User user = new User();
                                user.name = entnombre.Text;
                                user.email = entemail.Text;
                                user.username = entusername.Text;
                                user.id = json_d.id;

                                userDataBase.AddMember(user);
                                Application.Current.MainPage = new MainPage();


                                break;

                            case System.Net.HttpStatusCode.RequestEntityTooLarge:
                                await DisplayAlert("error", "error status 413  ", "ok");
                                break;
                            case System.Net.HttpStatusCode.RequestTimeout:
                                await DisplayAlert("error", "error status 408  ", "ok");
                                break;

                            case System.Net.HttpStatusCode.Unauthorized:
                                await DisplayAlert("error", "yeah status 401 Unauthorized", "ok");
                                break;

                        }
                    }

                }
                catch (Exception ex)
                {

                    await DisplayAlert("error", "Error : " + ex.ToString(), "ok");
                }
            }

           




        }

        private void btngoogle_Clicked(object sender, EventArgs e)
        {

        }
    }
}