using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WashDry.Models.DbModels;
using WashDry.SQLiteDb;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WashDry.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Test : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public User user;
        public UserDataBase userDb;
        public Test()
        {
            InitializeComponent();

            userDb = new UserDataBase();
            var users = userDb.GetMembers();         
            //userView.ItemsSource = users;
        }

       

        /*
 private async void GuardarUser_Clicked(object sender, EventArgs e)
 {
     try
     {

         // insert user a db 
         user = new User();
         user.email = Email.Text;
         user.pass = Pass.Text;
         Random rnd = new Random();
         user.id = rnd.Next(20);
         userDb.AddMember(user);


         // llamar users a list view
         var users = userDb.GetMembers();
         userView.ItemsSource = users; 
     }
     catch (Exception ex)
     {

         await DisplayAlert("Error", "Error : " + ex.ToString(), "ok");
     }
 }

 private async void userView_ItemTapped(object sender, ItemTappedEventArgs e)
 {

         var content_X = e.Item as User;

         await DisplayAlert("User", "userinfo : " + content_X.email
             + " "+ content_X.pass + " "+ content_X.id
             , "ok");

 }

 */
    }
    }