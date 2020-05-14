using LaavorRatingConception;
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

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoLavado : ContentPage
    {
        public int idx;
        public InfoLavado(int idsol)
        {
            InitializeComponent();
            idx = idsol;
            _ = GetSolicitudesWeb();
        }
        public async Task GetSolicitudesWeb()
        {


            try
            {

              
                HttpClient client = new HttpClient();
                var uri = "http://www.washdryapp.com/app/public/solicitud/lista_solicitud/" + idx;



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

                       
                        var json_d = JsonConvert.DeserializeObject<List<Solicitudes>>(xjsonD);

                        Mainlbl.Text = "Lavado del " + json_d[0].fecha;
                        precio.Text = "cargo : $" + json_d[0].precio;
                        Info.Text = "Forma de pago / " + json_d[0].forma_pago;
                        var califint = Int32.Parse(json_d[0].calificacion);
                        ratingStar.InitialValue = califint;

                        ImageCar.Source = json_d[0].foto;

                        //
                        var uri2 = "http://www.washdryapp.com/app/public/washer/perfil/" + json_d[0].id_washer;

                        var responseW = await client.GetAsync(uri2);
                        if (responseW.IsSuccessStatusCode)
                        {
                            HttpContent contentD2 = responseW.Content;
                            var xjsonD2 = await contentD2.ReadAsStringAsync();

                            if (xjsonD2 == "[]")
                            {
                                wname.Text = "Faltan datos de este washer, posible baja del elemento";
                            }
                            else
                            {
                                var json_d2 = JsonConvert.DeserializeObject<List<Washer>>(xjsonD2);

                                wname.Text = "atendido por : " + json_d2[0].nombre + " "
                                    + json_d2[0].app + " "
                                    + json_d2[0].apm + " ";
                            }
                           

                        }
                        else
                        {
                            wname.Text = "No hay informacion disponible del washer ";
                        }

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
        private void RatingConception_Voted(object sender, EventArgs e)
        {
            RatingConception rating = (RatingConception)sender;
            int index = rating.IndexVoted;
            int value = rating.Value;

       

            // rating.InitialValue = 2; sobres así se asigna por default e buen rating
        }
    }
}