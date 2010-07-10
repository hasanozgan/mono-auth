using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ConstantEnums
{
    public enum Permission : int
    {
        Unknown = 0,
        NewsEntity = 1,
        EstateEntity = 2,
        EstateAnalysisWF = 3,
    }

    public enum Grant : int
    {
        Unknown = 0,
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4,
        Execute = 5,
    }
}