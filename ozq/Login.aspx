<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ozq.Login" %>

<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid login-page-main-wrapper">

        <div class="row">

            <div class="col-12 left-frm-section-main">

                <div class="log-form-custom-bg-wrapper-main">
                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                        <li class="nav-item" role="presentation">
                            <a class="nav-link active" id="login-tab" data-toggle="tab" href="#login" role="tab" aria-controls="login" aria-selected="true">Login</a>
                        </li>
                        <li class="nav-item" role="presentation">
                            <a class="nav-link" id="signup-tab" data-toggle="tab" href="#signup" role="tab" aria-controls="signup" aria-selected="false">Sign Up</a>
                        </li>
                    </ul>

                    <div class="tab-content" id="myTabContent">
                        <div class="tab-pane fade show active" id="login" role="tabpanel" aria-labelledby="login-tab">
                            <div class="container-fluid">
                                <h2>Login Now</h2>
                                <asp:TextBox ID="txtLoginEmail" runat="server" placeholder="Email" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" placeholder="Password" CssClass="form-control"></asp:TextBox>
                                <div class="g-recaptcha-custom-wrapper">
                                    <div class="g-recaptcha" data-sitekey="6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI"></div>
                                </div>
                                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="Login_Click" CssClass="loginlogout" />
                                <asp:Label ID="lblResult" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="tab-pane fade" id="signup" role="tabpanel" aria-labelledby="signup-tab">
                            <div class="container-fluid">
                                <h2>Sign Up Now</h2>
                                <asp:TextBox ID="txtSignupFirstName" runat="server" placeholder="Your First Name" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtSignupLastName" runat="server" placeholder="Your Last Name" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtSignupEmail" runat="server" placeholder="Your Email" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtSignupAddress" runat="server" placeholder="Your Address" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtSignupPassword" runat="server" TextMode="Password" placeholder="Password" CssClass="form-control"></asp:TextBox>

                                <div class="g-recaptcha-custom-wrapper">
                                    <div class="g-recaptcha" data-sitekey="6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI"></div>
                                </div>
                                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" OnClick="Signup_Click" CssClass="loginlogout" />

                                <br />


                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>


    </div>

</asp:Content>

