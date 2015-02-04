
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Net.Security;
    using Microsoft.AspNet.Identity;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Dapper;
    using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
    namespace Adrien.Template
    {

        public class User : IUser
        {
            public string Id { get; set; }
            
            public string UserName { get; set; }

            public string PasswordHash { get; set; }

          

        }



        public class UserStore : IUserStore<User>, IUserRoleStore<User>, IUserPasswordStore<User>
        {

            private readonly string connectionString;
            public UserStore(string connectionString)
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new ArgumentNullException("connectionString");

                this.connectionString = connectionString;
            }

            public UserStore()
            {
                this.connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }


            System.Threading.Tasks.Task IUserStore<User, string>.CreateAsync(User user)
            {
                if (user == null)
                {
                    throw new ArgumentNullException("user");
                }
                return Task.Factory.StartNew(() =>
                {

                    using (SqlConnection connection = new SqlConnection(connectionString))
                        connection.Execute("insert into Users( UserName, PasswordHash) values(@UserName, @PasswordHash)", user);
                });
            }

            System.Threading.Tasks.Task IUserStore<User, string>.DeleteAsync(User user)
            {
                if (user == null)
                {
                    throw new ArgumentNullException("user");
                }
                return Task.Factory.StartNew(() =>
                {

                    using (SqlConnection connection = new SqlConnection(connectionString))
                        connection.Execute("delete from users where userid=@id", new { id = user.Id });
                });
            }

            System.Threading.Tasks.Task<User> IUserStore<User, string>.FindByIdAsync(string userId)
            {
                if (userId == null)
                {
                    throw new ArgumentNullException("userId");
                }
                return Task.Factory.StartNew(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                        return connection.Query<User>("select UserId as Id,UserName,PasswordHash from Users where UserId = @userId", new { userId = userId }).SingleOrDefault();
                });
            }

            System.Threading.Tasks.Task<User> IUserStore<User, string>.FindByNameAsync(string userName)
            {
                if (string.IsNullOrWhiteSpace(userName))
                    throw new ArgumentNullException("userName");

                return Task.Factory.StartNew(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString)) {
                        var u = connection.Query<User>("select UserId as id,UserName,PasswordHash from Users where lower(UserName) = lower(@userName)", new { userName = userName }).DefaultIfEmpty(null).FirstOrDefault();
                        return u;
                    }
                
                });
            }

            System.Threading.Tasks.Task IUserStore<User, string>.UpdateAsync(User user)
            {
                if (user == null)
                    throw new ArgumentNullException("user");

                return Task.Factory.StartNew(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                        connection.Execute("update Users set UserName = @UserName, Password = @Password where UserId = @userId", user);
                });
            }

            void IDisposable.Dispose()
            {

            }

            Task IUserRoleStore<User, string>.AddToRoleAsync(User user, string roleName)
            {
                return Task.Factory.StartNew(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                        connection.Execute("insert into UsersRoles (UserID,RoleName) values (@UserId,@roleName)", new { UserId = user.Id, roleName = roleName });
                });
            }

            Task<IList<string>> IUserRoleStore<User, string>.GetRolesAsync(User user)
            {
                return Task.Factory.StartNew(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                        return (IList<string>)connection.Query<string>("select rolename from UsersRoles where UserId=@UserId group by rolename", new { UserId = user.Id }).ToList();
                });
            }

            Task<bool> IUserRoleStore<User, string>.IsInRoleAsync(User user, string roleName)
            {
                return Task.Factory.StartNew(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        dynamic result = connection.Query("select count(*) as ct from UsersRoles where UserId=@UserId,RoleName=@roleName", new { UserId = user.Id, roleName = roleName }).Single();
                        return (bool)(result.ct > 0);
                    }
                });
            }

            Task IUserRoleStore<User, string>.RemoveFromRoleAsync(User user, string roleName)
            {
                return Task.Factory.StartNew(() =>
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                        connection.Execute("delete from UsersRoles where UserId=@UserId and RoleName=@roleName", new { UserId = user.Id, roleName = roleName });
                });
            }

            Task<string> IUserPasswordStore<User, string>.GetPasswordHashAsync(User user)
            {
                return Task.FromResult(user.PasswordHash);
            }

            Task<bool> IUserPasswordStore<User, string>.HasPasswordAsync(User user)
            {
                return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
            }

            Task IUserPasswordStore<User, string>.SetPasswordHashAsync(User user, string password)
            {
                user.PasswordHash = password;

                return Task.FromResult(0);
            }

            
        }

        public class CustomUserValidator<TUser> : IIdentityValidator<TUser>
    where TUser : class, Microsoft.AspNet.Identity.IUser
        {
         
            private readonly UserManager<TUser> _manager;

            public CustomUserValidator()
            {
            }

            public CustomUserValidator(UserManager<TUser> manager)
            {
                _manager = manager;
            }

            public async Task<IdentityResult> ValidateAsync(TUser item)
            {
                var errors = new List<string>();
               
                if (_manager != null)
                {
                    var otherAccount = await _manager.FindByNameAsync(item.UserName);
                    if (otherAccount != null )
                        errors.Add("Ce nom d'utilisateur existe déjà");
                }

                return errors.Any()
                    ? IdentityResult.Failed(errors.ToArray())
                    : IdentityResult.Success;
            }
        }


    }
