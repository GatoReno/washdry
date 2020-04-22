using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calificar : ContentPage
    {
        public Calificar()
        {
            InitializeComponent();
        }

        private void RatingConception_Voted(object sender, EventArgs e)
        {

        }
    }
}