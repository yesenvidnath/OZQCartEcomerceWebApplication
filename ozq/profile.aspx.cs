using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ozq
{
    public partial class profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["customerId"] != null)
                {
                    int customerId = Convert.ToInt32(Request.QueryString["customerId"]);

                    PaymentLink.HRef = "payment.aspx?customerId=" + customerId;
                    ShowOrders(customerId);  
                    AnotherFunction(customerId);
                    CustomerCartFuntion(customerId);

                   
                }


            }
        }

        private void ShowOrders(int customerId)
        {
            // Implement your logic to show orders based on customerId
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "SELECT * FROM Orders WHERE CustomerID = @customerId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Bind the reader to a Repeater or perform actions based on orders
                    // For example:
                    OrderRepeater.DataSource = reader;
                    OrderRepeater.DataBind();

                    // Process each order
                    while (reader.Read())
                    {
                        //Access order details using reader["ColumnName"]
                        // For example:
                        string orderId = reader["OrderID"].ToString();
                        string orderDate = reader["OrderDate"].ToString();
                        string TotalAmount = reader["TotalAmount"].ToString();
                        string OrderType = reader["OrderType"].ToString();
                    }
                }
            }
        }

        private void AnotherFunction(int customerId)
        {
            if (Request.QueryString["customerId"] != null)
            {

                string customerIdmain = Request.QueryString["customerId"];

                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT * FROM Customers WHERE CustomerID = @customerIdmain";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerIdmain", customerIdmain);

                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Bind the reader to the Repeater
                        ProductRepeater.DataSource = reader;
                        ProductRepeater.DataBind();
                    }
                }
            }
        }



        private void CustomerCartFuntion(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

            // Query to get the total count of cart items
            //string countQuery = "SELECT COUNT(*) AS TotalCount FROM Cart WHERE CustomerID = @customerId";

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
                    CustomerCartFuntion(customerId);
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




        protected void UpdateProfile_Click(object sender, EventArgs e)
        {
            // Retrieve the input values
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text; // Note: You should handle password securely
            string address = txtAddress.Text;

            // Retrieve the customer ID from the query string
            if (!int.TryParse(Request.QueryString["customerId"], out int customerId))
            {
                // Handle the case where customerId is not provided in the query string
                Response.Redirect("Profile.aspx?error=true");
                return;
            }

            // Call a method to update the user's profile
            bool success = UpdateUserProfile(customerId, firstName, lastName, email, password, address);

            if (success)
            {
                // Profile update was successful

                // Register the script to show the success modal
                string successScript = "<script>$('#updateSuccessModal').modal('show');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowUpdateSuccessModal", successScript);
                // Profile update was successful
                Response.Redirect($"Profile.aspx?customerId={customerId}"); // Redirect to the user's profile page
            }
            else
            {
                // Rest of the existing code ...
            }
        }

        private bool UpdateUserProfile(int customerId, string firstName, string lastName, string email, string password, string address)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password, Address = @Address WHERE CustomerID = @CustomerId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Address", address);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0; // Return true if rows were affected (update successful)
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the update
                // You can log the exception or return false to indicate failure
                return false;
            }
        }

    }
}