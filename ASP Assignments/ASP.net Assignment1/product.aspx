
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Products.Products" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Products</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <asp:DropDownList ID="ddlProducts" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProducts_SelectedIndexChanged">
           </asp:DropDownList>
            <br />
            <asp:Image ID="imgProduct" runat="server" Width="200px" Height="150px" />
            <br />
            <asp:Label ID="lblPrice" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btnGetPrice" runat="server" Text="Get Price" OnClick="btnGetPrice_Click" />
        </div>
    </form>
</body>
</html>
