<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="ozq.profile" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="container-fluid single-custo-probile-wrap-main">

        <div class="row">

            <div class="d-flex align-items-start">

                <div class="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">

                    <button class="nav-link active" id="v-pills-home-tab" data-bs-toggle="pill" data-bs-target="#v-pills-home" type="button" role="tab" aria-controls="v-pills-home" aria-selected="true"><i class="fas fa-user"></i>Profile</button>

                    <button class="nav-link" id="v-pills-Comment-tab" data-bs-toggle="pill" data-bs-target="#v-pills-Comment" type="button" role="tab" aria-controls="v-pills-Comment" aria-selected="false"><i class="fa-solid fa-box"></i>Orders</button>

                    <button class="nav-link" id="v-pills-profile-tab" data-bs-toggle="pill" data-bs-target="#v-pills-profile" type="button" role="tab" aria-controls="v-pills-profile" aria-selected="false"><i class="fas fa-gear"></i>Settings</button>
                    
                    <a class="nav-link" href="index.aspx"><i class="fas fa-home"></i> Home </a>


                </div>

                <div class="tab-content" id="v-pills-tabContent">

                    <div class="tab-pane fade show active" id="v-pills-home" role="tabpanel" aria-labelledby="v-pills-home-tab">

                        <div class="container">
                            <div class="row">
                                <div class="col-6">
                                    <h3>My Profile</h3>
                                    <br />
                                    <br />
                                    <asp:Repeater ID="ProductRepeater" runat="server">
                                        <ItemTemplate>

                                            <h6>Customer ID</h6>
                                            <p><%# Eval("CustomerID") %></p>
                                            <br>
                                            <h6>First Name</h6>
                                            <p><%# Eval("FirstName") %></p>
                                            <br>
                                            <h6>Last Name</h6>
                                            <p><%# Eval("LastName") %></p>
                                            <br>
                                            <h6>Email</h6>
                                            <p><%# Eval("Email") %></p>
                                            <br>
                                            <h6>Password</h6>
                                            <p><%# Eval("Password") %></p>
                                            <br>
                                            <h6>Address</h6>
                                            <p><%# Eval("Address") %></p>

                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>



                                <div class="col-6">
                                    <h3>Shopping Cart</h3>
                                    <br />
                                    <br />

                                    <div id="" class="cart-list-on-profile">
                                        <asp:Repeater ID="CartitemsRepeater" runat="server">
                                            <ItemTemplate>
                                                <div class="cart-item">
                                                    <div class="row">

                                                        <div class="col-4">
                                                            <img src='<%# Eval("ProductImage") %>' alt='<%# Eval("ProductName") %>' class="card-img-top" onerror="this.src='img/sample-product-image.png'" width="50%" height="auto" />
                                                        </div>

                                                        <div class="col-8">

                                                            <p><%# Eval("ProductName") %></p>

                                                            <h6>Price</h6>
                                                            <p><%# Eval("Price") %></p>

                                                            <p>Total Quantity: <%# Eval("TotalQuantity") %></p>
                                                            <br />
                                                            <asp:Button ID="btnRemoveItem" runat="server" Text="Remove" CssClass="btn-remove" OnClick="RemoveItem_Click" CommandArgument='<%# Eval("ProductID") %>' />

                                                        </div>

                                                    </div>


                                                </div>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <center>
                                            <a id="PaymentLink" href="payment.aspx" runat="server" class="pay-now-custom">Go to Checkout</a>
                                        </center>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>


                    <div class="tab-pane fade" id="v-pills-Comment" role="tabpanel" aria-labelledby="v-pills-Comment-tab">
                        <div class="container">
                            <div class="row">
                                <div class="col"></div>
                                <div class="col-5">
                                    <h3>My Orders</h3>
                                    <br />
                                    <br />
                                    <div id="" class="cart-list-on-profile">
                                        <asp:Repeater ID="OrderRepeater" runat="server">
                                            <ItemTemplate>

                                                <div class="Order-Item">
                                                    <h6>Order ID</h6>
                                                    <p><%# Eval("OrderID") %></p>

                                                    <h6>Order Date</h6>
                                                    <p><%# Eval("OrderDate") %></p>

                                                    <h6>Total Amount</h6>
                                                    <p><%# Eval("TotalAmount") %></p>

                                                    <h6>Order Type</h6>
                                                    <p><%# Eval("OrderType") %></p>

                                                </div>
                                            </ItemTemplate>

                                        </asp:Repeater>
                                    </div>
                                </div>
                                <div class="col"></div>
                            </div>
                        </div>
                    </div>



                    <div class="tab-pane fade" id="v-pills-profile" role="tabpanel" aria-labelledby="v-pills-profile-tab">

                        <div class="container-fluid">
                            <div class="row">
                                <div class="col"></div>
                                <div class="col-7">

                                    <div class="card">
                                        <div class="card-body">
                                            <h2 class="card-title">Edit Profile</h2>
                                            <asp:Panel ID="EditProfilePanel" runat="server">

                                                <div class="form-group">
                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="First Name" value='<%# Eval("FirstName") %>'></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Last Name" value='<%# Eval("LastName") %>'></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" value='<%# Eval("Email") %>'></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" value='<%# Eval("Password") %>'></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Address" value='<%# Eval("Address") %>'></asp:TextBox>
                                                </div>
                                                <asp:Button ID="btnUpdateProfile" runat="server" CssClass="btn btn-primary" Text="Update Profile" OnClick="UpdateProfile_Click" />
                                            </asp:Panel>
                                        </div>
                                    </div>



                                </div>

                                <div class="col"></div>
                            </div>
                        </div>

                    </div>


                </div>

            </div>

        </div>
    </div>

    <style>
        .main-nav-bar-wrapper-top-wrapper {
            display: none;
        }

        footer {
            display: none;
        }
    </style>


</asp:Content>
