<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchPage.aspx.cs" Inherits="WedInterface.SearchPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 1141px">

            <asp:CheckBox ID="CheckBox1" runat="server" Style="z-index: 1; left: 41px; top: 63px; position: absolute" Text="Страна" />
            <asp:CheckBox ID="CheckBox2" runat="server" Style="z-index: 1; left: 41px; position: absolute; top: 100px" Text="Транспорт" />
            <asp:CheckBox ID="CheckBox3" runat="server" Style="z-index: 1; left: 41px; top: 140px; position: absolute;" Text="Визовое обслуживание" />
            <asp:GridView ID="GridView1" runat="server" style="z-index: 1; left: 499px; top: 60px; position: absolute; height: 104px; width: 613px" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="GridView1_RowCommand">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
            <asp:Button ID="Button1" runat="server" style="z-index: 1; left: 286px; top: 889px; position: absolute; width: 95px; height: 25px;" Text="Найти тур" OnClick="Button1_Click" />
            <asp:Label ID="Label1" runat="server" style="z-index: 1; left: 169px; top: 25px; position: absolute" Text="Поиск туров"></asp:Label>
            <asp:CheckBox ID="CheckBox4" runat="server" Style="z-index: 1; left: 41px; top: 179px; position: absolute" Text="Проживание" />
            <asp:CheckBox ID="CheckBox5" runat="server" Style="z-index: 1; left: 41px; position: absolute; top: 224px" Text="Условия проживания" />
            <asp:CheckBox ID="CheckBox6" runat="server" Style="z-index: 1; left: 41px; top: 267px; position: absolute" Text="Стоимость" />
            <asp:CheckBox ID="CheckBox7" runat="server" Style="z-index: 1; left: 41px; top: 652px; position: absolute" Text="Дата прибытия" />
            <asp:CheckBox ID="CheckBox8" runat="server" Style="z-index: 1; left: 41px; top: 445px; position: absolute" Text="Дата отправления" />
            <asp:CheckBox ID="CheckBox9" runat="server" Style="z-index: 1; left: 41px; top: 322px; position: absolute; right: 849px" Text="Экскурсии" />
            <asp:CheckBox ID="CheckBox10" runat="server" Style="z-index: 1; left: 41px; top: 373px; position: absolute" Text="Длительность пребывания" />
            <asp:DropDownList ID="DropDownList1" runat="server" Style="z-index: 1; left: 259px; top: 63px; position: absolute; bottom: 463px; width: 123px">
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownList2" runat="server" Style="z-index: 1; left: 259px; top: 98px; position: absolute; width: 123px">
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownList4" runat="server" Style="z-index: 1; left: 259px; top: 176px; position: absolute; width: 123px">
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownList5" runat="server" Style="z-index: 1; left: 259px; top: 223px; position: absolute; width: 123px">
            </asp:DropDownList>
            <asp:RadioButton ID="RadioButton1" runat="server" Style="z-index: 1; left: 166px; top: 257px; position: absolute; bottom: 280px; width: 70px;" Text="более" GroupName="1" Checked="True" />
            <asp:RadioButton ID="RadioButton2" runat="server" Style="z-index: 1; left: 166px; top: 283px; position: absolute" Text="менее" GroupName="1" />
            <asp:RadioButton ID="RadioButton3" runat="server" Style="z-index: 1; left: 235px; top: 366px; position: absolute; bottom: 170px;" Text="более" GroupName="2" Checked="True" />
            <asp:RadioButton ID="RadioButton4" runat="server" Style="z-index: 1; left: 235px; top: 392px; position: absolute" Text="менее" GroupName="2" />
            <asp:TextBox ID="TextBox1" runat="server" Style="z-index: 1; left: 259px; top: 273px; position: absolute; width: 123px;"></asp:TextBox>
            <asp:TextBox ID="TextBox2" runat="server" Style="z-index: 1; left: 300px; top: 377px; position: absolute; width: 89px; bottom: 98px; right: 773px;"></asp:TextBox>
            <asp:Calendar ID="Calendar1" runat="server" Style="z-index: 1; left: 200px; top: 446px; position: absolute; height: 188px; width: 180px"></asp:Calendar>
            <asp:Calendar ID="Calendar2" runat="server" Style="z-index: 1; left: 200px; top: 654px; position: absolute; height: 188px; width: 180px"></asp:Calendar>

            <asp:Label ID="Label2" runat="server" style="z-index: 1; left: 41px; top: 993px; position: absolute" Text="Клиент"></asp:Label>
            <asp:Button ID="Button2" runat="server" style="z-index: 1; left: 286px; top: 1050px; position: absolute; width: 95px" Text="Продать" OnClick="Button2_Click" />
            <asp:DropDownList ID="DropDownList6" runat="server" style="z-index: 1; left: 144px; top: 990px; position: absolute; width: 225px">
            </asp:DropDownList>

            <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" style="z-index: 1; left: 48px; top: 1059px; position: absolute; width: 72px; height: 24px;" Text="Выход" />

        </div>
    </form>
</body>
</html>
