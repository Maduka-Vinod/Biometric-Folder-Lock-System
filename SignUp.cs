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
using System.IO;//Added library for Input/Output 

namespace Project
{
    public partial class frmSignUp : Form
    {
        public frmSignUp()
        {
            InitializeComponent();
        }
        FaceRec face = new FaceRec();//object fro face recognition library
        //Object fro connect database
        SqlConnection sc = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\New folder\Desktop\SendPro\Project\Database1.mdf;Integrated Security=True");
        
        private void btmOpen_Click(object sender, EventArgs e)//Button for Open Camara
        {
            face.openCamera(pictureBox1, pictureBox2);//Open camara
        }

        private void btnSave_Click(object sender, EventArgs e)//Button for Save Image
        {
            face.Save_IMAGE(txbUserName.Text);//Save iamge
            MessageBox.Show("Face saved", "Message", MessageBoxButtons.OK);
        }

        private void btnRecognize_Click(object sender, EventArgs e)//Button for Regcognition Face
        {
            //face.isTrained = true;//For recognition trained face
            face.isTrained = true;
        }

        private void button1_Click(object sender, EventArgs e)//Button for Save data in database
        {
            //DataAdapter object for SQL data Get
            SqlDataAdapter sda1 = new SqlDataAdapter("select count(*) from SignUp where id=id", sc);
            DataTable dt1 = new DataTable();//data table
            sda1.Fill(dt1);//fill table
            int count = Convert.ToInt32(dt1.Rows[0][0]);//initialize count valiable using data table value
            if (count < 1)
            {
                if (txbUserName.Text == "" && txbEmail.Text == "")//check weather both texboxes empty or not
                {
                    //Message for user to enter data
                    MessageBox.Show("Enter your details", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //Adding data to database with SQL command
                    SqlCommand sm = new SqlCommand("insert into SignUp values('" + txbUserName.Text + "','" + txbEmail.Text + "',@pic)", sc);
                    MemoryStream stream = new MemoryStream();//create stream object
                    pictureBox2.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);//to save that picturebox image to stream
                    byte[] pic = stream.ToArray();//Then convert that image into bytes using stream object
                    sm.Parameters.AddWithValue("@pic", pic);//adding values
                    sc.Open();//Open databse
                    sm.ExecuteNonQuery();//execute Query
                    sc.Close();//close database

                    //Message for user
                    MessageBox.Show("Account created", "Message", MessageBoxButtons.OK);
                    frmSignUp ob = (frmSignUp)Application.OpenForms["frmSignUp"];
                    if (ob != null)
                    {
                        frmLogin fl = new frmLogin();
                        fl.Show();
                        ob.Close();
                    }

                }
            }
            else
            {
                MessageBox.Show("Alredy you have an account", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }











            
            
        }
    }
}
