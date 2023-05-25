 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }
        //frmHome fhome = new frmHome();
        private void btnSignUp_Click(object sender, EventArgs e)//Button for SignUp
        {
            //Create an variyable for check that form open or not
            frmSignUp sing = (frmSignUp)Application.OpenForms["frmSignUp"];
            if (sing == null)//checking that vaiyable state null or not
            {
                sing = new frmSignUp();//if null create a new object for that form 
                sing.Show();//Show that form
                
                
                
            }
            else
            {
                sing.Show();//else show that form without create new form 
            }
            this.Hide();//To hide home form
        }

        private void btnLogin_Click(object sender, EventArgs e)//Button for Login
        {
            //Create an variyable for check that form open or not
            frmLogin login = (frmLogin)Application.OpenForms["frmLogin"];
            if (login == null)//checking that vaiyable state null or not
            {
                login = new frmLogin();//if null create a new object for that form 
                login.Show();//Show that form
            }
            else
            {
                login.Show();//else show that form without create new form
            }
            this.Hide();//To hide home form
        }
    }
}
