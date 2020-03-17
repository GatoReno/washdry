using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WashDry.Models.DbModels
{
     
        public class User
        {

            [PrimaryKey]
    
        public string id { get; set; }
        public string nombre { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string remember_token { get; set; }
        public string name { get; set; }
        public string google_id { get; set; }
    }




 
    
}
