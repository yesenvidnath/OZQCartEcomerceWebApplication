using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace ozq
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_Click(object sender, EventArgs e)
        {
            string email = txtLoginEmail.Text;
            string password = txtLoginPassword.Text;

            string recaptchaResponse = Request.Form["g-recaptcha-response"];
            bool isCaptchaValid = ReCaptcha.ValidateRecaptcha(recaptchaResponse);

            if (!isCaptchaValid)
            {
                lblResult.Text = "ReCaptcha validation failed. Please try again.";
                return;  // Do not proceed with login if ReCaptcha is not valid
            }

            int userId = AuthenticateUser(email, password);

            if (userId > 0)
            {
                Session["CustomerId"] = userId;
                // Authentication successful, set a cookie to indicate the user is logged in
                HttpCookie authCookie = new HttpCookie("UserId", userId.ToString());
                Response.Cookies.Add(authCookie);


                // Redirect to home page or wherever you want
                Response.Redirect("index.aspx");
            }
            else
            {
                Response.Redirect("login.aspx?error=true");
            }
        }


        private int AuthenticateUser(string email, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;

            // For this example, let's assume a simple SQL query
            string query = "SELECT CustomerID FROM Customers WHERE Email = @Email AND Password = @Password";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        return Convert.ToInt32(result);
                }
            }

            // Authentication failed, return -1
            return -1;
        }


        protected void Signup_Click(object sender, EventArgs e)
        {
            string firstName = txtSignupFirstName.Text;
            string lastName = txtSignupLastName.Text;
            string email = txtSignupEmail.Text;
            string password = txtSignupPassword.Text;
            string address = txtSignupAddress.Text;

            // Check if any required fields are empty
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(address))
            {
                // Register the script to show an error alert
                string scripts = "<script>alert('Please fill in all required fields.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", scripts);
                return;
            }

            string recaptchaResponse = Request.Form["g-recaptcha-response"];
            bool isCaptchaValid = ReCaptcha.ValidateRecaptcha(recaptchaResponse);

            if (!isCaptchaValid)
            {
                lblResult.Text = "ReCaptcha validation failed. Please try again.";
                return;
            }

            // Example query to insert a new user
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "INSERT INTO Customers (FirstName, LastName, Email, Password, Address) VALUES (@FirstName, @LastName, @Email, @Password, @Address)";

            // Execute the query and pass the parameters
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Address", address);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Check if any of the required fields is empty
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(address))
            {
                lblResult.Text = "Please fill in all the required fields.";
                return;
            }

            string script = "<script>alert('Signup successful!');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "SuccessAlert", script);

            // Redirect to home page after signup
            //Response.Redirect("index.aspx");
        }

    }
}