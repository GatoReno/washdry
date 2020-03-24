using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.RegistCar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCars : ContentPage
    {
        public ListCars()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {

            _ = GetVisitas();
        }


        public async Task GetVisitas()
        {

            HttpClient client = new HttpClient();
            var uri = "http://washdryapp.com/app/public/auto/listado";

            try
            {

                var response = await client.GetAsync(uri);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.InternalServerError:
                        Console.WriteLine("----------------------------------------------_____:Here status 500");

                        //xlabel.Text = "error 500";
                        // Cator.IsVisible = false;
                        break;


                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine("----------------------------------------------_____:Here status 200");

                        // ylabel.Text = "Ultimas noticas de proyectos";
                        HttpContent content = response.Content;
                        var xjson = await content.ReadAsStringAsync();

                        var json_ = JsonConvert.DeserializeObject<List<AutosModel>>(xjson);
                        ListAutos.ItemsSource = json_;
                        Cator.IsVisible = false;



                        break;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex.ToString(), "ok");
                Cator.IsVisible = false;

                CatorT.Text = "Ha habido un error";
                return;
            }


        }
        private async void ListVisitas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as AutosModel;
             await Navigation.PushAsync(new CarInfo(Int32.Parse(content.id_auto)));
        }

    }
}