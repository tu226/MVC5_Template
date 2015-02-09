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
        public List<RoleModel> roles { get; set; }
        public static UserModel GetUser(int userid)
        {
            UserModel user = new UserModel();
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
            {
                user = cnx.Query<UserModel>("select userid,username from users where userid=@id", new { id = userid }).FirstOrDefault();
                user.roles = cnx.Query<RoleModel>("select rolename,roleid from usersroles as ur inner join roles as r on r.roleid=ur.roleid where userid=@id", new { id = userid }).ToList();
            }
            return user;

        }
        public static List<UserModel> GetUsers()
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
            {
                users = cnx.Query<UserModel>("select userid,username from users").ToList();
                foreach (UserModel user in users)
                {
                    user.roles = cnx.Query<RoleModel>("select rolename,r.roleid from usersroles as ur inner join roles as r on r.roleid=ur.roleid where userid=@id", new { id = user.userid }).ToList();
                }

                return users;
            }


        }
        public void Save()
        {

            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
            {
                cnx.Execute("update users set username=@username where userid=@id", new { id = userid, username = username });
                cnx.Execute("delete from usersroles where userid=@id", new { id = userid });
                if (this.roles != null)
                {
                    foreach (RoleModel role in this.roles)
                    {

                        cnx.Execute("insert into usersroles (userid,roleid) select @id,@roleid where not exists (select 1 from usersroles where userid=@id and roleid=@roleid)", new { id = userid, roleid = role.roleid });
                    }
                }

            }

        }
        public static UserModel Create(string username)
        {
            UserModel user = new UserModel { username = username };
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
            {
                user.userid = cnx.Query<int>("insert into users (username) select @username where not exists (select 1 from users where username=@username);select scope_identity();", new { username = username }).First();
            }
            return user;

        }

    }

        public class RoleModel
        {

            public string rolename { get; set; }
            public int roleid { get; set; }

            public static List<RoleModel> GetRoles()
            {
                using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
                    return cnx.Query<RoleModel>("select  r.roleid,rolename from  roles as r order by rolename").ToList<RoleModel>();
            }

            public void Save() {
                using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString))
                {
                    cnx.Execute("update roles set rolename=@rolename where roleid=@id", new { id = roleid,rolename=rolename });
                    

                }
            
            }

            public static RoleModel Create(string rolename){
                RoleModel role = new RoleModel { rolename=rolename};
                using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["laptop"].ConnectionString)) {
                   role.roleid= cnx.Query<int>("insert into roles (rolename) select @rolename where not exists (select 1 from roles where rolename=@rolename);select scope_identity();", new {rolename=rolename }).First();
                }
                return role;
            }

            

        }






    
}