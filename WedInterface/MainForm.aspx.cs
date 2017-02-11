using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WedInterface
{
    public partial class MainForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (Entities myEntities = new Entities())
            {
                var user =
                    from employee in myEntities.EMPLOYEE
                    where employee.LOGIN == TextBox1.Text && employee.PASSWORD == TextBox2.Text
                    select new { employee.ID_EMPLOYEE };

                if (user.Count() != 0)
                {
                    Session["login"] = TextBox1.Text;
                    Response.Redirect("SearchPage.aspx");
                }
            }

        }








    }
}