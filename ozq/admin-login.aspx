<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="admin-login.aspx.cs" Inherits="ozq.WebForm3" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid admin-login-main-panel-main">
        <div class="row justify-content-center">
            <div class="col-md-6 mt-5">

                <div class="admin-login-main-frm-wrap">
                    <center>
                        <i class="fas fa-user"></i>
                    </center>
                    
                    <h2 class="text-center">Admin Login</h2>

                    <asp:Panel ID="LoginPanel" runat="server">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="email" Text="Email:" CssClass="control-label" />
                            <asp:TextBox runat="server" ID="email" CssClass="form-control" placeholder="Enter email" />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="password" Text="Password:" CssClass="control-label" />
                            <asp:TextBox runat="server" ID="password" CssClass="form-control" TextMode="Password" placeholder="Enter password" />
                        </div>

                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-6">
                                    <a href="index.aspx" class="back-home"> Back To Home </a>
                                </div>
                                <div class="col-6">
                                     <asp:Button runat="server" ID="loginButton" Text="Login " CssClass="login" OnClick="LoginButton_Click" />
                                </div>
                            </div>
                        </div>

                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <style>
        nav {
            display: none;
        }
        footer{
            display: none;
        }
    </style>

</asp:Content>
