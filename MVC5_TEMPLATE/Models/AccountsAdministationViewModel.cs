using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;


namespace Adrien.Template.Models
{

    public class UserModel
    {
        public string username { get; set; }
        public int userid { get; set; }
        public List<string> roles { get; set; }
    }

    public class AccountsAdministationViewModel
    {

        public List<UserModel> users { get; set; }
        public List<string> roles { get; set; }

        public AccountsAdministationViewModel()
        {

            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
            {
                users = cnx.Query<UserModel>("select userid,username from users").ToList<UserModel>();
                foreach (UserModel user in users)
                {
                    user.roles = cnx.Query<string>("select rolename from UsersRoles where userid=@id", new { id = user.userid }).ToList<string>();
                }
                roles = cnx.Query<string>("select rolename from Usersroles group by rolename").ToList<string>();
            }

        }


    }

    public class EditUserControllerViewModel {

        public UserModel user { get; set; }
        public List<string> roles { get; set; }
        public EditUserControllerViewModel(int userid) {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
            {
                user = cnx.Query<UserModel>("select userid,username from users where userid=@id", new { id=userid }).FirstOrDefault();
                user.roles = cnx.Query<string>("select rolename from UsersRoles where userid=@id", new { id = user.userid }).ToList<string>();
                roles = cnx.Query<string>("select rolename from usersroles where userid<>@id  group by rolename order by rolename", new { id = userid }).ToList<string>();
            }

        }

    
    }

    
}