using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Netology.NJoy.Security.Authorization;

/// <summary>
/// Summary description for BasePage
/// </summary>
public abstract class BasePage : System.Web.UI.Page
{
    private IAuthorization _authorization;

    protected IAuthorization Authorization
    {
        get
        {
            return _authorization;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        try
        {
            _authorization = new Authorization(User.Identity.Name);
        }
        catch { }

        base.OnInit(e);
    }


}
