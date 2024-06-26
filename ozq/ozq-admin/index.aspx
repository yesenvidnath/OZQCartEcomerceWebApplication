<%@ Page Title="" Language="C#" MasterPageFile="~/ozq-admin/admin.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ozq.ozq_admin.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid admin-panel-main-styles">

        <div class="row">

            <div class="col-2 admin-panel-left-side-wraper">


                <div class="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">

                    <button class="nav-link  active" id="v-pills-profile-tab" data-bs-toggle="pill" data-bs-target="#v-pills-profile" type="button" role="tab" aria-controls="v-pills-profile" aria-selected="false"><i class="fa-regular fa-file"></i>Manage Orders <span id="totalOrdersLabel" runat="server"></span></button>

                    <button class="nav-link" id="v-pills-Products-tab" data-bs-toggle="pill" data-bs-target="#v-pills-Products" type="button" role="tab" aria-controls="v-pills-Products" aria-selected="false"><i class="fa-solid fa-box"></i>Mange Products <span id="totalCustomersLabel" runat="server"></span></button>

                    <button class="nav-link" id="v-pills-Customer-tab" data-bs-toggle="pill" data-bs-target="#v-pills-Customer" type="button" role="tab" aria-controls="v-pills-Customer" aria-selected="false"><i class="fa-regular fa-user"></i>Manage Customers <span id="totalProductsLabel" runat="server"></span></button>

                    <button class="nav-link" id="v-pills-Admins-tab" data-bs-toggle="pill" data-bs-target="#v-pills-Admins" type="button" role="tab" aria-controls="v-pills-Admins" aria-selected="false"><i class="fa-regular fa-user"></i>Admins</button>

                    <asp:LinkButton class="nav-link" ID="LogoutButton" runat="server" OnClick="Logout_Click" Text="Logout"> <i class="fa-solid fa-power-off"></i> Logout</asp:LinkButton>

                </div>

            </div>

            <div class="col-10 admin-panel-content-side-wrapper">

                <div class="tab-content" id="v-pills-tabContent">


                    <!-- Manage Orders Tab -->
                    <div class="tab-pane fade show active order-tab-main-wrapper-top" id="v-pills-profile" role="tabpanel" aria-labelledby="v-pills-profile-tab">

                        <h3>Manage All Orders </h3>
                        <br />

                        <asp:GridView ID="ordersGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped">
                            <Columns>
                                <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                                <asp:BoundField DataField="FirstName" HeaderText="Customer Name" />
                                <asp:BoundField DataField="OrderType" HeaderText="Order Type" />
                                <asp:TemplateField HeaderText="Products">
                                    <ItemTemplate>
                                        <asp:Label CssClass="items-custom-class-miain" ID="productsLabel" runat="server" Text='<%# Bind("Products") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Change Order Type">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="orderTypeDropdown" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="Paid order">Paid Order</asp:ListItem>
                                            <asp:ListItem Value="Cash on Order">Cash on Order</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="changeOrderTypeButton" runat="server" Text="Change Order Type" OnClick="ChangeOrderType_Click" CommandArgument='<%# Eval("OrderID") %>' CssClass="btn btn-primary mt-2" />
                                        
                                        <asp:Button ID="deleteButton" runat="server" Text="Delete Now" OnClick="DeleteOrder_Click" CommandArgument='<%# Eval("OrderID") %>' CssClass="btn btn-danger mt-2" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>



                    <!-- Manage Products Tab -->
                    <div class="tab-pane fade" id="v-pills-Products" role="tabpanel" aria-labelledby="v-pills-Products-tab">

                        <h3>Manage All Products </h3>
                        <br />

                        <div class="row">

                            <div class="col-4">

                                <div class="container">

                                    <div class="form-group">
                                        <label for="productId">Product ID (for search)</label>
                                        <asp:TextBox runat="server" ID="productId" CssClass="form-control" placeholder="Enter Product ID"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productName">Product Name</label>
                                        <asp:TextBox runat="server" ID="productNames" CssClass="form-control" placeholder="Enter Product Name"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productName">Product Price</label>
                                        <asp:TextBox runat="server" ID="productPrice" CssClass="form-control" placeholder="Enter Product Price"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productName">Product Stock</label>
                                        <asp:TextBox runat="server" ID="productStock" CssClass="form-control" placeholder="Enter Product Stock"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productDescription">Product Description</label>
                                        <asp:TextBox runat="server" ID="productDescriptions" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Product Description"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productImage">Product Image</label>
                                        <asp:FileUpload ID="productImage" runat="server" CssClass="form-control-file" />
                                    </div>


                                    <div class="buttongroupmain">
                                        <asp:Button runat="server" ID="insertProductButton" CssClass="btn btn-primary" Text="Insert Product" OnClick="InsertProduct" />
                                        <asp:Button runat="server" ID="UpdateProductButton" CssClass="btn btn-info ml-2" Text="Update Product" OnClick="UpdateProduct" />
                                        <asp:Button runat="server" ID="SearchProductButton" CssClass="btn btn-info" Text="Search Product" OnClick="SearchProduct" />
                                    </div>

                                </div>

                            </div>


                            <div class="col-8">

                                <asp:GridView ID="productsGridView" runat="server" AutoGenerateColumns="false" OnRowCommand="ProductsGridView_RowCommand" DataKeyNames="ProductID" CssClass="table table-bordered table-striped">
                                    <Columns>
                                        <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:BoundField DataField="Price" HeaderText="Price" />
                                        <asp:BoundField DataField="Stock" HeaderText="Stock" />
                                        <asp:TemplateField HeaderText="Product Image">
                                            <ItemTemplate>
                                                <asp:Image ID="productImage" runat="server" ImageUrl='<%# "" + Eval("ProductImage") %>' Height="90" Width="100" onerror="this.src='../img/sample-product-image.png'" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:Button runat="server" Text="Delete Now" CommandName="DeleteProduct" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this product?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>


                    <!-- Content for the Manage Customers tab -->
                    <div class="tab-pane fade" id="v-pills-Customer" role="tabpanel" aria-labelledby="v-pills-Customer-tab">


                        <h3>Manage All Customers </h3>
                        <br />

                        <asp:GridView ID="customersGridView" runat="server" AutoGenerateColumns="false" OnRowCommand="CustomersGridView_RowCommand" DataKeyNames="CustomerID" CssClass="table table-bordered table-striped">
                            <Columns>
                                <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" />
                                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Password" HeaderText="Password" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                <asp:ButtonField ButtonType="Button" Text="Delete Now" CommandName="DeleteCustomer" HeaderText="Action" />
                            </Columns>
                        </asp:GridView>

                    </div>


                    <!-- Content for the Admins tab -->
                    <div class="tab-pane fade" id="v-pills-Admins" role="tabpanel" aria-labelledby="v-pills-Admins-tab">

                        <h3>Manage Admins</h3>
                        <br />

                        <div class="row">

                            <div class="col-4">

                                <div class="container">

                                    <div class="form-group">
                                        <label for="productId">Admin ID (for search)</label>
                                        <asp:TextBox runat="server" ID="AdminID" CssClass="form-control" placeholder="Enter ID"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productName">First Name</label>
                                        <asp:TextBox runat="server" ID="FirstName" CssClass="form-control" placeholder="Enter First Name"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productName">Last Name</label>
                                        <asp:TextBox runat="server" ID="LastName" CssClass="form-control" placeholder="Enter Last Name"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productName">Email</label>
                                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" placeholder="Enter Enter"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="productDescription">Password</label>
                                        <asp:TextBox runat="server" ID="Password" CssClass="form-control" placeholder="Enter Password"></asp:TextBox>
                                    </div>


                                    <div class="buttongroupmain">
                                        <asp:Button runat="server" ID="Button1" CssClass="btn btn-primary" Text="Insert Admin" OnClick="InsertAdmin" />
                                        <asp:Button runat="server" ID="Button2" CssClass="btn btn-info ml-2" Text="Update Admin" OnClick="UpdateAdmin" />
                                        <asp:Button runat="server" ID="Button3" CssClass="btn btn-info" Text="Search Admin" OnClick="SearchAdmin" />
                                    </div>

                                </div>

                            </div>


                            <div class="col-8">
                                <<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowCommand="AdminGridView_RowCommand" DataKeyNames="AdminID" CssClass="table table-bordered table-striped">

                                    <Columns>
                                        <asp:BoundField DataField="AdminID" HeaderText="Admin ID" />
                                        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                        <asp:BoundField DataField="Password" HeaderText="Password" />
                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:Button runat="server" Text="Remove Admin" CommandName="DeleteAdmin" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this Admin?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
