using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FaceRecognition;//Added library for Face recognize
using System.Data.SqlClient;//Added library for connect With databse

namespace Project
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        FaceRec face = new FaceRec();//object fro face recognition library
        //Object fro connect database
        SqlConnection sc = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\New folder\Desktop\SendPro\Project\Database1.mdf;Integrated Security=True");

        private void btnOpen_Click(object sender, EventArgs e)//Button for Open Camara
        {
            face.openCamera(pictureBox1, pictureBox2);//Open camara
        }

        private void btnRecognize_Click(object sender, EventArgs e)//Button for Regcognition Face
        {
            face.isTrained = true;//For recognition trained face
            
        }

        private void btnLogin_Click_1(object sender, EventArgs e)//Button for Login
        {
            if (txbUserName.Text == "")//check weather UserName texboxe empty or not
            {
                //If empty Message for User
                MessageBox.Show("Enter User_Name", "Warinng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bool check = false;//Initialize a boolean variyable

                //DataAdapter object for SQL data Get
                SqlDataAdapter sda1 = new SqlDataAdapter("select count(*) from SignUp where id=id", sc);
                DataTable dt1 = new DataTable();//data table
                sda1.Fill(dt1);//fill table
                int count = Convert.ToInt32(dt1.Rows[0][0]);//initialize count valiable using data table value

                for (int i = 0; i < count; i++)//forloop for travel all values in data table 
                {
                    //Adapter for get user name from database
                    SqlDataAdapter sda2 = new SqlDataAdapter("select User_Name from SignUp where id=" + i + 1 + "", sc);
                    DataTable dt2 = new DataTable();//data table for store UserName
                    sda2.Fill(dt2);//fill data table
                    

                    if(dt2.Rows[0][0].ToString() == txbUserName.Text)//check data table value with text box value equal or not
                    {
                        check = true;//if it's true update check variyable
                    }
                }

                if (check == true)//cheking check variyable is true or not
                {
                    if (face.isTrained == true)//checking face is recongized or not
                    {
                        //Create an variyable for check that form open or not
                        frmLocker locker = (frmLocker)Application.OpenForms["frmLocker"];
                        if (locker == null)//checking that vaiyable state null or not
                        {
                            locker = new frmLocker();//if null create a new object for that form
                            locker.Show();//Show that form
                            this.Close();//Close the current form
                        }
                        else
                        {
                            locker.Show();//else show that form without create new form
                        }
                    }
                    else
                    {
                        //Else Message for User
                        MessageBox.Show("Recognize yor face", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //Else Message for User
                    MessageBox.Show("User Name is incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
