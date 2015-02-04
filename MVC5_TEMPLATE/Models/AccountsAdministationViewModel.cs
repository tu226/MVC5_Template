using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
namespace Adrien.Template.Models
{
    public class AccountsAdministationViewModel
    {
        public List<User> users { get; set; }

        public AccountsAdministationViewModel() {
            
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString)) {
                cnx.Query<User>("select * from ");
            }

        }

        public class User {
            string username { get; set; }
            int userid { get; set; }
            List<string> roles { get; set; }
        }

        

    }
}