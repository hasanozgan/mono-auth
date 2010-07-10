using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ConstantEnums;

public partial class _Default : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NewsEntityCreateLabel.Visible = Authorization.IsAuthorized(Permission.NewsEntity, Grant.Create);
        NewsEntityReadLabel.Visible = Authorization.IsAuthorized(Permission.NewsEntity, Grant.Read);
        NewsEntityUpdateLabel.Visible = Authorization.IsAuthorized(Permission.NewsEntity, Grant.Update);
        NewsEntityDeleteLabel.Visible = Authorization.IsAuthorized(Permission.NewsEntity, Grant.Delete);
        EstateWFExecuteLabel.Visible = Authorization.IsAuthorized(Permission.EstateAnalysisWF, Grant.Execute);
    }
}
