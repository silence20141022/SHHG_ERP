<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NLogin.aspx.cs" Inherits="Aim.Examining.Web.NLogin"
    Title="上海宏谷ERP系统" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
        body .top-bar
        {
            border-bottom: 1px solid #ccc; /*  margin-bottom: 50px;
          line-height: 60px; padding: 10px 20px; */
            height: 100px;
            background-color: #191970;
        }
        body .top-bar div
        {
            padding-top: 2%;
            color: white;
            margin-left: 5%;
        }
        body .bottom-bar
        {
            border-top: 1px solid #ccc;
            text-align: center;
            position: absolute;
            bottom: 0px;
            width: 100%;
            font-size: 13px;
        }
        
        body .bottom-bar span
        {
            padding-left: 10px;
            color: gray;
            font-weight: bold;
            font-size: 12px;
        }
        .lbl-message
        {
            color: Red;
        }
    </style>
    <link href="font-awesome41/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap32/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script src="/js/lib/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var islogining = false;

                function onPgLoad() {
                    $(document).bind("keydown", function (e) {
                        // 回车
                        if (e.keyCode == 13 && !islogining) {
                            DoLogin();
                        }
                    });

                    getCookie();    // 获取Cookie

                    if (!$("#uname").val()) {
                        $("#uname").focus();
                    } else if (!$("#pwd").val()) {
                        $("#pwd").focus();
                    } else {
                        $("#imgDoLogin").focus();
                    }
                }

        function DoLogin() {
            //            if (islogining) {
            //                return;
            //            }

            // setLoginStatus(true);

            if (!$("#uname").val()) {
                $("#message").text("提示：请输入用户名。");
                $("#uname").focus();

                //setLoginStatus(false);
            }
            //             else {
            //                $("#message").text("");
            //            }

            // setCookie();    // 设置Cookie

            //            $("form").ajaxSubmit({ data: { 'reqaction': 'login', 'asyncreq': true },
            //                success: function (resp) {
            //                    if (resp) {
            //                        if (resp.indexOf("success") == 0) {
            //                            var redurl = resp.substr("success".length + 1);
            //                            location.href = redurl;
            //                        } else {
            //                            $("#message").html("提示：" + resp);
            //                        }
            //                    }
            //                   // setLoginStatus(false);
            //                }, error: function (resp) {
            //                    alert(resp);
            //                }
            //            });
            $.post("NLogin.aspx?id=" + Math.random(), { action: "login", uname: $("#uname").val(), pwd: $("#pwd").val() }, function () {
                alert("123s");
                window.location.href = "Default.aspx";
            })
        }

        function OpenPwdChgPage() {
            rtn = OpenWin("/Modules/SysApp/OrgMag/UsrChgPwd.aspx", "_blank", CenterWin("width=350,height=180,scrollbars=yes"));
        }

        function setLoginStatus(flag) {
            if (flag) {
                islogining = true;
                $("input").attr("disabled", true);
                $("#imgDoLogin").attr("disabled", true);

                // $("#span-loading").css("display", ""); // 显示进度条
            } else {
                islogining = false;
                $("input").attr("disabled", false);
                $("#imgDoLogin").attr("disabled", false);
                // $("#span-loading").css("display", "none"); // 隐藏进度条
            }
        }

        function setCookie() {
            var isSaveAccount = $("#saveAcount").attr("checked");
            var isSavePassword = $("#savePassword").attr("checked");

            if (isSaveAccount) {
                $.cookie("uname", $("#uname").val());
                $.cookie("saveAcount", isSaveAccount);
            } else {
                $.cookie("uname", null, { expires: 300 });
                $.cookie("saveAcount", null, { expires: 300 });
            }

            if (isSavePassword) {
                $.cookie("pwd", $("#pwd").val());
                $.cookie("savePassword", isSavePassword);
            } else {
                $.cookie("pwd", null, { expires: 300 });
                $.cookie("savePassword", null, { expires: 300 });
            }
        }

        function getCookie() {
            var isSaveAccount = $.cookie("saveAcount");
            var isSavePassword = $.cookie("savePassword");

            if (isSaveAccount) {
                $("#saveAcount").attr("checked", true);
                $("#uname").val($.cookie("uname"));
            }

            if (isSavePassword) {
                $("#savePassword").attr("checked", true);
                $("#pwd").val($.cookie("pwd"));
            }
        }
        
    </script>
</head>
<body>
    <div class="  panel-default ">
        <div class=" top-bar">
            <div>
                <i class="fa fa-th fa-2x ">上海宏谷进出口贸易有限公司</i>
            </div>
        </div>
        <form method="post">
        <div class=" col-md-12" style=" background-color: ; position: absolute;
            bottom:20px; top: 100px">
            <div style="margin-top: 8%; margin-left: 30%">
                <div class="" style="width: 400px; border: 2px solid #ddd; padding:20px;background-color:White;">
                    <strong class="">ERP</strong>
                    <div class="input-group input-group-lg ">
                        <i class="fa fa-user fa-2x input-group-addon"></i>
                        <input id="uname" name="uname" class="form-control" value="admin" placeholder="用户名" />
                    </div>
                    <div class="input-group  input-group-lg" style="margin-top: 15px; margin-bottom: 15px">
                        <i class="fa fa-lock fa-2x input-group-addon"></i>
                        <input id="pwd" name="pwd" type="password" class="form-control" value="hg2012" placeholder="密码" />
                    </div>
                    <button id="imgDoLogin" class="btn btn-md btn-primary btn-block" onclick="DoLogin();">
                        登 录</button>
                    <div>
                        <label class="lbl-message" style="margin-top: 10px;" id="message" name="message">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        </form>
    </div>
    <div class="bottom-bar">
        上海宏谷ERP系统V1.2 All Rights Reserved © 2012-2016. ShangHai HongGu ERP System
    </div>
</body>
</html>
