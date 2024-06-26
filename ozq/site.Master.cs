using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ozq
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["UserId"] != null)
                {
                    int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);
                    // Assuming you have a link with ID 'cartLink'
                    customerLink.HRef = "profile.aspx?customerId=" + userId;
                    PaymentLink.HRef = "payment.aspx?customerId=" + userId;

                    // Call the function to load cart items for the customer
                    CustomerCartFuntionMain(userId);

                }
            }
        }

        protected bool IsLoggedIn()
        {
            return Request.Cookies["UserId"] != null;
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            // Remove the cookie to log the user out
            if (Request.Cookies["UserId"] != null)
            {
                HttpCookie myCookie = new HttpCookie("UserId");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            // Redirect to home page or wherever you want
            Response.Redirect("login.aspx");
        }


        private void CustomerCartFuntionMain(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;



            // Query to get the total count of cart items
            string countQuery = "SELECT COUNT(*) AS TotalCount FROM Cart WHERE CustomerID = @customerId";

            // Query to get the cart item details
            string cartItemsQuery = @"
                SELECT c.ProductID, SUM(c.Quantity) AS TotalQuantity, p.ProductName, p.Description, p.Price, p.ProductImage
                FROM Cart c
                JOIN Products p ON c.ProductID = p.ProductID
                WHERE c.CustomerID = @customerId
                GROUP BY c.ProductID, p.ProductName, p.Description, p.Price, p.ProductImage";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get the total count of cart items
                using (SqlCommand countCmd = new SqlCommand(countQuery, conn))
                {
                    countCmd.Parameters.AddWithValue("@customerId", customerId);
                    int totalCount = (int)countCmd.ExecuteScalar();

                    // Set the total count to the label
                    lblTotalCount.Text = totalCount.ToString();
                }

                // Get the cart item details
                using (SqlCommand cartItemsCmd = new SqlCommand(cartItemsQuery, conn))
                {
                    cartItemsCmd.Parameters.AddWithValue("@customerId", customerId);

                    SqlDataReader reader = cartItemsCmd.ExecuteReader();

                    // Bind the reader to a Repeater or perform actions based on orders
                    CartitemsRepeater.DataSource = reader;
                    CartitemsRepeater.DataBind();

                    // Process each order
                    while (reader.Read())
                    {
                        // Access cart item details using reader["ColumnName"]
                        string ProductID = reader["ProductID"].ToString();
                        string Quantity = reader["TotalQuantity"].ToString();

                        // Display the product and its total quantity
                        // Note: Modify this part as needed to display in your desired format
                        string productInfo = $"ProductID: {ProductID}, Total Quantity: {Quantity}";
                        // Do something with the cart item details
                    }
                }
            }
        }


        protected void RemoveItem_Click(object sender, EventArgs e)
        {
            if (sender is Button btnRemoveItem)
            {
                int productIdToRemove = Convert.ToInt32(btnRemoveItem.CommandArgument);

                if (Request.Cookies["UserId"] != null)
                {
                    int customerId = Convert.ToInt32(Request.Cookies["UserId"].Value);

                    // Implement your logic to remove the item with productIdToRemove for the given customerId
                    RemoveCartItem(customerId, productIdToRemove);

                    // Refresh the cart items
                    CustomerCartFuntionMain(customerId);
                }
                else
                {
                    // Handle the case where there is no customer ID (e.g., redirect to login)
                }
            }
        }


        private void RemoveCartItem(int customerId, int productIdToRemove)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "DELETE FROM Cart WHERE CustomerID = @customerId AND ProductID = @productIdToRemove";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@productIdToRemove", productIdToRemove);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}