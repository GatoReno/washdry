using LaavorRatingConception;
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
    public partial class LavadorInfo : ContentPage
    {
        public LavadorInfo()
        {
            InitializeComponent();
        }

        private void RatingConception_Voted(object sender, EventArgs e)
        {
            RatingConception rating = (RatingConception)sender;
            int index = rating.IndexVoted;
            int value = rating.Value;

            index_star.Text = index.ToString();
            value_star.Text = value.ToString();
           
        }
    }
}