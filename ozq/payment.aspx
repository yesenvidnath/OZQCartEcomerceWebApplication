<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="ozq.WebForm4" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid single-payment-wrap-main">

        <div class="row">

            <!-- Column 1: Display Items -->
            <div class="col-md-6 product-list-side">

                <div class="payment-product-list">

                    <!-- Display items from the cart -->
                    <asp:Repeater ID="CartItemsRepeater" runat="server">

                        <ItemTemplate>

                            <div class="cart-item">

                                <div class="row">
                                    <!-- Column 1: Product Image and Details -->
                                    <div class="col-8">
                                        <div class="row">
                                            <div class="col-4">
                                                <img src='<%# Eval("ProductImage") %>' alt='<%# Eval("ProductName") %>' class="card-img-top" onerror="this.src='img/sample-product-image.png'" width="100%" height="auto" />
                                            </div>
                                            <div class="col-8">
                                                <b>
                                                    <p><%# Eval("ProductName") %></p>
                                                </b>
                                                <h6>Price</h6>
                                                <p>$<%# Eval("Price") %></p>
                                                <p>Quantity: <%# Eval("TotalQuantity") %></p>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Column 2: Reduce and Remove Buttons -->
                                    <div class="col-4">

                                        <asp:Button ID="btnReduceItem" runat="server" Text="Reduce" CssClass="btn btn-warning" OnClick="ReduceItem_Click" CommandArgument='<%# Eval("ProductID") %>' />

                                        <asp:Button ID="btnRemoveItem" runat="server" Text="Remove" CssClass="btn btn-secondary" OnClick="RemoveItem_Click" CommandArgument='<%# Eval("ProductID") %>' />
                                    </div>
                                </div>

                            </div>

                        </ItemTemplate>

                    </asp:Repeater>

                    <!-- Display the success message after the Ordering -->
                    <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label>

                </div>

            </div>

            <!-- Column 2: Payment Options -->
            <div class="col-md-6 order-form-column">
                <div class="container mt-5">
                    <div class="row justify-content-center">

                        <asp:Panel ID="PaymentPanel" runat="server">

                            <div class="form-wrapper-custom">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-8">

                                            <div class="form-group">
                                                <label for="Address">Address</label>
                                                <asp:TextBox ID="Address" CssClass="form-control" runat="server" placeholder="Address"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-4">
                                            <div class="form-group">
                                                <label for="ZipCode">ZipCode</label>
                                                <asp:TextBox ID="ZipCode" CssClass="form-control" runat="server" placeholder="Zip Code"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="form-group">
                                                <label for="PostalCode">Postal Code</label>
                                                <asp:TextBox ID="PostalCode" CssClass="form-control" runat="server" placeholder="Postal Code"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <div class="form-group">
                                                <label for="TelephoneNo">Telephone No</label>
                                                <asp:TextBox ID="TelephoneNo" CssClass="form-control" runat="server" placeholder="Telephone No"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="CardNumber">Card Number</label>
                                    <asp:TextBox ID="CardNumber" CssClass="form-control" runat="server" placeholder="Card Number"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label for="ExpiryDate">Expiry Date</label>
                                    <asp:TextBox ID="ExpiryDate" CssClass="form-control" runat="server" placeholder="MM/YY"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label for="CVV">CVV</label>
                                    <asp:TextBox ID="CVV" CssClass="form-control" runat="server" placeholder="CVV"></asp:TextBox>
                                </div>

                                 <br />
                                <h4 class="">Total Cost: $ <asp:Label CssClass="cost-tot-display-main" ID="TotalCostLabel" runat="server" Text=" 0" /></h4>
                                <div class="container-fluid">
                                    
                                    <div class="row">
                                        
                                        <div class="col-md-6">
                                            <div class="form-group btn-frm-grp-wrap">
                                                <asp:Button ID="SubmitPaymentButton" runat="server" Text="Pay Now 💵" CssClass="btn-removes" OnClick="SubmitPaymentButton_Click" />
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group btn-frm-grp-wrap">
                                                <asp:Button ID="PlaceOrderButton" runat="server" Text="Cash On Dilivery 🚛" CssClass="pay-now-customs" OnClick="PlaceOrderButton_Click" />
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>

                           
                        </asp:Panel>

                    </div>
                </div>

                

            </div>

        </div>

    </div>

</asp:Content>
