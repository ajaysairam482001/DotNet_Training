<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Validate.aspx.cs" Inherits="LineEndingIssue.Validator" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Line Ending Issue Validator</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Enter your details:</h1>
        <table>
            <tr>
                <td>Name:</td>
                <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Family Name:</td>
                <td><asp:TextBox ID="txtFamilyName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Address:</td>
                <td><asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>City:</td>
                <td><asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Zip Code:</td>
                <td><asp:TextBox ID="txtZipCode" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Phone:</td>
                <td><asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>E-mail Address:</td>
                <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnCheck" runat="server" Text="Check" OnClick="btnCheck_Click" />
                </td>
            </tr>
        </table>

      
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>  
    </form>
</body>
</html>
