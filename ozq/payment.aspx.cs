using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PayPal;
using PayPal.Api;

namespace ozq
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        private int customerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (Request.QueryString["customerId"] != null)
                {
                    int customerId = Convert.ToInt32(Request.QueryString["customerId"]);
                    CustomerCartFuntion(customerId);
                    CalculateTotalCost(customerId);
                }

                if (Request.QueryString["productId"] != null)
                {
                    int productId = Convert.ToInt32(Request.QueryString["productId"]);

                }
            }

            // Call a method to initialize PayPal integration
            InitializePayPalIntegration();

        }

        private decimal CalculateTotalCost(SqlDataReader reader)
        {
            decimal totalCost = 0;

            while (reader.Read())
            {
                int quantity = Convert.ToInt32(reader["TotalQuantity"]);
                decimal price = Convert.ToDecimal(reader["Price"]);
                totalCost += quantity * price;
            }

            return totalCost;
        }


        private void CustomerCartFuntion(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

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
                    CartItemsRepeater.DataSource = reader;
                    CartItemsRepeater.DataBind();


                    decimal totalCost = CalculateTotalCost(reader);

                    // Update the total cost label
                    TotalCostLabel.Text = totalCost.ToString("0.00");


                    // Process each order
                    int i = 0;
                    while (reader.Read())
                    {
                        // Access cart item details using reader["ColumnName"]
                        string ProductID = reader["ProductID"].ToString();
                        string Quantity = reader["TotalQuantity"].ToString();

                        // Display the product and its total quantity
                        // Note: Modify this part as needed to display in your desired format
                        string productInfo = $"ProductID: {ProductID}, Total Quantity: {Quantity}";

                        // Check if the quantity is 1, if so, hide the "Reduce" button
                        Button btnReduceItem = (Button)CartItemsRepeater.Items[i].FindControl("btnReduceItem");
                        if (btnReduceItem != null)
                        {
                            int quantity = int.Parse(Quantity);
                            btnReduceItem.Visible = quantity > 1;
                        }

                        i++;
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


        protected void ReduceItem_Click(object sender, EventArgs e)
        {
            if (sender is Button btnReduceItem)
            {
                int productIdToReduce = Convert.ToInt32(btnReduceItem.CommandArgument);

                if (Request.Cookies["UserId"] != null)
                {
                    int customerId = Convert.ToInt32(Request.Cookies["UserId"].Value);

                    // Implement your logic to reduce the item quantity for the given customerId
                    ReduceCartItem(customerId, productIdToReduce);

                    // Refresh the cart items
                    CustomerCartFuntion(customerId);
                }
                else
                {
                    // Handle the case where there is no customer ID (e.g., redirect to login)
                }
            }
        }


        private void ReduceCartItem(int customerId, int productIdToReduce)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

            // Query to reduce the quantity of a specific product for a customer
            string reduceQuantityQuery = @"
            UPDATE TOP(1) Cart
            SET Quantity = Quantity - 1
            WHERE CustomerID = @customerId AND ProductID = @productIdToReduce AND Quantity > 0";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(reduceQuantityQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@productIdToReduce", productIdToReduce);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void CalculateTotalCost(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

            string totalPriceQuery = @"
            SELECT SUM(p.Price * c.Quantity) AS TotalPrice
            FROM Cart c
            JOIN Products p ON c.ProductID = p.ProductID
            WHERE c.CustomerID = @customerId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(totalPriceQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        decimal totalPrice = Convert.ToDecimal(result);
                        TotalCostLabel.Text = totalPrice.ToString("0.00"); // Display the total cost
                    }
                    else
                    {
                        TotalCostLabel.Text = "0.00"; // No products in the cart, set total cost to zero
                    }
                }
            }
        }


        private decimal CalculateTotalAmountByCustomerId(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

            string totalPriceQuery = @"
            SELECT SUM(p.Price * c.Quantity) AS TotalPrice
            FROM Cart c
            JOIN Products p ON c.ProductID = p.ProductID
            WHERE c.CustomerID = @customerId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(totalPriceQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }
                    else
                    {
                        return 0.00M; // Return 0 if no products in the cart
                    }
                }
            }
        }


        //
        // Place Order Funtion Area Start
        //


        protected void PlaceOrderButton_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["UserId"] != null)
            {
                int customerId = Convert.ToInt32(Request.Cookies["UserId"].Value);

                // Retrieve product IDs for the customer from the cart
                List<int> productIds = GetProductIdsInCart(customerId);

                // Calculate total amount
                decimal totalAmount = CalculateTotalAmountByCustomerId(customerId);

                // Get order details (address, zipCode, postalCode, telephoneNo)
                string address = Address.Text;
                string zipCode = ZipCode.Text;
                string postalCode = PostalCode.Text;
                string telephoneNo = TelephoneNo.Text;

                // Insert order details into the Order table
                int orderId = InsertOrderDetails(customerId, productIds, totalAmount, address, zipCode, postalCode, telephoneNo);

                // Display a success message or redirect to a confirmation page
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Order placed with OrderID: " + orderId + "');", true);

                // Delete the cart items for the customer
                DeleteCartItems(customerId);

                // Display the order ID and a message
                MessageLabel.Text = "<div class='alert alert-success'>Thank you for your order! Your Order ID is: " + orderId + ". We appreciate your business and hope to serve you again soon!</div>";

                // Refresh the cart items
                CustomerCartFuntion(customerId);
            }
            else
            {

            }

        }


        // Getting Product deetails form the Cart 
        private List<int> GetProductIdsInCart(int customerId)
        {
            List<int> productIds = new List<int>();

            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

            string query = "SELECT ProductID FROM Cart WHERE CustomerID = @customerId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int productId = Convert.ToInt32(reader["ProductID"]);
                        productIds.Add(productId);
                    }
                }
            }

            return productIds;
        }


        //Placing orders to the table 
        private int InsertOrderDetails(int customerId, List<int> productIds, decimal totalAmount, string address, string zipCode, string postalCode, string telephoneNo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string createOrderQuery = @"
        INSERT INTO Orders (CustomerID, TotalAmount, OrderDate, Address, ZipCode, PostalCode, TelephoneNo, OrderType)
        VALUES (@CustomerId, @TotalAmount, GETDATE(), @Address, @ZipCode, @PostalCode, @TelephoneNo, @OrderType);
        SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(createOrderQuery, conn))
                {
                    conn.Open();

                    // Add parameters for order details
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@ZipCode", zipCode);
                    cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                    cmd.Parameters.AddWithValue("@TelephoneNo", telephoneNo);
                    cmd.Parameters.AddWithValue("@OrderType", "Cash on Order");

                    // Execute the query to insert the order and retrieve the order ID
                    int orderId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Iterate over the product IDs and associate them with the order ID
                    foreach (int productId in productIds)
                    {
                        string associateProductQuery = @"
                    INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price)
                    VALUES (@OrderId, @ProductId, 1, (SELECT Price FROM Products WHERE ProductID = @ProductId))";

                        using (SqlCommand associateCmd = new SqlCommand(associateProductQuery, conn))
                        {
                            associateCmd.Parameters.AddWithValue("@OrderId", orderId);
                            associateCmd.Parameters.AddWithValue("@ProductId", productId);

                            // Execute the query to associate the product with the order
                            associateCmd.ExecuteNonQuery();
                        }
                    }

                    return orderId;
                }
            }
        }



        // Remove the products form the Cart Table after the ordering is successful 
        private void DeleteCartItems(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string deleteCartItemsQuery = @"DELETE FROM Cart WHERE CustomerID = @CustomerId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(deleteCartItemsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //
        // Place Order Funtion Area End
        //



        //
        // PayPal Order Section 
        //

        private APIContext apiContext;

        protected void InitializePayPalIntegration()
        {
            // Retrieve PayPal settings from app.config
            var clientId = ConfigurationManager.AppSettings["clientId"];
            var clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            var mode = ConfigurationManager.AppSettings["mode"];

            // Check if clientId is not null or empty
            if (string.IsNullOrEmpty(clientId))
            {
                throw new Exception("PayPal client ID is missing. Please check your configuration.");
            }

            var config = new Dictionary<string, string>
            {
                { "mode", mode },
                { "clientId", clientId },
                { "clientSecret", clientSecret }
            };

            // Get an access token
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();

            // Assign to the class-level apiContext
            apiContext = new APIContext(accessToken) { Config = config };
        }



        protected void SubmitPaymentButton_Click(object sender, EventArgs e)
        {
            // Initialize PayPal integration
            InitializePayPalIntegration();

            // Get the total amount to be paid
            int customerId = Convert.ToInt32(Request.Cookies["UserId"].Value); // Assuming you have the customerId
            decimal totalAmount = CalculateTotalAmountByCustomerId(customerId);

            try
            {
                // Create a Payment object
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
            {
                new Transaction
                {
                    description = "Your purchase description",
                    amount = new Amount
                    {
                        currency = "USD",
                        total = totalAmount.ToString("0.00") // Make sure to format the amount correctly
                    }
                }
            },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = "http://localhost:PayPal/Success.aspx", // Set your success page URL
                        cancel_url = "http://localhost:PayPal/Cancel.aspx"   // Set your cancel page URL
                    }
                };

                // Create and execute the payment
                var createdPayment = payment.Create(apiContext);

                // Redirect to PayPal for payment
                Response.Redirect(createdPayment.links.First(x => x.rel.ToLower() == "approval_url").href);
            }
            catch (PayPalException ex)
            {
                // Handle any exceptions related to PayPal API calls
                // Log the exception, display an error to the user, etc.
                // Example:
                // Response.Write("Error: " + ex.Message);
            }
        }

    }
}