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
    public partial class CarInfo : ContentPage
    {
        public int idx;
        public CarInfo(int id)
        {
            InitializeComponent();
            idx = id;
        }
       
        protected override void OnAppearing()
        {

            _ = GetinfoCarWeb();
        }

        public async Task GetinfoCarWeb()
        {


            try
            {

            
                HttpClient client = new HttpClient();
                var uri = "http://www.washdryapp.com/app/public/auto/datos/" + idx;



                var responseMsg = await client.GetAsync(uri);



                switch (responseMsg.StatusCode)
                {
                    case System.Net.HttpStatusCode.InternalServerError:
                        Console.WriteLine("----------------------------------------------_____:Here status 500");

                        //xlabel.Text = "error 500";
                        // Cator.IsVisible = false;
                        break;


                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine("----------------------------------------------_____:Here status 200");

                        // ylabel.Text = "Ultimas noticas de proyectos";


                        // var json_ = JsonConvert.DeserializeObject<List<VisitasMod>>(xjson);

                        string xjson = await responseMsg.Content.ReadAsStringAsync();
                        //DireccApiCall

                        HttpContent contentD = responseMsg.Content;
                        var xjsonD = await contentD.ReadAsStringAsync();


                       
                            var json_d = JsonConvert.DeserializeObject<List<AutosModel>>(xjsonD);

                        Placas.Text = json_d[0].placas;
                        Logoimg.Source = json_d[0].imagen;
                        Modelo.Text = json_d[0].modelo;
                        Anio.Text = json_d[0].ann;
                        Marca.Text = json_d[0].marca;

                        break;

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex.ToString(), "ok");
                //  Cator.IsVisible = false;

                //  CatorT.Text = "Ha habido un error";
                return;
            }


        }
    }
}