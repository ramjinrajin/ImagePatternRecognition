using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImagePatternDetection
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string FilenameofImage = ""; ;
            OpenFileDialog fg = new OpenFileDialog();
            if (DialogResult.OK == fg.ShowDialog())
            {
                textBox1.Text = fg.FileName;
                 FilenameofImage = fg.SafeFileName;
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
            int Count = 0;

            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\AppData.mdf;Integrated Security=True", db)))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select TOP 1   [IMAGE] FROM  [IMAGES] Where Type=@Type AND Code=1", con);
                    cmd.Parameters.AddWithValue("@Type", FilenameofImage);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            byte[] img = (byte[])rdr["IMAGE"];
                            for (int i = 0; i < img.Length; i++)
                            {
                                if (img[i] == imgLogo[i])
                                {
                                    Count++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }
                    if (Count == imgLogo.Length)
                    {

                         


                        
                            rdr.Close();

                            RenderImage(FilenameofImage);
                      




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

        private void RenderImage(string FileName)
        {

            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con2 = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\AppData.mdf;Integrated Security=True", db)))
            {
                try
                {
                    int i=2;
                    con2.Open();
                    SqlCommand cmd2 = new SqlCommand("Select TOP 4  [IMAGE] FROM  [IMAGES] Where TYPE=@TYPE", con2);
                    cmd2.Parameters.AddWithValue("@TYPE", FileName);
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    
                    if (rdr2.HasRows)
                    {
                        while (rdr2.Read())
                        {
                            FileStream fstream = new FileStream(Application.StartupPath + "\\Photo.jpg", FileMode.Create, FileAccess.ReadWrite);
                            byte[] img = (byte[])rdr2["IMAGE"];
                            fstream.Write(img, 0, img.Length);
                            switch (i)
                            {
                                case 2:
                                    pictureBox2.Image = Image.FromStream(fstream);
                                    break;
                                case 3:
                                    pictureBox3.Image = Image.FromStream(fstream);
                                    break;
                                case 4:
                                    pictureBox4.Image = Image.FromStream(fstream);
                                    break;
                                case 5:
                                    pictureBox5.Image = Image.FromStream(fstream);
                                    break;
                                default:
                                    break;
                            }

                            //pictureBox2.Image = Image.FromStream(fstream);
                            fstream.Flush();
                            fstream.Close();
                            i++;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con2.Close();
                }

            }
        }

    }
}
