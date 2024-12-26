using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace LineEndingIssue
{
    public partial class Validator : Page
    {

        protected void btnCheck_Click(object sender, EventArgs e)
        {

            string name = txtName.Text;
            string familyName = txtFamilyName.Text;
            string address = txtAddress.Text;
            string city = txtCity.Text;
            string zipCode = txtZipCode.Text;
            string phone = txtPhone.Text;
            string email = txtEmail.Text;

            if (!ValidateInput())
            {
                return;
            }


            Session["Name"] = name;
            Session["FamilyName"] = familyName;
            Session["Address"] = address;
            Session["City"] = city;
            Session["ZipCode"] = zipCode;
            Session["Phone"] = phone;
            Session["Email"] = email;

        }


        private bool ValidateInput()
        {

            lblError.Visible = false;


            if (txtName.Text == txtFamilyName.Text)
            {
                lblError.Text = "Name cannot be the same as family name.";
                lblError.Visible = true;
                return false;
            }


            if (txtAddress.Text.Length < 2)
            {
                lblError.Text = "Address must be at least 2 letters.";
                lblError.Visible = true;
                return false;
            }


            if (txtCity.Text.Length < 2)
            {
                lblError.Text = "City must be at least 2 letters.";
                lblError.Visible = true;
                return false;
            }



            if (txtZipCode.Text.Length != 6 || !txtZipCode.Text.All(char.IsDigit))
            {
                lblError.Text = "Zip code must be 5 digits.";
                lblError.Visible = true;
                return false;
            }

            string phonePattern = @"^(?:\d{2}-|\d{3}-)\d{7}$";
            if (!Regex.IsMatch(txtPhone.Text, phonePattern))
            {
                lblError.Text = "Phone number must be in the format XX-XXXXXXX or XXX-XXXXXXX.";
                lblError.Visible = true;
                return false;
            }


            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(txtEmail.Text, emailPattern))
            {
                lblError.Text = "E-mail address is not valid.";
                lblError.Visible = true;
                return false;
            }

            return true;
        }
    }
}
