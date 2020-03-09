using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WashDry.Interface;
using WashDry.Models.DbModels;
using Xamarin.Forms;

namespace WashDry.SQLiteDb
{
    public class UserDataBase
    {
        private SQLiteConnection conn;

        public UserDataBase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<User>();
       
        }

        public IEnumerable<User> GetMembers()
        {
            var members = (from mem in conn.Table<User>() select mem);
            return members.ToList();
        }


        public string AddMember(User member)
        {
            try
            {
                conn.Insert(member);
                return "success baby bluye ;*";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

        }

        public void DeleteMember(int ID)
        {
            conn.Delete<User>(ID);
        }

       


    }
}
