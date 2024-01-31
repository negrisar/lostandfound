<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.View.Login" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>
    <title>Login</title>

    <link rel="stylesheet" type="text/css" href="LoginStyle.css" />
    <style type="text/css">
        .auto-style3 {
            margin-left: 0px;
            margin-top: 0;
        }

        .auto-style4 {
            margin-left: 0px;
        }
    </style>

    <script type="text/javascript">
        window.history.forward();
        function noBack() {
            window.history.forward();
        }
    </script>

</head>
<body onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="">
    <form id="form1" runat="server">
        <div class="container">
            <div class="divan_logo">
                <img src="https://classification.divan.com.tr/divanlogo.jpg" />
            </div>
            <div class="login" style="min-width: 325px">
                <h2>Kayıp Eşya Kayıt Sistemi</h2>

                <div class="form-group">
                    <div>
                        <div class="user">
                            <asp:Label ID="Label1" runat="server"  Text="Kullanıcı Adı: " Width="120px"></asp:Label>
                            <div class="input-class">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged" CssClass="form-control auto-style2" Width="162px" Height="30px" style="margin-left: 0"></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="Kullanıcı Adı Giriniz" ControlToValidate="TextBox1"></asp:RequiredFieldValidator>
                        </div>
                        <div class="password">
                            <asp:Label ID="Label2" runat="server" Text="Kullanıcı Şifresi: "></asp:Label>
                            <div class="input-class">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                <asp:TextBox ID="TextBox2" runat="server" OnTextChanged="TextBox1_TextChanged" TextMode="Password" CssClass="form-control auto-style3" Width="162px" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Şifre Giriniz" ControlToValidate="TextBox2"></asp:RequiredFieldValidator>

                        <div class="security">
                            <asp:Label ID="lbl_securitycode" runat="server" Text="Güvenlik Kodu:" CssClass="control-label" Font-Bold="False" meta:resourcekey="lbl_securitycodeResource1"></asp:Label>
                            <div class="input-class">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                <asp:TextBox ID="txtCaptcha" runat="server" meta:resourcekey="txtCaptchaResource1" OnTextChanged="txtCaptcha_TextChanged" CssClass="form-control auto-style4" Width="162px" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Güvenlik Kodunu Giriniz" ControlToValidate="txtCaptcha"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">

                    <asp:Label ID="Label3" runat="server" CssClass="col-md-2 control-label" meta:resourcekey="Label1Resource1"></asp:Label>
                    <div>
                        <cc1:CaptchaControl ID="Captcha1" runat="server"
                            CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                            ontColor="210, 11, 12" NoiseColor="177, 177, 177" BackColor="White" CaptchaChars="ACDEFGHJKLNPQRTUVXYZ2346789" LineColor="Black" meta:resourcekey="Captcha1Resource1" CssClass="auto-style4" Width="206px" />
                        <div>
                            <br />
                            <asp:Button ID="Button1" runat="server" Text="Giriş" OnClick="Button1_Click" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
﻿>

﻿