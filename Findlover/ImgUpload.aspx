<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImgUpload.aspx.cs" Inherits="Findlover.ImgUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:FileUpload ID="FileUpload1" runat="server" />
&nbsp;
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" style="height: 21px" Text="上传" />
&nbsp;
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="确定" />
        <br />
        <asp:Image ID="Image1" runat="server" Height="200px" Width="200px" />
    </form>
</body>
</html>
