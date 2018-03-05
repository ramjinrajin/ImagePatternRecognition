using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace ImagePatternDetection
{
    public partial class UploadImage : Form
    {
        //string ConnectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Projects\Inzenjer\Kerala Univerity\ImagePatternDetection\ImagePatternDetection\Data\AppData.mdf;Integrated Security=True";
        public UploadImage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fg = new OpenFileDialog();
            if (DialogResult.OK == fg.ShowDialog())
            {
                textBox1.Text = fg.FileName;
                if (File.Exists(textBox1.Text))
                {
                    pictureBox1.Image = Image.FromFile(textBox1.Text);
                    pictureBox1.Refresh();
                }
            }

            byte[] imgLogo = new byte[] { };
            if (File.Exists(textBox1.Text))
            {
                StreamReader strImg = new StreamReader(textBox1.Text);
                imgLogo = new byte[File.ReadAllBytes(textBox1.Text).Length];
                imgLogo = File.ReadAllBytes(textBox1.Text);
                strImg.Close();
            }



            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\AppData.mdf;Integrated Security=True", db)))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO IMAGES (IMAGE,CODE,TYPE) VALUES (@IMAGE,@CODE,@TYPE)", con);
                    cmd.Parameters.AddWithValue("@IMAGE", imgLogo);
                    cmd.Parameters.AddWithValue("@CODE", 0);
                    cmd.Parameters.AddWithValue("@TYPE", "496dacdb-4441-405b-8cd3-667f77862b26_425x425.jpg");
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            RenderImage();
        }

        private void RenderImage()
        {
            FileStream fstream = new FileStream(Application.StartupPath + "\\Photo.jpg", FileMode.Create, FileAccess.ReadWrite);
            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\AppData.mdf;Integrated Security=True", db)))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select TOP 1  [IMAGE] FROM  [IMAGES] Where Code=@Code", con);
                    cmd.Parameters.AddWithValue("@Code", 1);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            byte[] img = (byte[])rdr["IMAGE"];
                            fstream.Write(img, 0, img.Length);
                            pictureBox1.Image = Image.FromStream(fstream);
                            fstream.Flush();
                            fstream.Close();
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }

            }
        }

        private void UploadImage_Load(object sender, EventArgs e)
        {

        }
    }
}
