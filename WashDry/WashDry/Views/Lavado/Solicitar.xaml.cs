using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views.Lavado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Solicitar : ContentPage
    {
        public string StepSelected { get; set; }
        public Solicitar()
        {
            InitializeComponent();
            lbltry.Text = stepBar.StepSelected.ToString();
            stepBar.Opacity = 0;
            stepBar.FadeTo(1,1000,null);
            stepBar.ScaleTo(1, 1000);
            //local: StepProgressBarControl




        }

        private async void DiceRollCancelledExecute()
        {
            await DisplayAlert("Dice Roll Cancelled", "got it", "OK");
        }

    }

    
 


}