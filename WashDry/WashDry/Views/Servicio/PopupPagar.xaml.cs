using Rg.Plugins.Popup.Pages;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.SQLiteDb;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Servicio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPagar : PopupPage
    {
        public int sid;
        public PopupPagar(int sol)
        {
            InitializeComponent();
            sid = sol;
        }
        public UserDataBase userDataBase;
        private TokenService Tokenservice;
        private Token stripeToken;
        [Obsolete]

        private async void StripeTokenBtn_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Card_.Text))
            {
                Card_.Focus();
            }

            else if (string.IsNullOrEmpty(Month.Text))
            {
                Month.Focus();
            }
            else if (string.IsNullOrEmpty(Year.Text))
            {
                Year.Focus();
            }

            else if (string.IsNullOrEmpty(Secret.Text))
            {
                Secret.Focus();
            }

            

            try
            {

                userDataBase = new UserDataBase();
                var user = userDataBase.GetMembers().ToList();
                var solx = userDataBase.GetSolicitud(sid).ToList();

                StripeConfiguration.SetApiKey("pk_test_HQOqIXmo6C3MyZ2h9bBAcWKs00ngt4dRKC");
                var service = new ChargeService();
                var Tokenoptions = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {
                        Number = "4242424242424242",
                        ExpYear = Int32.Parse(Year.Text),
                        ExpMonth = Int32.Parse(Month.Text),
                        Cvc = Secret.Text,
                        Name = user[0].nombre,
                      /*  AddressLine1 = "18",
                        AddressLine2 = "SpringBoard",
                        AddressCity = "Gurgoan",
                        AddressZip = "284005",
                        AddressState = "Haryana",*/
                        AddressCountry = "Mexico",
                        Currency = "mxn",
                    }
                };

                Tokenservice = new TokenService();
                stripeToken = Tokenservice.Create(Tokenoptions);
                // StripeLbl.Text = stripeToken.Id;


                HttpClient client = new HttpClient();
                var value_check = new Dictionary<string, string>
                         {
                            { "stripeToken", stripeToken.Id},
                            { "email"  , user[0].email},
                    {"id_washer", solx[0].id_washer },
                    {"id_solicitud", solx[0].id_solicitud },
                     {"tipo_pago", solx[0].forma_pago },
                     {"id_usuario", solx[0].id_usuario },
                      {"cambio", solx[0].cambio },
                    { "monto"  , solx[0].precio}
                         };


                var content = new FormUrlEncodedContent(value_check);
                var response = await client.PostAsync("http://www.washdryapp.com/app/public/pagos/generar", content);

                switch (response.StatusCode)
                {

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
                        await DisplayAlert("error", "yeah status 200", "ok");
                        userDataBase.DeleteSolicitud(Int32.Parse(solx[0].id_solicitud));
                        string xjson = await response.Content.ReadAsStringAsync();
                        await Navigation.PopAsync();
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
            catch (Exception ex)
            {
                var x = ex.ToString();
                //  StripeLbl.Text = ex.ToString() ;

            }

        }

    }
}