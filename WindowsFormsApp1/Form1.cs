using Alturos.Yolo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        YoloConfigurationDetector configurationDetector;
        YoloConfiguration config;
        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox1.AcceptsReturn = true;
            
            configurationDetector = new YoloConfigurationDetector();
            config = configurationDetector.Detect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
            using (OpenFileDialog op = new OpenFileDialog() { Filter = "*.png | *.PNG" })
            {
                if (op.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = string.Empty;
                    Image img= Image.FromFile(op.FileName);

                    using (var yoloWrapper = new YoloWrapper(config))
                    {
                        var items = yoloWrapper.Detect(op.FileName);
                        foreach (var it in items)
                        {
                            textBox1.Text += $@"Type:       {it.Type}
Confidence:     {it.Confidence}

";
                            using (Graphics gs = Graphics.FromImage(img))
                            {
                                using (Pen pen = new Pen(Color.Red, 5))
                                {
                                    gs.DrawRectangle(pen, it.X, it.Y, it.Width, it.Height);
                                }
                            }

                            pictureBox1.Image = img;
                        }
                    }
                }
                else MessageBox.Show("Choose Image");
            }
        }
    }
}
