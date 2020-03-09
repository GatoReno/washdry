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
            public string email { get; set; }
            public string username { get; set; }
            public string pass { get; set; }

            

           
           
        }
    
}
