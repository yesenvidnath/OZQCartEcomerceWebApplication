<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ozq.WebForm1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="hero-section">

        <!-- Hero Section -->
        <div class="container-fluid">

            <div class="banner-container-hero-section">

                <img src="img/main-page-hero-img.png" alt="Home Page Image" />

                <div class="container serch-bar-section-wrapper">
                    <!-- Search Section -->
                    <div class="row">
                        <div class="col-10">
                            <asp:TextBox runat="server" ID="productNameSearch" CssClass="form-control" placeholder="Search Now 🔎"></asp:TextBox>
                        </div>
                        <div class="col-2">
                            <asp:Button runat="server" ID="searchProductByNameButton" CssClass="btn btn-primary" Text="Search" OnClick="SearchProductByName" />
                        </div>
                    </div>
                    <ul id="productList" runat="server" class="list-group mt-4">
                    </ul>
                </div>

            </div>
        </div>

    </div>

    <div class="container-fluid">


        <br />

        <div class="row flex flex-wrap justify-content-center product-grid-main-wrap">

            <asp:Repeater ID="ProductRepeater" runat="server" OnItemCommand="AddToCartButton_Command">

                <ItemTemplate>

                    <div class="col-md-3">

                        <div class="item <%# GetActiveImgClass(Container.ItemIndex) %>">
                            <div class="card">
                                <img src='<%# Eval("ProductImage") %>' alt='<%# Eval("ProductName") %>' class="card-img-top" onerror="this.src='img/sample-product-image.png'" width="50%" height="auto" />


                                <div class="card-body">

                                    <h5 class="card-title"><%# Eval("ProductName") %></h5>
                                    <p class="card-text card-disc"><%# Eval("Description") %></p>
                                    <p class="card-text">Price: $<%# Eval("Price") %></p>
                                    <p class="card-text">Stock: <%# Eval("Stock") %></p>

                                    <div class="row">
                                        <div class="col-6">
                                            <% if (IsLoggedIn())
                                                { %>
                                            <asp:Button class="btn-custom-btn-secondary" runat="server" Text="Add to Cart" CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>' />
                                            <% }
                                                else
                                                { %>
                                            <asp:Button class="btn-custom-btn-secondary" runat="server" Text="Log in" CommandName="AddToCart" CommandArgument='' />
                                            <% } %>
                                        </div>
                                        <div class="col-6">
                                            <a href='productdetails.aspx?productId=<%# Eval("ProductID") %>' class="btn btn-primary">Buy Now</a>
                                        </div>
                                    </div>



                                </div>
                            </div>
                        </div>

                    </div>
                </ItemTemplate>

            </asp:Repeater>
        </div>

    </div>

</asp:Content>
