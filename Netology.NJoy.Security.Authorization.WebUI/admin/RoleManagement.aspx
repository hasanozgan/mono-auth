<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="RoleManagement.aspx.cs" Inherits="RoleManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">

    <asp:Label ID="NotificationMessageLabel" runat="server" Font-Bold="True" ForeColor="Red"
        Text="Label" Visible="False"></asp:Label><br />
    <b>Action: </b><asp:Label ID="Info_ActionLabel" runat="server"></asp:Label>
    <hr />
    
    <asp:Panel ID="ListPanel" runat="server">
        <asp:LinkButton ID="AddRoleButton" runat="server" Text="Rol Ekle" OnClick="AddRoleButton_Click"></asp:LinkButton>
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Refresh</asp:LinkButton>
        <asp:GridView ID="RoleGridView" runat="server" AutoGenerateColumns="true" OnRowDeleting="RoleGridView_RowDeleting" OnRowEditing="RoleGridView_RowEditing">
            <Columns>
                <asp:CommandField EditText="D&#252;zenle" ShowEditButton="True" ShowDeleteButton="True" DeleteText="Sil"  />
            </Columns>
        </asp:GridView>
        
    </asp:Panel>

    <asp:Panel ID="AddPanel" runat="server">
        <table>
            <tr>
                <td><asp:Label ID="AP_RolenameLabel" runat="server" Text="Rol Adý" Font-Bold="True"></asp:Label></td>
                <td>
                    <strong>:</strong></td>
                <td><asp:TextBox ID="AP_RoleTextBox" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3">
                <asp:GridView ID="AP_PermissionGridView" runat="server" AutoGenerateColumns="false" Width="400px" >
                    <Columns>
                        <asp:TemplateField HeaderText="Yetki">
                            <ItemTemplate>
                                <asp:HiddenField ID="PermissionIdHiddenField" runat="server" Value='<%# Eval("PermissionId") %>' />
                                <asp:Label ID="PermissionName" runat="server" Text='<%# GetPermissionName(Eval("PermissionId")) %>'></asp:Label>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ekle">
                            <ItemTemplate><asp:CheckBox ID="CreateCheckBox" runat="server" Visible=<%# CreateGrantStatus(Eval("HasGrants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Oku">
                            <ItemTemplate><asp:CheckBox ID="ReadCheckBox" runat="server" Visible=<%# ReadGrantStatus(Eval("HasGrants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Güncelle">
                            <ItemTemplate><asp:CheckBox ID="UpdateCheckBox" runat="server" Visible=<%# UpdateGrantStatus(Eval("HasGrants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Sil">
                            <ItemTemplate><asp:CheckBox ID="DeleteCheckBox" runat="server" Visible=<%# DeleteGrantStatus(Eval("HasGrants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Çalýþtýr">
                            <ItemTemplate><asp:CheckBox ID="ExecuteCheckBox" runat="server" Visible=<%# ExecuteGrantStatus(Eval("HasGrants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>                                                                                             
                    </Columns>          
                </asp:GridView>
                </td>
            </tr>          
            <tr>
                <td colspan="3"><hr /></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="AP_SaveButton" runat="server" Text="Kaydet" OnClick="AP_SaveButton_Click" />
                </td>
                <td colspan="2" align="right">
                    <asp:Button ID="AP_CancelButton" runat="server" Text="Vazgeç" OnClick="AP_CancelButton_Click"  />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="EditPanel" runat="server">
		<table>
            <tr>
                <td><asp:Label ID="EP_RolenameLabel" runat="server" Text="Rol Adý"></asp:Label></td>
                <td>:</td>
                <td><asp:TextBox ID="EP_RolenameTextBox" runat="server" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3">
                <asp:GridView ID="EP_PermissionGridView" runat="server" AutoGenerateColumns="false" Width="400px">
                    <Columns>
                        <asp:TemplateField HeaderText="Yetki">
                            <ItemTemplate>
                                <asp:HiddenField ID="PermissionIdHiddenField" runat="server" Value='<%# Eval("PermissionId") %>' />                            
                                <asp:Label ID="PermissionName" runat="server" Text='<%# GetPermissionName(Eval("PermissionId")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ekle">
                            <ItemTemplate><asp:CheckBox ID="CreateCheckBox" runat="server" Visible=<%# CreateGrantStatus(Eval("HasGrants")) %> Checked=<%# CreateGrantStatus(Eval("Grants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Oku">
                            <ItemTemplate><asp:CheckBox ID="ReadCheckBox" runat="server" Visible=<%# ReadGrantStatus(Eval("HasGrants")) %> Checked=<%# ReadGrantStatus(Eval("Grants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Güncelle">
                            <ItemTemplate><asp:CheckBox ID="UpdateCheckBox" runat="server" Visible=<%# UpdateGrantStatus(Eval("HasGrants")) %> Checked=<%# UpdateGrantStatus(Eval("Grants")) %> /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Sil">
                            <ItemTemplate><asp:CheckBox ID="DeleteCheckBox" runat="server" Visible=<%# DeleteGrantStatus(Eval("HasGrants")) %> Checked=<%# DeleteGrantStatus(Eval("Grants")) %>  /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Çalýþtýr">
                            <ItemTemplate><asp:CheckBox ID="ExecuteCheckBox" runat="server" Visible=<%# ExecuteGrantStatus(Eval("HasGrants")) %> Checked=<%# ExecuteGrantStatus(Eval("Grants")) %>  /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>                                                                                             
                    </Columns>  
                </asp:GridView>                
                </td>
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
        <asp:Button ID="DeleteButton" runat="server" Text="Evet" OnClick="DeleteButton_Click" />
        <asp:Button ID="CancelButton" runat="server" Text="Hayýr" OnClick="CancelButton_Click" /></asp:Panel>

</asp:Content>

