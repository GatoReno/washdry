using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WashDry.Interface;
using WashDry.Models.ApiModels;
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
            conn.CreateTable<Solicitudes>();
       
        }

        public void DeleteSolicitudes()
        {
            conn.DeleteAll<Solicitudes>();
        }

        public IEnumerable<Solicitudes> GetSolicitudes() {
            var members = (from mem in conn.Table<Solicitudes>() select mem);
            return members.ToList();
        }
        public void DeleteSolicitud(int ID)
        {
            conn.Delete<Solicitudes>(ID);
        }

         
        public IEnumerable<Solicitudes> GetSolicitud(int id) {
            var data = conn.Table<Solicitudes>();
            var xs = id.ToString();
            var  d1 = (from values in data
                      where values.id_solicitud == xs
                      select values).ToList();

            return d1;
        }
        public string AddSolicitudes(Solicitudes member)
        {
            try
            {
                conn.Insert(member);
                return "success Solicitudes added ";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

        }



        // Member areas
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

        public void DeleteMembers()
        {
            conn.DeleteAll<User>();
        }

        public string UpdateMemberToken(int id, string token)
        {
            try
            {
                var res = "Fallo";
                var data = conn.Table<User>();
                var d1 = (from values in data
                          where values.id == id
                          select values).Single();
                if (true)
                {
                    d1.token = token;
                    //d1.status = status;
                    conn.Update(d1);
                    res = "Correcto";
                }
                return "Res->" + res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string UpdateMember(int id,  string username, string email, string nombre, string password,   int status)
        {
            try
            {
                var res = "Fallo";
                var data = conn.Table<User>();
                var d1 = (from values in data
                          where values.id == id
                          select values).Single();
                if (true)
                {
                     d1.nombre = nombre;
                    d1.username = username;
                    d1.email = email;
                    d1.password = password;
                     //d1.foto = foto;
                    d1.status = status;
                    conn.Update(d1);
                    res = "Correcto";
                }
                return "Res->" + res;
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
