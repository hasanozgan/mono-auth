//
// Netology.NJoy.Security.Authorization.Role.cs
//
// Authors:
// 	Hasan Ozgan  (hasan@ozgan.net)
//
// (C) 2007 Netology Software Foundation. (http://www.netology.org)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.Data.SqlClient;

namespace Netology.NJoy.Security.Authorization
{
    public class Role
    {
        private const string RoleAppName = "Role_AppName";

        private string name;
        private Dictionary<int, Rule> rules;

        public string Name
        {
            get { return name; }
        }

        public Dictionary<int, Rule> Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        public Role(string name)
        {
            this.name = name;
            this.rules = new Dictionary<int, Rule>();
            foreach (KeyValuePair<int, Permission> permission in Permission.Permissions)
            {
                Rule item = new Rule();
                item.PermissionId = permission.Key;

                foreach (KeyValuePair<int, Grant> grant in Grant.Grants)
                {
                    item.HasGrants[grant.Key] = (Permission.Permissions[permission.Key].TypeId == grant.Value.TypeId);
                }

                this.rules.Add(permission.Key, item);
            }
        }


        public static Dictionary<string, Role> Roles
        {
            get
            {
                if (HttpContext.Current.Application[RoleAppName] == null)
                {
                    HttpContext.Current.Application[RoleAppName] = new Dictionary<string, Role>();
                    Refresh();
                }

                return (Dictionary<string, Role>)HttpContext.Current.Application[RoleAppName];
            }
        }

        public static void Refresh()
        {
            string[] roles = System.Web.Security.Roles.GetAllRoles();

            Roles.Clear();
            foreach (string rolename in roles)
            {
                Roles.Add(rolename, PrepareRules(rolename));
            }
        }

        private static Role PrepareRules(string rolename)
        {
            SqlConnection sqlConnection = DBUtil.CreateConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.Connection.Open();

            cmd.CommandText = @"
                SELECT PermissionId, GrantId FROM PermissionRules
                    WHERE RoleName = @RoleName
                    ORDER BY PermissionId, GrantId 
                ";
            //USE PARAMETERS FOR SECURITY
            cmd.Parameters.Add(new SqlParameter("@RoleName", rolename.Trim()));
            SqlDataReader reader = cmd.ExecuteReader();

            Role role = new Role(rolename);
            while (reader.Read())
            {
                int permissionId = reader.GetInt32(0);
                int grantId = reader.GetInt32(1);

                role.Rules[permissionId].Grants[grantId] = true;
            }
            cmd.Connection.Close();

            return role;
        }

        public static Role FindRoleByName(string rolename)
        {
            return Roles[rolename];
        }

        public static bool CreateRole(Role role)
        {
            bool transactionStatus = false;
            SqlConnection sqlConnection = DBUtil.CreateConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.Connection.Open();
            cmd.Transaction = sqlConnection.BeginTransaction("CreateRole");

            try
            {
                System.Web.Security.Roles.CreateRole(role.Name);

                cmd.CommandText = CreateRoleSqlScript(role);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();

                transactionStatus = true;
                Refresh();
            }
            catch
            {
                System.Web.Security.Roles.DeleteRole(role.Name);
                cmd.Transaction.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }

            return transactionStatus;
        }

        public static bool RemoveRules(string roleName)
        {
            bool transactionStatus = false;
            SqlConnection sqlConnection = DBUtil.CreateConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.Connection.Open();
            cmd.Transaction = sqlConnection.BeginTransaction("RemoveRules");

            try
            {
               cmd.CommandText = RemoveRulesSqlScript(roleName);
               cmd.ExecuteNonQuery();
               cmd.Transaction.Commit();

               transactionStatus = true;
               Refresh();
            }
            catch (Exception)
            {
                cmd.Transaction.Rollback();
            }

            return transactionStatus;
        }

        public static bool DeleteRole(string roleName)
        {
            string[] usernames  = System.Web.Security.Roles.FindUsersInRole(roleName, "%");
            if (usernames.Length > 0)
            {
                System.Web.Security.Roles.RemoveUsersFromRole(usernames, roleName);
            }
            if (RemoveRules(roleName) && System.Web.Security.Roles.DeleteRole(roleName))
            {
                Refresh();
                return true;
            }

            return false;
        }

        public static bool EditRole(Role role)
        {
            bool transactionStatus = false;
            SqlConnection sqlConnection = DBUtil.CreateConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.Connection.Open();
            cmd.Transaction = sqlConnection.BeginTransaction("EditRole");

            try
            {
               cmd.CommandText = RemoveRulesSqlScript(role.Name);
               cmd.CommandText += CreateRoleSqlScript(role);
               cmd.ExecuteNonQuery();
               cmd.Transaction.Commit();

               transactionStatus = true;
               Refresh();
            }
            catch (Exception)
            {
                cmd.Transaction.Rollback();
            }

            return transactionStatus;
        }


        private static string CreateRoleSqlScript(Role role)
        {
            string commandText = string.Empty;
            const string InsertQueryTemplate = @"
                    INSERT INTO PermissionRules (RoleName, PermissionId, GrantId) 
                    VALUES ('{0}', {1}, {2});
                ";

            foreach (KeyValuePair<int, Rule> rule in role.Rules)
            {
                foreach (KeyValuePair<int, Grant> grant in Grant.Grants)
                {
                    if (rule.Value.HasGrants[grant.Value.Id] && rule.Value.Grants[grant.Value.Id])
                    {
                        commandText += string.Format(InsertQueryTemplate,
                                                role.Name,
                                                rule.Value.PermissionId,
                                                grant.Value.Id
                                        );
                    }
                }
            }

            return commandText;
        }

        private static string RemoveRulesSqlScript(string rolename)
        {
            return string.Format("DELETE FROM PermissionRules WHERE RoleName LIKE '{0}';", rolename);
        }

    }
}
