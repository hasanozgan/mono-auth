using System;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using Netology.NJoy.Security.Authorization;

public partial class RoleManagement : System.Web.UI.Page
{
    private const string viewstate_ActionName = "aspnet_ActionName";
    private const string viewstate_RoleName = "aspnet_RoleName";

    public string RoleName
    {
        get
        {
            return ViewState[viewstate_RoleName].ToString();
        }
        set
        {
            if (ViewState[viewstate_RoleName] == null)
            {
                ViewState.Add(viewstate_RoleName, value);
            }
            else
            {
                ViewState[viewstate_RoleName] = value;
            }
        }
    }
    public string ActionName
    {
        get
        {
            return ViewState[viewstate_ActionName].ToString();
        }
        set
        {
            if (ViewState[viewstate_ActionName] == null)
            {
                ViewState.Add(viewstate_ActionName, value);
            }
            else
            {
                ViewState[viewstate_ActionName] = value;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        NotificationMessageLabel.Text = "";
        NotificationMessageLabel.Visible = false;

        if (!IsPostBack)
        {
            ActionName = ListPanel.ID;
        }
    }

    protected override void OnLoadComplete(EventArgs e)
    {
        ShowPanel(Page);

        if (ActionName == EditPanel.ID)
        {
            EP_PermissionGridView.DataSource = Role.Roles[RoleName].Rules.Values;
            EP_PermissionGridView.DataBind();

            EP_RolenameTextBox.Text = RoleName;
        }
        else if (ActionName == AddPanel.ID)
        {
            AP_RoleTextBox.Text = string.Empty;

            Role role = new Role("unknown");
            AP_PermissionGridView.DataSource = role.Rules.Values;
            AP_PermissionGridView.DataBind();
        }
        else if (ActionName == ListPanel.ID)
        {
            RoleName = string.Empty;
            RoleGridView.DataSource = Roles.GetAllRoles();
            RoleGridView.DataBind();
        }
        else if (ActionName == DeletePanel.ID)
        {
            ConfirmationLabel.Text = string.Format("<b>{0}</b> isimli rolü silmek istediðinize emin misiniz?", RoleName);

            string[] usernames  = System.Web.Security.Roles.FindUsersInRole(RoleName, "%");

            if (usernames.Length > 0)
            {
                ConfirmationLabel.Text += string.Format("<p/><font color='#DD0000'>Dikkat! Aþaðýdaki kullanýcýlara <b>{0}</b> isimli rol atanmýþtýr.<br/></font>{1}",
                                                        RoleName,
                                                        String.Join("<br/>", usernames)
                                          );
            }
        }

        if (NotificationMessageLabel.Text.Length > 0)
            NotificationMessageLabel.Visible = true;

        Info_ActionLabel.Text = ActionName;

        base.OnLoadComplete(e);
    }

    private void ShowPanel(Control parentControl)
    {
        foreach (Control ctrl in parentControl.Controls)
        {
            if (ctrl is System.Web.UI.WebControls.Panel)
            {
                ctrl.Visible = (ctrl.ID == ActionName);
            }

            if (ctrl.Controls.Count > 0)
            {
                ShowPanel(ctrl);
            }
        }
    }

    protected void AddRoleButton_Click(object sender, EventArgs e)
    {
        ActionName = AddPanel.ID;
    }

    protected void RoleGridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RoleName = RoleGridView.Rows[e.NewEditIndex].Cells[1].Text;
        ActionName = EditPanel.ID;
    }
    protected void RoleGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        RoleName = RoleGridView.Rows[e.RowIndex].Cells[1].Text;
        ActionName = DeletePanel.ID;
    }

    private bool GetGrantStatus(object grantCollection, ConstantEnums.Grant grantType)
    {
        Dictionary<int, bool> grants = (Dictionary<int, bool>)grantCollection;

        return grants[Convert.ToInt32(grantType)];
    }

    protected bool CreateGrantStatus(object grantCollection)
    {
        return GetGrantStatus(grantCollection, ConstantEnums.Grant.Create);
    }

    protected bool ReadGrantStatus(object grantCollection)
    {
        return GetGrantStatus(grantCollection, ConstantEnums.Grant.Read);
    }

    protected bool UpdateGrantStatus(object grantCollection)
    {
        return GetGrantStatus(grantCollection, ConstantEnums.Grant.Update);
    }

    protected bool DeleteGrantStatus(object grantCollection)
    {
        return GetGrantStatus(grantCollection, ConstantEnums.Grant.Delete);
    }

    protected bool ExecuteGrantStatus(object grantCollection)
    {
        return GetGrantStatus(grantCollection, ConstantEnums.Grant.Execute);
    }

    protected string GetPermissionName(object permissionId)
    {
        return Netology.NJoy.Security.Authorization.Permission.Permissions[Convert.ToInt32(permissionId)].Name;
    }

    protected void AP_SaveButton_Click(object sender, EventArgs e)
    {
        string roleName = AP_RoleTextBox.Text.Trim();
        Role role = new Role(roleName);

        foreach (GridViewRow row in AP_PermissionGridView.Rows)
        {
            HiddenField hiddenField = (HiddenField)row.FindControl("PermissionIdHiddenField");
            int permissionId = Convert.ToInt32(hiddenField.Value);

            foreach (KeyValuePair<int, Grant> item in Grant.Grants)
            {
                CheckBox checkBox = (CheckBox)row.FindControl(item.Value.Name+"CheckBox");
                if (checkBox.Visible && checkBox.Checked) role.Rules[permissionId].Grants[item.Value.Id] = true;
            }
        }

        if (Role.CreateRole(role))
        {
            InfoMessage("Yeni rol eklendi!.");
            RoleName = roleName;
            ActionName = EditPanel.ID;
        }
        else
        {
            ErrorMessage("Rol eklenirken hata oluþtu!..");
        }
    }
    protected void EP_SaveButton_Click(object sender, EventArgs e)
    {
        Role role = new Role(RoleName);

        foreach (GridViewRow row in EP_PermissionGridView.Rows)
        {
            HiddenField hiddenField = (HiddenField)row.FindControl("PermissionIdHiddenField");
            int permissionId = Convert.ToInt32(hiddenField.Value);

            foreach (KeyValuePair<int, Grant> item in Grant.Grants)
            {
                CheckBox checkBox = (CheckBox)row.FindControl(item.Value.Name + "CheckBox");
                if (checkBox.Visible && checkBox.Checked) role.Rules[permissionId].Grants[item.Value.Id] = true;
            }
        }

        if (Role.EditRole(role))
        {
            InfoMessage("Rol yetkileri baþarýlý bir þekilde güncellendi!");
            ActionName = EditPanel.ID;
        }
        else
        {
            ErrorMessage("Rol'e ait yetkiler güncellenirken hata oluþtu!");
        }
    }
    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        Role.DeleteRole(RoleName);
        
        ActionName = ListPanel.ID;
    }
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        ActionName = ListPanel.ID;
    }

    private void ErrorMessage(string message)
    {
        NotificationMessageLabel.Text = message;
        NotificationMessageLabel.ForeColor = Color.Red;
    }

    private void InfoMessage(string message)
    {
        NotificationMessageLabel.Text = message;
        NotificationMessageLabel.ForeColor = Color.Green;
    }

    protected void EP_DeleteButton_Click(object sender, EventArgs e)
    {
        ActionName = DeletePanel.ID;
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Role.Refresh();
    }

    protected void EP_CancelButton_Click(object sender, EventArgs e)
    {
        ActionName = ListPanel.ID;
    }

    protected void AP_CancelButton_Click(object sender, EventArgs e)
    {
        ActionName = ListPanel.ID;
    }
}
