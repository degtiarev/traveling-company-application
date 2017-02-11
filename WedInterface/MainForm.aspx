<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="WedInterface.MainForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="height: 480px">
    <form id="form1" runat="server">
    <div style="height: 478px">
    
        <asp:Label ID="Label4" runat="server" style="z-index: 1; left: 310px; top: 231px; position: absolute" Text="Пароль"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" style="z-index: 1; left: 424px; top: 193px; position: absolute">artur</asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server" style="z-index: 1; left: 424px; top: 232px; position: absolute" TextMode="Password">artur</asp:TextBox>
    
    </div>
        <asp:Label ID="Label1" runat="server" style="z-index: 1; left: 367px; top: 135px; position: absolute" Text="Авторизация"></asp:Label>
        <asp:Label ID="Label3" runat="server" style="z-index: 1; left: 314px; top: 194px; position: absolute" Text="Логин"></asp:Label>
        <asp:Label ID="Label2" runat="server" style="z-index: 1; left: 373px; top: 37px; position: absolute" Text="Туристическая фирма"></asp:Label>
        <asp:Button ID="Button1" runat="server" style="z-index: 1; left: 485px; top: 283px; position: absolute" Text="Войти" OnClick="Button1_Click" />
    </form>
</body>
</html>
