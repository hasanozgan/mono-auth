//
// Netology.NJoy.Security.Authorization.Rule.cs
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

namespace Netology.NJoy.Security.Authorization
{
    public class Rule
    {
        private int permissionId;
        private Dictionary<int, bool> grants;
        private Dictionary<int, bool> hasGrants;

        public int PermissionId
        {
            get
            {
                return permissionId;
            }
            set
            {
                permissionId = value;
            }
        }
        public Dictionary<int, bool> Grants
        {
            get
            {
                return grants;
            }
            set
            {
                grants = value;
            }
        }
        public Dictionary<int, bool> HasGrants
        {
            get
            {
                return hasGrants;
            }
            set
            {
                hasGrants = value;
            }
        }

        public Rule()
        {
            grants = new Dictionary<int, bool>();
            hasGrants = new Dictionary<int, bool>();
            foreach (KeyValuePair<int, Grant> grant in Grant.Grants)
            {
                grants.Add(grant.Key, false);
                hasGrants.Add(grant.Key, false);
            }
        }

        public static Dictionary<int, Rule> FindRulesByRole(string rolename)
        {
            return Role.Roles[rolename].Rules;
        }

        public static bool Check(string rolename, int permissionid, int grantid)
        {
            return (Role.Roles[rolename].Rules[permissionid].Grants[grantid]);
        }
    }
}
