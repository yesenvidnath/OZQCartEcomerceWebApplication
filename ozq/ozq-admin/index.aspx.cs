using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Web.Security;

namespace ozq.ozq_admin
{

    public partial class index : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["AdminLoggedIn"] != null && (bool)Session["AdminLoggedIn"])
            {
                // Admin is logged in
            }
            else
            {
                // Admin is not logged in, handle accordingly (redirect to login page, show a message, etc.)
                Response.Redirect("../admin-login.aspx");
            }
            if (!IsPostBack)
            {
                // Load and display total number of orders

                DisplayTotalOrders();
                DisplayTotalProducts();
                DisplayTotalCustomers();
                PopulateAdminsGridView();
                DisplayOrdersInGridView();
                PopulateProductsGridView();
                DisplayCustomersInGridView();

            }
        }


        //
        // Display all the things in the main Dashbord 
        //
        private void DisplayTotalOrders()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT COUNT(*) FROM Orders";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int totalOrders = (int)command.ExecuteScalar();

                        // Update the label in the card with the total number of orders
                        totalOrdersLabel.InnerText = totalOrders.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., log the error
                // You can also display an error message to the user
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        private void DisplayTotalCustomers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT COUNT(*) FROM Customers";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int totalCustomers = (int)command.ExecuteScalar();

                        // Update the label in the card with the total number of customers
                        totalCustomersLabel.InnerText = totalCustomers.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., log the error
                // You can also display an error message to the user
                // Response.Write("An error occurred: " + ex.Message);
            }
        }
        private void DisplayTotalProducts()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT COUNT(*) FROM Products";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int totalProducts = (int)command.ExecuteScalar();

                        // Update the label in the card with the total number of products
                        totalProductsLabel.InnerText = totalProducts.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., log the error
                // You can also display an error message to the user
                // Response.Write("An error occurred: " + ex.Message);
            }
        }




        // 
        // Order Editing Funtonality 
        //

        protected void ChangeOrderType_Click(object sender, EventArgs e)
        {
            // Get the order ID from the CommandArgument
            Button btn = (Button)sender;
            string orderId = btn.CommandArgument;

            // Get the selected order type
            DropDownList orderTypeDropdown = (DropDownList)btn.Parent.FindControl("orderTypeDropdown");
            string selectedOrderType = orderTypeDropdown.SelectedValue;

            // Update the order type for this order
            UpdateOrderType(orderId, selectedOrderType);

            // Refresh the GridView to reflect the changes
            DisplayOrdersInGridView();
        }

        private void UpdateOrderType(string orderId, string orderType)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string updateQuery = "UPDATE Orders SET OrderType = @OrderType WHERE OrderID = @OrderID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);
                        command.Parameters.AddWithValue("@OrderType", orderType);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }


        private void DisplayOrdersInGridView()
        {
            // Connection string
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

            // SQL query to retrieve orders with customer names and products
            string query = @"
            SELECT Orders.OrderID, Customers.FirstName, Orders.OrderType, (
                SELECT ProductName + ', '
                FROM OrderItems
                JOIN Products ON OrderItems.ProductID = Products.ProductID
                WHERE OrderItems.OrderID = Orders.OrderID
                FOR XML PATH('')
            ) AS Products
            FROM Orders
            JOIN Customers ON Orders.CustomerID = Customers.CustomerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ordersGridView.DataSource = dt;
                    ordersGridView.DataBind();
                }
            }
        }


        protected void DeleteOrder_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                int orderId;
                if (int.TryParse(button.CommandArgument, out orderId))
                {
                    // Implement the logic to delete the order
                    DeleteOrder(orderId);
                }
            }

            // Refresh the GridView to reflect the changes
            DisplayOrdersInGridView();
        }

        private void DeleteOrder(int orderId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

                // Delete associated records from OrderItems table
                string deleteOrderItemsQuery = "DELETE FROM OrderItems WHERE OrderID = @OrderId";

                // Delete the order from Orders table
                string deleteOrderQuery = "DELETE FROM Orders WHERE OrderID = @OrderId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete associated records from OrderItems table
                    using (SqlCommand command = new SqlCommand(deleteOrderItemsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        int rowsAffected = command.ExecuteNonQuery();
                        // Optionally, you can provide feedback about deleted OrderItems
                    }

                    // Delete the order from Orders table
                    using (SqlCommand command = new SqlCommand(deleteOrderQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        int rowsAffected = command.ExecuteNonQuery();
                        // Optionally, you can provide feedback about the deleted order
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }



        // 
        // Manage Product Funtonality Section 
        //

        private DataTable GetProducts()
        {
            DataTable dataTable = new DataTable();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT * FROM Products";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }

            return dataTable;
        }

        private void PopulateProductsGridView()
        {
            DataTable productsDataTable = GetProducts();
            productsGridView.DataSource = productsDataTable;
            productsGridView.DataBind();
        }


        protected void ProductsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int productID = Convert.ToInt32(productsGridView.DataKeys[rowIndex].Value);

                // Call a method to delete the product based on the ProductID
                DeleteProduct(productID);

                // Refresh the GridView to reflect the changes
                PopulateProductsGridView();
            }
        }


        private void DeleteProduct(int productId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

                // Delete associated records from OrderItems table
                string deleteOrderItemsQuery = "DELETE FROM OrderItems WHERE ProductID = @productId";
                
                // Delete associated records from OrderItems table
                string deleteCartQuery = "DELETE FROM Cart WHERE ProductID = @productId";

                // Delete the product from Products table
                string deleteCommentQuery = "DELETE FROM Comment WHERE ProductID = @productId";

                // Delete the product from Products table
                string deleteProductQuery = "DELETE FROM Products WHERE ProductID = @productId"; 
                

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete associated records from OrderItems table
                    using (SqlCommand command = new SqlCommand(deleteOrderItemsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    // Delete the product from Cart table
                    using (SqlCommand command = new SqlCommand(deleteCartQuery, connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                    
                    // Delete the product from Cart table
                    using (SqlCommand command = new SqlCommand(deleteCommentQuery, connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    // Delete the product from Products table
                    using (SqlCommand command = new SqlCommand(deleteProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }


        protected void InsertProduct(object sender, EventArgs e)
        {
            // Get values from the form
            string productName = productNames.Text;
            string productDescription = productDescriptions.Text;
            decimal price = Convert.ToDecimal(productPrice.Text);
            int stock = Convert.ToInt32(productStock.Text);

            try
            {
                // Get the file name from the FileUpload control
                string fileName = Path.GetFileName(productImage.FileName);

                // Save the file to the server's img folder
                string imgPath = "~/img/" + fileName;
                productImage.SaveAs(Server.MapPath(imgPath));

                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Define the query to insert a new product
                    string query = "INSERT INTO Products (ProductName, Description, Price, Stock, ProductImage) " +
                                   "VALUES (@ProductName, @Description, @Price, @Stock, @ProductImage)";

                    // Create a new SqlCommand and set the parameters
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", productName);
                    command.Parameters.AddWithValue("@Description", productDescription);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Stock", stock);
                    command.Parameters.AddWithValue("@ProductImage", imgPath);

                    // Open the connection and execute the query
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Clear the form after successful insertion
                productNames.Text = "";
                productDescriptions.Text = "";
                productPrice.Text = "";
                productStock.Text = "";

                // Optionally, you can display a success message
                Response.Write("<script>alert('Product inserted successfully!');</script>");

                PopulateProductsGridView();

                // Redirect to the same page to clear the form
                Response.Redirect(Request.Url.AbsoluteUri);


            }
            catch (Exception ex)
            {
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        protected void SearchProduct(object sender, EventArgs e)
        {
            // Get the product ID to search for
            int productIdToSearch = int.Parse(productId.Text);

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT * FROM Products WHERE ProductID = @ProductId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the SQL command
                        command.Parameters.AddWithValue("@ProductId", productIdToSearch);

                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            // Product found, populate the form
                            productNames.Text = reader["ProductName"].ToString();
                            productDescriptions.Text = reader["Description"].ToString();
                            productPrice.Text = reader["Price"].ToString();
                            productStock.Text = reader["Stock"].ToString();

                            // Optionally, you can display a success message
                            Response.Write("<script>alert('Product found!');</script>");
                        }
                        else
                        {
                            // Product not found, clear the form
                            ClearForm();

                            // Optionally, you can display an error message
                            Response.Write("<script>alert('Product not found.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            productNames.Text = string.Empty;
            productDescriptions.Text = string.Empty;
            productPrice.Text = string.Empty;
            productStock.Text = string.Empty;

        }


        protected void UpdateProduct(object sender, EventArgs e)
        {
            int productIdToUpdate = int.Parse(productId.Text);
            string updatedProductName = productNames.Text;
            string updatedProductDescription = productDescriptions.Text;
            decimal updatedProductPrice = decimal.Parse(productPrice.Text);
            int updatedProductStock = int.Parse(productStock.Text);

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "UPDATE Products SET ProductName = @ProductName, Description = @Description, Price = @Price, Stock = @Stock, ProductImage = @ProductImage WHERE ProductID = @ProductId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", updatedProductName);
                        command.Parameters.AddWithValue("@Description", updatedProductDescription);
                        command.Parameters.AddWithValue("@Price", updatedProductPrice);
                        command.Parameters.AddWithValue("@Stock", updatedProductStock);

                        // Handle file upload for product image
                        if (productImage.HasFile)
                        {
                            string fileName = Path.GetFileName(productImage.FileName);
                            string imagePath = "~/img/" + fileName;
                            productImage.SaveAs(Server.MapPath(imagePath));
                            command.Parameters.AddWithValue("@ProductImage", imagePath);
                        }
                        else
                        {
                            // If no new image uploaded, keep the existing image
                            command.Parameters.AddWithValue("@ProductImage", GetProductImage(productIdToUpdate));
                        }

                        command.Parameters.AddWithValue("@ProductId", productIdToUpdate);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Product updated successfully
                            // Optionally, you can display a success message
                            Response.Write("<script>alert('Product updated successfully.');</script>");
                        }
                        else
                        {
                            // No product with the given ID found
                            // Optionally, you can display an error message
                            Response.Write("<script>alert('Product not found. Update failed.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        // Helper method to get existing product image path by ProductID
        private string GetProductImage(int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "SELECT ProductImage FROM Products WHERE ProductID = @ProductId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    connection.Open();
                    string existingImagePath = command.ExecuteScalar() as string;
                    return existingImagePath;
                }
            }
        }



        // 
        // Manage Customer Funtonality Section 
        //

        protected void DisplayCustomersInGridView()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT * FROM Customers";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        customersGridView.DataSource = dt;
                        customersGridView.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        protected void CustomersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCustomer")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                if (customersGridView.DataKeys != null)
                {
                    int customerId = Convert.ToInt32(customersGridView.DataKeys[rowIndex].Value);

                    // Call a method to delete the customer based on the CustomerID
                    DeleteCustomer(customerId);

                    // Refresh the GridView to reflect the changes
                    DisplayCustomersInGridView();
                }
            }
        }

        private void DeleteCustomer(int customerId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

                // Delete associated records from OrderItems table
                string deleteOrderItemsQuery = "DELETE FROM OrderItems WHERE OrderID IN (SELECT OrderID FROM Orders WHERE CustomerID = @customerId)";

                // Delete associated records from Comments table
                string deleteCommentsQuery = "DELETE FROM Comment WHERE CustomerID = @customerId";

                // Delete associated records from Orders table
                string deleteOrdersQuery = "DELETE FROM Orders WHERE CustomerID = @customerId";

                // Delete the customer from Customers table
                string deleteCustomerQuery = "DELETE FROM Customers WHERE CustomerID = @customerId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete associated records from OrderItems table
                    using (SqlCommand command = new SqlCommand(deleteOrderItemsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    // Delete associated records from Comments table
                    using (SqlCommand command = new SqlCommand(deleteCommentsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    // Delete associated records from Orders table
                    using (SqlCommand command = new SqlCommand(deleteOrdersQuery, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    // Delete the customer from Customers table
                    using (SqlCommand command = new SqlCommand(deleteCustomerQuery, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }


        // 
        // Admin Section Funtonality Section 
        //

        private DataTable GetAdmins()
        {
            DataTable dataTable = new DataTable();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT * FROM Admins";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }

            return dataTable;
        }

        private void PopulateAdminsGridView()
        {
            DataTable adminsDataTable = GetAdmins();
            GridView1.DataSource = adminsDataTable;
            GridView1.DataBind();
        }

        protected void AdminGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAdmin")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int adminID = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);

                // Call a method to delete the admin based on the AdminID
                DeleteAdmin(adminID);

                // Refresh the GridView to reflect the changes
                PopulateAdminsGridView();
            }
        }

        private void DeleteAdmin(int adminID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

                // Delete the admin from Admins table
                string deleteAdminQuery = "DELETE FROM Admins WHERE AdminID = @adminID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(deleteAdminQuery, connection))
                    {
                        command.Parameters.AddWithValue("@adminID", adminID);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        protected void InsertAdmin(object sender, EventArgs e)
        {
            // Get values from the form
            string firstName = FirstName.Text;
            string lastName = LastName.Text;
            string email = Email.Text;
            string password = Password.Text;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

                string query = "INSERT INTO Admins (FirstName, LastName, Email, Password) VALUES (@FirstName, @LastName, @Email, @Password)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }

                // Clear the form after successful insertion
                FirstName.Text = "";
                LastName.Text = "";
                Email.Text = "";
                Password.Text = "";

                // Optionally, you can display a success message
                Response.Write("<script>alert('Admin inserted successfully!');</script>");

                PopulateAdminsGridView();

                // Redirect to the same page to clear the form
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        protected void SearchAdmin(object sender, EventArgs e)
        {
            // Get the admin ID to search for
            int adminIdToSearch = int.Parse(AdminID.Text);

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT * FROM Admins WHERE AdminID = @AdminID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the SQL command
                        command.Parameters.AddWithValue("@AdminID", adminIdToSearch);

                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            // Admin found, populate the form
                            FirstName.Text = reader["FirstName"].ToString();
                            LastName.Text = reader["LastName"].ToString();
                            Email.Text = reader["Email"].ToString();
                            Password.Text = reader["Password"].ToString();

                            // Optionally, you can display a success message
                            Response.Write("<script>alert('Admin found!');</script>");
                        }
                        else
                        {
                            // Admin not found, clear the form
                            AdminClearForm();

                            // Optionally, you can display an error message
                            Response.Write("<script>alert('Admin not found.');</script>");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }

        private void AdminClearForm()
        {
            FirstName.Text = string.Empty;
            LastName.Text = string.Empty;
            Email.Text = string.Empty;
            Password.Text = string.Empty;
        }

        protected void UpdateAdmin(object sender, EventArgs e)
        {
            int adminIdToUpdate = int.Parse(AdminID.Text);
            string updatedFirstName = FirstName.Text;
            string updatedLastName = LastName.Text;
            string updatedEmail = Email.Text;
            string updatedPassword = Password.Text;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "UPDATE Admins SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password WHERE AdminID = @AdminID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", updatedFirstName);
                        command.Parameters.AddWithValue("@LastName", updatedLastName);
                        command.Parameters.AddWithValue("@Email", updatedEmail);
                        command.Parameters.AddWithValue("@Password", updatedPassword);
                        command.Parameters.AddWithValue("@AdminID", adminIdToUpdate);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Admin updated successfully
                            // Optionally, you can display a success message
                            Response.Write("<script>alert('Admin updated successfully.');</script>");
                        }
                        else
                        {
                            // No admin with the given ID found
                            // Optionally, you can display an error message
                            Response.Write("<script>alert('Admin not found. Update failed.');</script>");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Response.Write("An error occurred: " + ex.Message);
            }
        }



        protected void Logout_Click(object sender, EventArgs e)
        {
            // Clear the session indicating the admin is logged in
            Session["AdminLoggedIn"] = false;

            // Redirect to home page or wherever you want
            Response.Redirect("../admin-login.aspx");
        }

    }
}