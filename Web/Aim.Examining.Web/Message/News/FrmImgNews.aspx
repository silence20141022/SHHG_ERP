<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmImgNews.aspx.cs" Inherits="Aim.Examining.Web.FrmImgNews" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>图片新闻</title>
    <link href="roll/reset.css" rel="stylesheet" type="text/css" />
    <link href="roll/webmain.css" rel="stylesheet" type="text/css" />
    <link href="roll/ddsmoothmenu.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="roll/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="roll/jquery.KinSlideshow-1.2.1.js"></script>

    <script type="text/javascript" src="roll/webtry_roll.js"></script>

    <script type="text/javascript" src="roll/ddsmoothmenu.js"></script>

</head>

<script language="javascript" type="text/javascript">
    $(function() {
        $("#banner").KinSlideshow({
            moveStyle: "right",
            titleBar: { titleBar_height: 30, titleBar_bgColor: "#000", titleBar_alpha: 0.7 },
            titleFont: { TitleFont_size: 12, TitleFont_color: "#FFFFFF", TitleFont_weight: "normal" },
            btn: { btn_bgColor: "#2d2d2d", btn_bgHoverColor: "#1072aa", btn_fontColor: "#FFF", btn_fontHoverColor: "#FFF", btn_borderColor: "#4a4a4a", btn_borderHoverColor: "#1188c0", btn_borderWidth: 1 }
        });
    })
</script>

<body>
    <form id="form1" runat="server">
    <div id="banner">
        <asp:Literal ID="litimg" runat="server" />
        <a href='#'>
            <img runat="server" id="imglast" width="350" height="200" /></a>
    </div>
    </form>
</body>
</html>
