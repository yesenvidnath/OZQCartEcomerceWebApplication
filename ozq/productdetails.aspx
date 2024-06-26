<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="productdetails.aspx.cs" Inherits="ozq.WebForm2" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid single-prod-wrap-main">

        <div class="row">

            <asp:Repeater ID="ProductRepeater" runat="server" OnItemCommand="AddToCartButton_Command_Product_page">

                <ItemTemplate>

                    <div class="col-5">

                        <img src='<%# Eval("ProductImage") %>' alt='<%# Eval("ProductName") %>' onerror="this.src='img/sample-product-image.png'" width="50%" height="auto" />

                    </div>

                    <div class="col-7">

                        <div class="description">
                            <h3><%# Eval("ProductName") %></h3>
                            <p><%# Eval("Description") %></p>
                            <p>Price: $<%# Eval("Price") %></p>
                            <p>Stock: <%# Eval("Stock") %></p>

                            <div class="parent-fam">

                                <a href='<%# $"payment.aspx?productId={Eval("ProductID")}" %>' class="btn btn-primary">Pay now</a>

                                <asp:Button class="btn btn-secondary" runat="server" Text="Add to Cart" CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>' />
                            </div>
                        </div>


                        <br />


                        <h3>Feedbacks</h3>

                        <div class="comment-section">
                            <div class="item-repater">
                                <asp:Repeater ID="CommentsRepeater" runat="server">
                                    <ItemTemplate>
                                        <div class="comment-item">
                                            <p><%# Eval("CustomerName") %> says: <span class="comment"><%# Eval("CommentText") %></span></p>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="add-comment">
                                <asp:TextBox class="commentaddflied" ID="CommentTextBox" runat="server" placeholder="Give Us Your Feedback"></asp:TextBox>
                                <asp:Button class="btn btn-primary" ID="AddCommentButton" runat="server" Text="Give Us Your Feedback" OnClick="AddCommentButton_Click" />
                            </div>
                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </div>

</asp:Content>
