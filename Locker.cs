using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;//System Library for Input/Output 
using System.Data.SqlClient;//System library for SQl Database
using System.Configuration;
using System.Security.AccessControl;//System library for directory AccessControl
using FaceRecognition;

namespace Project
{
    public partial class frmLocker : Form
    {
       
        public frmLocker()
        {
            InitializeComponent();
        }
        
        FolderBrowserDialog FBD = new FolderBrowserDialog();//Object for Folder Browsing
        private void btnAdd_Click(object sender, EventArgs e)//Button for Add file
        {
            
            if (FBD.ShowDialog() == DialogResult.OK)//Checking dialog box result
            {
                string path = FBD.SelectedPath;//Adding file path for "path" variyable
                insert(path);//Calling insert() Method
                loadBind();//Calling loadBind() Method
            }
        }

        public void insert(string path)//insert() Method
        {
            //Object for To connect SQL Database
            string status = "Unlocked";
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\New folder\Desktop\SendPro\Project\Database1.mdf;Integrated Security=True");
            cn.Open();//Open that object
            SqlCommand scm = new SqlCommand("insert into Folder(Path,Status)Values(@path,@Status)", cn);//Object for SQL Command
            scm.Parameters.AddWithValue("@path", path);//Adding value with parameter 'path'
            scm.Parameters.AddWithValue("@Status", status);//Adding value with parameter 'path'
            scm.ExecuteNonQuery();//Execute Query
            cn.Close();//Close tha object
        }
        

        public void loadBind()//LoadBind() Method
        {
            //Object for To connect SQL Database
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\New folder\Desktop\SendPro\Project\Database1.mdf;Integrated Security=True");
            SqlCommand com = new SqlCommand("select * from Folder", cn);//Object for SQL Command
            SqlDataAdapter adpter = new SqlDataAdapter(com);//DataAdapter for sql data update
            DataTable dt = new DataTable();//Data table for store tempory data
            adpter.Fill(dt);//Filling that table using adapter object
            dataGridView.DataSource = dt;//View that all table data in data grid View
        }

        private void frmLocker_Load(object sender, EventArgs e)//Method for From load
        {
            loadBind();//Cal loadBind() method for load all data form opening time
        }

        private void btnRemove_Click_1(object sender, EventArgs e)//Button for remove data from data grid view
        {
            int row = dataGridView.RowCount;//Variyable for count data grid view rows
            
            //Show a Message for user and check weather that message box result
            if(MessageBox.Show("Are you sure to delete?", "Message", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                for (int i = row - 1; i >= 0; i--)//forloop for travel all data grid view data
                {
                    if (dataGridView.Rows[i].Selected)//checkig selected row
                    {
                        //variyable id for get id from data grid view selected row from id column
                        string id = dataGridView.Rows[i].Cells["idDataGridViewTextBoxColumn"].Value.ToString();
                        //Object for To connect SQL Database
                        SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\New folder\Desktop\SendPro\Project\Database1.mdf;Integrated Security=True");
                        cn.Open();//Open that object

                        //Object for SQL Command
                        SqlCommand com = new SqlCommand("delete from Folder where id='" + id + "'", cn);
                        com.ExecuteNonQuery();//Execute Query
                        MessageBox.Show("Succesfully Deleted", "Message", MessageBoxButtons.OK);
                        cn.Close();//Close tha object
                        loadBind();//Call loadBind() method for reset data 
                    }
                }
            }
            
        }

        private void btnLock_Click(object sender, EventArgs e)//Button for Lock 
        {
            int row = dataGridView.RowCount;//Variyable for count data grid view rows
            for (int i = row-1; i >= 0; i--)//forloop for travel all data grid view data
            {
                if (dataGridView.Rows[i].Selected)//checkig selected row
                {
                    try
                    {
                        //variyable filePath for get path from data grid view selected row from path column
                        string filePath = dataGridView.Rows[i].Cells["pathDataGridViewTextBoxColumn"].Value.ToString();
                        //Variyable 'id' for get id from selected row
                        string id = dataGridView.Rows[i].Cells["idDataGridViewTextBoxColumn"].Value.ToString();
                        string adminUser = Environment.UserName;

                        //Object for get access control for filePath
                        DirectorySecurity ds = Directory.GetAccessControl(filePath);

                        //Object for get access control and type for adminUser
                        FileSystemAccessRule fsa = new FileSystemAccessRule(adminUser, FileSystemRights.FullControl, AccessControlType.Deny);
                        ds.AddAccessRule(fsa);//adding access control
                        Directory.SetAccessControl(filePath, ds);//Set that access control for that folder

                        //To connect database
                        SqlConnection sc = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\New folder\Desktop\SendPro\Project\Database1.mdf;Integrated Security=True");
                        sc.Open();//To open databse
                        SqlCommand sm = new SqlCommand("update folder set Status='Locked' where id='" + id + "'", sc);//SQL command for update data
                        sm.ExecuteNonQuery();//For execute Query
                        sc.Close();//To Close datbase
                        MessageBox.Show("Folder Locked","Message",MessageBoxButtons.OK);//Message for user
                        loadBind();//For refresh data grid view
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);//For exception handlling
                    }
                    
                }
            }
            
        }

        private void btnUnlock_Click(object sender, EventArgs e)//Button for UnLock 
        {
            if (face.isTrained == true)
            {
                int row = dataGridView.RowCount;//Variyable for count data grid view rows
                for (int i = row - 1; i >= 0; i--)//forloop for travel all data grid view data
                {
                    if (dataGridView.Rows[i].Selected)//checkig selected row
                    {
                        try
                        {
                            //variyable 'filePath' for get path from data grid view selected row from path column
                            string filePath = dataGridView.Rows[i].Cells["pathDataGridViewTextBoxColumn"].Value.ToString();
                            //Variyable 'id' for get id from selected row
                            string id = dataGridView.Rows[i].Cells["idDataGridViewTextBoxColumn"].Value.ToString();
                            string adminUser = Environment.UserName;

                            //Object for get access control for filePath
                            DirectorySecurity ds = Directory.GetAccessControl(filePath);

                            //Object for get access control and type for adminUser
                            FileSystemAccessRule fsa = new FileSystemAccessRule(adminUser, FileSystemRights.FullControl, AccessControlType.Deny);
                            ds.RemoveAccessRule(fsa);//Remove access control
                            Directory.SetAccessControl(filePath, ds);//Set that access control for that folder

                            //To connect database
                            SqlConnection sc = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\New folder\Desktop\SendPro\Project\Database1.mdf;Integrated Security=True");
                            sc.Open();//To open databse
                            SqlCommand sm = new SqlCommand("update folder set Status='Unlocked' where id='" + id + "'", sc);//SQL command for update data
                            sm.ExecuteNonQuery();//For execute Query
                            sc.Close();//To Close datbase
                            MessageBox.Show("Unlocked", "Message", MessageBoxButtons.OK);//Message for user
                            loadBind();//For refresh data grid view
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);//For exception handlling
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Reconize your face first!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        FaceRec face = new FaceRec();
        private void btnOpen_Click(object sender, EventArgs e)
        {
            face.openCamera(pictureBox1, pictureBox2);
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            face.isTrained = true;
        }
    }

}
