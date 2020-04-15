using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
        
        public User user;
        private UserDataBase userdb;
        private MediaFile _image;
        private string idx;
        public string imagen_name;

        public SignUp()
        {
            InitializeComponent();
            Title = "WASH DRY";
            //PropertyMaximumDate = DateTime.Now;
            userdb = new UserDataBase();
            var user_token = userdb.GetMembers().ToList();
           
            int RowCount = 0;
            int regcount = user_token.Count();
            RowCount = Convert.ToInt32(regcount);
            var token = user_token[0].token;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            idx = "13";
        }

        private async void Registrar_Clicked(object sender, EventArgs e)
        {

            Cator.IsVisible = true;
            btnRegistrar.IsVisible = false;
                userdb = new UserDataBase();
                var reg_existL = userdb.GetMembers().ToList();
               // var idRegL = reg_existL[0].id;
               var nombreL = Nombre.Text;
                var appL = Appaterno.Text;
                var apmL = Apmaterno.Text;
                var fca_nacL = Fca_nac.Date.ToString("yyyy-MM-dd");//Fca_nac.Date.ToString();
                var telefonoL = Telefono.Text;
                var correoL = Correo.Text;
                var passwordL = Password.Text;
               // var foto = imgx.Source;
                //userdb.UpdateAll(idRegL, nombreL, appL, apmL, fca_nacL.ToString(), telefonoL, correoL, passwordL, 1);
                try
                {
                    var reg_exist = userdb.GetMembers().ToList();
                var nombre = nombreL;// reg_exist[0].nombre;
                    var app = appL;
                    var apm = apmL;
                    var fca_nac = fca_nacL;
                    var telefono = telefonoL;
                    var correo = correoL;
                    var password = passwordL;
                    var confPass = passwordL;
                    var tokens = reg_exist[0].token;
                    if (tokens == null || tokens == "")
                    {
                        userdb = new UserDataBase();
                        var user_token = userdb.GetMembers().ToList();
                        var tokenUsuario = user_token[0].token;
                        tokens = tokenUsuario;
                    }
                    if (password == "" || password == null)
                    {
                        password = Password.Text;
                        confPass = ConfirmaPass.Text;
                    }
                    if (password == confPass)
                    {
                        //GUARDAR IMAGEN
                        var content1 = new MultipartFormDataContent();
                        content1.Add(new StreamContent(_image.GetStream()), "\"file\"", $"\"{_image.Path}\"");

                        var httpClient1 = new System.Net.Http.HttpClient();
                        httpClient1.BaseAddress = new Uri("http://www.washdryapp.com");
                        var url1 = "http://www.washdryapp.com/oficial/ImagenesPerfil.php";
                        var responseMsg1 = await httpClient1.PostAsync(url1, content1);
                        var remotePath = await responseMsg1.Content.ReadAsStringAsync();
                        imagen_name = remotePath;
                        //*************
                        var httpClient = new HttpClient();
                        //var url = /washer/guardar;
                        var url = "http://www.washdryapp.com/app/public/cliente/registra";

                        var value_check = new Dictionary<string, string>
                {
                    {"nombre", nombre },
                    {"app", app},
                    {"apm", apm},
                   
                    {"fecha_nac", fca_nac },
                    {"telefono", telefono },
                    {"email", correo },
                    {"password", password },
                    {"token", tokens },
                    {"foto",  imagen_name}
                };
                        var content = new FormUrlEncodedContent(value_check);
                        var responseMsg = await httpClient.PostAsync(url, content);

                        switch (responseMsg.StatusCode)
                        {
                            case System.Net.HttpStatusCode.InternalServerError:
                                await DisplayAlert("error", "error status 500 InternalServerError", "ok");
                                break;
                            case System.Net.HttpStatusCode.BadRequest:
                                await DisplayAlert("error", "error status 400 Unauthorized", "ok");
                                break;
                            case System.Net.HttpStatusCode.Forbidden:
                                await DisplayAlert("error", "error status 403  ", "ok");
                                break;
                            case System.Net.HttpStatusCode.NotFound:
                                await DisplayAlert("error", "error status 404  ", "ok");
                                break;
                            case System.Net.HttpStatusCode.OK:
                                string xjson = await responseMsg.Content.ReadAsStringAsync();
                            //var json_ = JsonConvert.DeserializeObject<List<User>>(xjson);

                            //userdb.UpdateMember();

                            await Navigation.PopAsync();
                                await DisplayAlert("Success", "Se agrego Correctamente ", "ok");
                                break;
                            case System.Net.HttpStatusCode.Unauthorized:
                                await DisplayAlert("error", "yeah status 401 Unauthorized", "ok");
                                break;
                        }
                    }
                    else
                    {
                        await DisplayAlert("error", "Contraseña no coinciden", "ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Error : " + ex.ToString(), "OK");
                }


            Cator.IsVisible = false;
            btnRegistrar.IsVisible = true;

            //Application.Current.MainPage = new NavigationPage(new Login());
        }

     

        private async void BtnFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera soportada.", "OK");
                return;
            }
            _image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "washers",
                Name = "washer.jpg"
            });
            if (_image == null)
                return;
            // await DisplayAlert("File Location Error", "Error parece que hubo un problema con la camara, confirme espacio en memoria o notifique a sistemas", "OK");
            var xlocal = _image.Path;
            imgx.Source = ImageSource.FromStream(() => {

                return _image.GetStream();


            });
        }

    
        }
    }