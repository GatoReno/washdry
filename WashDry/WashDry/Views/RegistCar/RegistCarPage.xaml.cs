using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.RegistCar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistCarPage : ContentPage
    {
        public RegistCarPage()
        {
            InitializeComponent();
            gMain.BindingContext = this;
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
        }

        private void AgregarAutobtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}