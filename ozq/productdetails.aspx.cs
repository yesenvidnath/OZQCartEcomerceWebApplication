using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ozq
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ProductID"] != null)
                {
                    string productId = Request.QueryString["ProductID"];

                    string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                    string query = "SELECT * FROM Products WHERE ProductID = @ProductId";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", productId);

                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();

                            // Bind the reader to the Repeater
                            ProductRepeater.DataSource = reader;
                            ProductRepeater.DataBind();

                            // Display previous comments
                            DisplayPreviousComments(Convert.ToInt32(productId));
                        }
                    }
                }
            }

        }

        // Added to Cart Funtion 
        protected void AddToCartButton_Command_Product_page(object sender, CommandEventArgs e)
        {
            if (Request.Cookies["UserId"] != null && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);
                int productId = Convert.ToInt32(e.CommandArgument);
                int quantity = 1;  // You can adjust this as needed

                AddToCartFuntion(userId, productId, quantity);
            }
        }


        private void AddToCartFuntion(int customerId, int productId, int quantity)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "INSERT INTO [Ozq_g_db].[dbo].[Cart] ([CustomerID], [ProductID], [Quantity]) VALUES (@CustomerId, @ProductId, @Quantity)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                }
            }
            // Redirect back to the index page
            Response.Redirect($"productdetails.aspx?productId={productId}");
        }


        private void DisplayPreviousComments(int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = @"
            SELECT C.CommentText, U.FirstName AS CustomerName
            FROM Comment C
            INNER JOIN Customers U ON C.CustomerID = U.CustomerID
            WHERE C.ProductID = @ProductId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();  // Open the connection here

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Access the CommentsRepeater using FindControl to set its data source
                    Repeater CommentsRepeater = (Repeater)ProductRepeater.Items[0].FindControl("CommentsRepeater");

                    // Bind the reader to the Repeater
                    CommentsRepeater.DataSource = reader;
                    CommentsRepeater.DataBind();
                }
            }
        }

        private void AddNewComment(int productId, int customerId, string commentText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "INSERT INTO Comment (ProductID, CustomerID, CommentText) VALUES (@ProductId, @CustomerId, @CommentText)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@CommentText", commentText);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void AddCommentButton_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["UserId"] != null && Request.QueryString["ProductID"] != null)
            {
                int customerId = Convert.ToInt32(Request.Cookies["UserId"].Value);
                int productId = Convert.ToInt32(Request.QueryString["ProductID"]);

                // Access the CommentTextBox using FindControl to retrieve the text
                TextBox CommentTextBox = (TextBox)ProductRepeater.Items[0].FindControl("CommentTextBox");
                string commentText = CommentTextBox.Text;

                AddNewComment(productId, customerId, commentText);

                // Refresh the comment section after adding a comment
                DisplayPreviousComments(productId);
            }
            else
            {
                // Redirect to login page or handle access denial as needed
                Response.Redirect("Login.aspx");
            }
        }

    }
}