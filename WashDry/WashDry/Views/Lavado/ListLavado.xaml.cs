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
    public partial class ListLavado : ContentPage
    {
        public ListLavado()
        {
            InitializeComponent();
        }

        private async void ListLavados_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new InfoLavado());
        }
    }
}