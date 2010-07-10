using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Collections.Generic;
using Netology.NJoy.Security.Authorization;

public partial class UserManagement : System.Web.UI.Page
{
    private const string viewstate_ActionName = "aspnet_ActionName";
    private const string viewstate_UserName = "aspnet_UserName";
    private const string questionKey = "soru?";
    private const string answerKey = "cevap!";

    public string UserName
    {
        get
        {
            return ViewState[viewstate_UserName].ToString();
        }
        set
        {
            if (ViewState[viewstate_UserName] == null)
            {
                ViewState.Add(viewstate_UserName, value);
            }
            else
            {
                ViewState[viewstate_UserName] = value;
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

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UserName = GridView1.Rows[e.NewEditIndex].Cells[0].Text;
        ActionName = EditPanel.ID;
    }

    protected override void OnLoadComplete(EventArgs e)
    {
        ShowPanel(Page);

        if (ActionName == EditPanel.ID)
        {
            EP_RoleCheckBoxList.DataSource = Roles.GetAllRoles();
            EP_RoleCheckBoxList.DataBind();

            MembershipUser muser = Membership.GetUser(UserName);
            EP_UsernameTextBox.Text = UserName;
            EP_EmailTextBox.Text = muser.Email;
            string[] roles = Roles.GetRolesForUser(UserName);
            foreach (string role in roles)
            {
                EP_RoleCheckBoxList.Items.FindByText(role).Selected = true;
            }
        }
        else if (ActionName == AddPanel.ID)
        {
            AP_UsernameTextBox.Text = string.Empty;
            AP_EmailTextBox.Text = string.Empty;
            AP_RoleCheckBoxList.DataSource = Roles.GetAllRoles();
            AP_RoleCheckBoxList.DataBind();
        }
        else if (ActionName == ListPanel.ID)
        {
            UserName = string.Empty;
            GridView1.DataSource = Membership.GetAllUsers();
            GridView1.DataBind();
        }
        else if (ActionName == DeletePanel.ID)
        {
            if (UserName == User.Identity.Name)
            {
                ErrorMessage("Kendi kullanýcý kaydýnýzý silemezsiniz!");
                DeleteButton.Enabled = false;
            }
            else
            {
                DeleteButton.Enabled = true;
            }
            ConfirmationLabel.Text = string.Format("<b>{0}</b> isimli kullanýcýyý silmek istediðinize emin misiniz?", UserName);
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

    protected void AddUserButton_Click(object sender, EventArgs e)
    {
        ActionName = AddPanel.ID;
    }

    protected void AP_SaveButton_Click(object sender, EventArgs e)
    {
        MembershipCreateStatus status = new MembershipCreateStatus();
        MembershipUser muser = Membership.CreateUser(AP_UsernameTextBox.Text.Trim(), AP_PasswordTextBox.Text.Trim(), AP_EmailTextBox.Text.Trim(), questionKey, answerKey, true, out status);
        if (status == MembershipCreateStatus.Success)
        {
            Roles.AddUserToRoles(AP_UsernameTextBox.Text.Trim(), GetSelectedList(AP_RoleCheckBoxList));
            ActionName = ListPanel.ID;
            InfoMessage("Yeni kullanýcý eklendi");
        }
        else
        {
            ErrorMessage(status.ToString());
        }
    }

    protected void EP_SaveButton_Click(object sender, EventArgs e)
    {
        MembershipUser muser = Membership.GetUser(UserName);

        if (muser != null)
        {
            Roles.RemoveUserFromRoles(UserName, Roles.GetRolesForUser(UserName));

            Roles.AddUserToRoles(EP_UsernameTextBox.Text.Trim(), GetSelectedList(EP_RoleCheckBoxList));
            muser.Email = EP_EmailTextBox.Text.Trim();
            if (EP_PasswordTextBox.Text.Length > 0)
            {
                MembershipProvider p = Membership.Providers[muser.ProviderName];
                bool a = p.EnablePasswordRetrieval;
                string s = p.GetPassword(UserName, answerKey);
                muser.ChangePassword(muser.GetPassword(answerKey), EP_PasswordTextBox.Text.Trim());
            }
            Membership.UpdateUser(muser);
            ActionName = ListPanel.ID;
            InfoMessage("Deðiþiklikler kaydedildi");
        }
    }

    private string[] GetSelectedList(CheckBoxList checkBoxList)
    {
        List<string> selectedItems = new List<string>();
        foreach (ListItem item in checkBoxList.Items)
        {
            if (item.Selected) selectedItems.Add(item.Text);
        }

        return selectedItems.ToArray();
    }

    protected void EP_DeleteButton_Click(object sender, EventArgs e)
    {
        ActionName = DeletePanel.ID;
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        UserName = GridView1.Rows[e.RowIndex].Cells[0].Text;
        ActionName = DeletePanel.ID;
    }

    protected void AP_CancelButton_Click(object sender, EventArgs e)
    {
        ActionName = ListPanel.ID;
    }

    protected void EP_CancelButton_Click(object sender, EventArgs e)
    {
        ActionName = ListPanel.ID;
    }

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        Roles.RemoveUserFromRoles(UserName, Roles.GetRolesForUser(UserName));
        Membership.DeleteUser(UserName);

        ActionName = ListPanel.ID;
        InfoMessage("Kullanýcý silindi");
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
}
