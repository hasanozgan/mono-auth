<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="~/admin/UserManagement.aspx.cs" Inherits="UserManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">

    <asp:Label ID="NotificationMessageLabel" runat="server" Font-Bold="True" ForeColor="Red"
        Text="Label" Visible="False"></asp:Label><br />
    <b>Action: </b><asp:Label ID="Info_ActionLabel" runat="server"></asp:Label>
    <hr />
    
    <asp:Panel ID="ListPanel" runat="server">
        <asp:LinkButton ID="AddUserButton" runat="server" Text="Kullanýcý Ekle" OnClick="AddUserButton_Click"></asp:LinkButton>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowEditing="GridView1_RowEditing" OnRowDeleting="GridView1_RowDeleting">
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Kullanýcý" />
                <asp:BoundField DataField="Email" HeaderText="E-Posta" />
                <asp:CheckBoxField DataField="IsOnline" HeaderText="Aktif mi?" />
                <asp:BoundField DataField="LastLoginDate" HeaderText="Son Giriþ Tarihi" />
                <asp:CommandField EditText="Düzenle" ShowEditButton="true" ShowDeleteButton="true" DeleteText="Sil"  />
            </Columns>
        </asp:GridView>
        
        <asp:GridView ID="HataTable" runat="server">
        
        </asp:GridView>
        
    </asp:Panel>

    <asp:Panel ID="AddPanel" runat="server">
        <table>
            <tr>
                <td><asp:Label ID="AP_UsernameLabel" runat="server" Text="Kullanýcý Adý"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="AP_UsernameTextBox" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="AP_EmailLabel" runat="server" Text="Eposta"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="AP_EmailTextBox" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="AP_PasswordLabel" runat="server" Text="Þifre"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="AP_PasswordTextBox" TextMode="Password" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="AP_Password2Label" runat="server" Text="Þifre Tekrar"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="AP_Password2TextBox" TextMode="Password" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td valign="top"><asp:Label ID="AP_RoleLabel" runat="server" Text="Roller"></asp:Label></td>
                <td>:</td>
                <td>
                    <asp:CheckBoxList ID="AP_RoleCheckBoxList" runat="server">
                    </asp:CheckBoxList></td>
            </tr>            
            <tr>
                <td colspan="3"><hr /></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="AP_SaveButton" runat="server" Text="Kaydet" OnClick="AP_SaveButton_Click" />
                </td>
                <td colspan="2" align="right">
                    <asp:Button ID="AP_CancelButton" runat="server" Text="Vazgeç" OnClick="AP_CancelButton_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="EditPanel" runat="server">
		<table>
            <tr>
                <td><asp:Label ID="EP_UsernameLabel" runat="server" Text="Kullanýcý Adý"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="EP_UsernameTextBox" runat="server" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="EP_EmailLabel" runat="server" Text="Eposta"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="EP_EmailTextBox" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="EP_PasswordLabel" runat="server" Text="Þifre"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="EP_PasswordTextBox" TextMode="Password" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="EP_Password2Label" runat="server" Text="Þifre Tekrar"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="EP_Password2TextBox" TextMode="Password" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td valign="top"><asp:Label ID="EP_RoleLabel" runat="server" Text="Roller"></asp:Label></td>
                <td>:</td>
                <td>
                    <asp:CheckBoxList ID="EP_RoleCheckBoxList" runat="server">
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td colspan="3"><hr /></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="EP_SaveButton" runat="server" Text="Kaydet" OnClick="EP_SaveButton_Click" />
                    <asp:Button ID="EP_DeleteButton" runat="server" Text="Sil" OnClick="EP_DeleteButton_Click" />
                </td>
                <td colspan="2" align="right">
                    <asp:Button ID="EP_CancelButton" runat="server" Text="Vazgeç" OnClick="EP_CancelButton_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="DeletePanel" runat="server">
        <br />
        <asp:Label ID="ConfirmationLabel" runat="server" Text="Label"></asp:Label><br />
        <br />
        <asp:Button ID="DeleteButton" runat="server" OnClick="DeleteButton_Click" Text="Evet" />
        <asp:Button ID="CancelButton" runat="server" OnClick="CancelButton_Click" Text="Hayýr" /></asp:Panel>

</asp:Content>

