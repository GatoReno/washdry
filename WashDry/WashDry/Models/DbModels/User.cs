using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WashDry.Models.DbModels
{

    public class User
    {

        [PrimaryKey]
        public int id { get; set; }
        public  int id_cliente { set; get; }
        public string nombre { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string token { get; set; }
        public int status { get; set; }
        public string email { get; set; }
        public string remember_token { get; set; }
        public string name { get; set; }
        public string google_id { get; set; }
    }




 
    
}
