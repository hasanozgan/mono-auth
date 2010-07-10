//
// Netology.NJoy.Security.Authorization.DBUtil.cs
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
using System.Configuration;
using System.Data.SqlClient;

namespace Netology.NJoy.Security.Authorization
{
    internal class DBUtil
    {
        private const string ConnectionStringName_ParameterName = "Authorization.ConnectionStringName";
        private const string ConnectionString_ParameterName = "Authorization.ConnectionString";

        // Add Web.Config in AppSettings...
        //<add key="Authorization.ConnectionStringName" value="Main.ConnectionString" />
        //<add key="Authorization.ConnectionString" value="data source=Serdb\SQL2005;initial catalog=Biddoor;persist security info=true;User ID=biddoor;Password=biddoor2007;packet size=4096" />

        private static string ConnectionStringName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings[ConnectionStringName_ParameterName];
            }
        }

        public static string ConnectionString 
        {
            get
            {
                if (ConnectionStringName == null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings[ConnectionString_ParameterName];
                }

                return System.Configuration.ConfigurationManager.AppSettings[ConnectionStringName];
            }
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
