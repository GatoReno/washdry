using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.ApiModels;
using Xamarin.Forms;
using WashDry.Models.ApiModels;
using WashDry.Models.DbModels;
using Xamarin.Forms.Xaml;
using WashDry.SQLiteDb;

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
        public User user;
        public UserDataBase userDataBase;

        public async Task GetVisitas()
        {


            userDataBase = new UserDataBase();
            var user_exist = userDataBase.GetMembers().ToList();
            HttpClient client = new HttpClient();
            var uri = "http://www.washdryapp.com/app/public/auto/listadoAutoUser/" + user_exist[0].id;
     

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
                        //Console.WriteLine("----------------------------------------------_____:Here status 200");

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

     

        private void btnaddcar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistCarPage());
        }
    }
}