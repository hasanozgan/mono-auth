//
// Netology.NJoy.Security.Authorization.Authorization.cs
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
using System.Web;
using System.Web.Security;

namespace Netology.NJoy.Security.Authorization
{
    public class Authorization : IAuthorization
    {
        private const string RoleListName = "nexum_RoleListName";

        private string username;

        private static Dictionary<string, Role> Roles
        {
            get
            {
                return (Dictionary<string, Role>)HttpContext.Current.Application[RoleListName];
            }
        }

        public Authorization(string username)
        {
            this.username = username;
        }

        public bool IsAuthorized(object permission, object grant)
        {
            return IsAuthorized(username, permission, grant);
        }

        public void Refresh()
        {
            Grant.Refresh();
            Permission.Refresh();
            Role.Refresh();
        }

        public void Update(object roles)
        {
            HttpContext.Current.Application.Remove(RoleListName);
            HttpContext.Current.Application.Add(RoleListName, roles);
        }

        private bool IsAuthorized(string username, object permission, object grant)
        {
            bool hasRole = false;
            int permissionId = (int)permission;
            int grantId = (int)grant;

            string[] roles = System.Web.Security.Roles.GetRolesForUser(username);

            foreach (string rolename in roles)
            {
                hasRole = Rule.Check(rolename, permissionId, grantId);
                if (hasRole) 
                    break;
            }

            return hasRole;
        }
    }
}
