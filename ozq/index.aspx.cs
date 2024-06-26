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

namespace ozq
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductData();

                if (Request.QueryString["CustomerId"] != null)
                {
                    int customerId = Convert.ToInt32(Request.QueryString["CustomerId"]);
                }
            }

        }
        protected bool IsLoggedIn()
        {
            return Request.Cookies["UserId"] != null;
        }

        private void BindProductData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "SELECT ProductID, ProductName, Description, Price, Stock, ProductImage FROM Products";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    ProductRepeater.DataSource = dt;
                    ProductRepeater.DataBind();
                }
            }
        }

        protected string GetActiveImgClass(int ItemIndex)
        {
            if (ItemIndex == 0)
            {
                return "active";
            }
            else
            {
                return "";

            }
        }

        // Added to Cart Funtion 
        protected void AddToCartButton_Command(object sender, CommandEventArgs e)
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
            Response.Redirect("index.aspx");
        }


        protected void SearchProductByName(object sender, EventArgs e)
        {
            string productName = productNameSearch.Text.Trim();
            DataTable productsDataTable = SearchProductsByName(productName);

            // Clear previous product list
            productList.Controls.Clear();

            foreach (DataRow row in productsDataTable.Rows)
            {
                string productId = row["ProductID"].ToString();
                string productNames = row["ProductName"].ToString();
                string productDescription = row["Description"].ToString();

                // Generate the clickable list item with a link to product details
                string productListItem = $"<li class='list-group-item'><a href='productdetails.aspx?productId={productId}'>{productNames} - {productDescription}</a></li>";

                productList.Controls.Add(new LiteralControl(productListItem));
            }
        }


        private DataTable SearchProductsByName(string productName)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
                string query = "SELECT * FROM Products WHERE ProductName LIKE @ProductName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
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


    }
}