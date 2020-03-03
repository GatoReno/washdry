using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using Spillman.Xamarin.Forms.ColorPicker;

namespace WashDry.Views.RegistCar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistCarPage : ContentPage
    {
        public RegistCarPage()
        {
            InitializeComponent();

        }

        private Color editedColor;

        public Color EditedColor
        {
            get => editedColor;
            set { editedColor = value; OnPropertyChanged(); }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, true);

            idx = "1";
        }

        private MediaFile _image;
        private string idx;
        private async void AgregarAutobtn_Clicked(object sender, EventArgs e)
        {

            try
            {
                var color = colorx.BackgroundColor;
                var color2 = colorx.ViewModel.Color;
                var colorhex = colorx.ViewModel.Hex;
                var color22 = colorx.ViewModel.HueColor;
                var modelox = Modelo.ToString();
                var marcax = Marca_.Text.ToString();
                var annx = Ann_.Date.ToString();
                var placasx = Placas.ToString();
                
                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    await DisplayAlert("Sin linea", "Active sus datos o su wifi para agregar un auto.", "OK");
                }

                StringContent hex = new StringContent(colorhex.ToString());
                StringContent id_usuario = new StringContent(idx);
                StringContent placas = new StringContent(placasx);
                StringContent modelo = new StringContent(modelox);
                StringContent ann = new StringContent(annx);
                StringContent marca = new StringContent(marcax);


                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(_image.GetStream()), "\"file\"", $"\"{_image.Path}\"");
                content.Add(id_usuario);
                content.Add(placas);
                content.Add(modelo);
                content.Add(ann);
                content.Add(marca);
                content.Add(hex);

                // puedo agregar a este content puro formato de texto ?
                // debo mandar un id y otros datos a mi servidor que espera el archivo
                // y los otros strings

                    // ... subir a internet
                var httpClient = new System.Net.Http.HttpClient();
                var url = "http://www.washdryapp.com/app/public/auto/guardar";
                var responseMsg = await httpClient.PostAsync(url, content);
                // ... subir a internet
                var remotePath = await responseMsg.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", "Error : "+ex.ToString(), "OK");
            }

        }

        private async void btnCamara_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera soportada.", "OK");
                return;
            }
            _image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "auto_" + idx,
                Name = "auto.jpg"
            });
            if (_image == null)
                return;
            // await DisplayAlert("File Location Error", "Error parece que hubo un problema con la camara, confirme espacio en memoria o notifique a sistemas", "OK");
            var xlocal = _image.Path;
            imgx.Source = ImageSource.FromStream(() => {

                return _image.GetStream();


            });





        }

        private async void btnGal_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Galeria", ":( No camera soportada.", "OK");
                return;
            }

            _image = await CrossMedia.Current.PickPhotoAsync();

            if (_image == null)
            {
                return;
            }
            imgx.Source = ImageSource.FromStream(() => {

                return _image.GetStream();


            });
        }
    }
}