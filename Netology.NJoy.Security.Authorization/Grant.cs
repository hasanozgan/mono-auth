//
// Netology.NJoy.Security.Authorization.Grant.cs
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
using System.Data.SqlClient;

namespace Netology.NJoy.Security.Authorization
{
    public class Grant
    {
        private const string GrantAppName = "Grant_AppName";

        private int id;
        private int typeId;
        private string name;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static Dictionary<int, Grant> Grants
        {
            get
            {
                if (HttpContext.Current.Application[GrantAppName] == null)
                {
                    HttpContext.Current.Application[GrantAppName] = new Dictionary<int, Grant>();
                    Refresh();
                }

                return (Dictionary<int, Grant>)HttpContext.Current.Application[GrantAppName];
            }
        }

        public static void Refresh()
        {
            SqlConnection sqlConnection = DBUtil.CreateConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.Connection.Open();

            cmd.CommandText = @"SELECT Id, PermissionTypeId, Name FROM [Grant]";

            SqlDataReader reader = cmd.ExecuteReader();

            Grants.Clear();
            while (reader.Read())
            {
                Grant grant = new Grant();
                grant.Id = reader.GetInt32(0);
                grant.TypeId = reader.GetInt32(1);
                grant.Name = reader.GetString(2);

                Grants.Add(grant.Id, grant);
            }

            cmd.Connection.Close();
        }


        public static Grant FindGrantById(int id)
        {
            return Grants[id];
        }

        public static Grant FindGrantByName(string name)
        {
            foreach (KeyValuePair<int, Grant> item in Grants)
            {
                if (item.Value.Name == name) return item.Value;
            }

            return null;
        }
    }
}
