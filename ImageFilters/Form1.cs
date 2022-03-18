using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;
        string OpenedFilePath;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                 OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
        }

        private void btnZGraph_Click2(object sender, EventArgs e)
        {
          //alphamedian Counting Sort graph
            
            double[] x=new double[3];
            x[0] = 3;
            x[1] = 5;
            x[2] = 7;
            double[] y = new double[3];
            y[0] = 0.5;
            y[1] = 0.8;
            y[2] = 1.2;

            //alphamedian Select K graph

            double[] q = new double[3];
            q[0] = 3;
            q[1] = 5;
            q[2] = 7;
            double[] w = new double[3];
            w[0] = 0.6;
            w[1] = 1.2;
            w[2] = 2.6;

            //adaptive Counting Sort graph
            double[] a = new double[3];
            a[0] = 3;
            a[1] = 5;
            a[2] = 7;
            double[] b = new double[3];
            b[0] = 0.5;
            b[1] = 0.7;
            b[2] = 1.1;

            //adaptive Quick Sort graph
            double[] c = new double[3];
            c[0] = 3;
            c[1] = 5;
            c[2] = 7;
            double[] d = new double[3];
            d[0] = 0.25;
            d[1] = 0.8;
            d[2] = 1.5;



            ZGraphForm ZGFF = new ZGraphForm("Alpha Trimmed Graph", "Wmax", "Time");
            ZGFF.add_curve("(Alpha Counting Sort) : Wmax  & Time ", x, y, Color.Red);
            ZGFF.add_curve("(Alpha Select K ) : Wmax  & Time ", q, w, Color.Black);
            ZGraphForm ZGFF2 = new ZGraphForm("Adaptive Median Graph", "Wmax", "Time");
            ZGFF2.add_curve("(Adaptive Counting Sort) : Wmax  & Time ", a, b, Color.Blue);
            ZGFF2.add_curve("(Adaptive Quick Sort) : Wmax  & Time ", c, d, Color.Yellow);

            ZGFF.Show();
            ZGFF2.Show();

        }
        private void btnZGraph_Click(object sender, EventArgs e)
        {
            // Make up some data points from the N, N log(N) functions
            int N = 40;
            double[] x_values = new double[N];
            double[] y_values_N = new double[N];
            double[] y_values_NLogN = new double[N];

            for (int i = 0; i < N; i++)
            {
                x_values[i] = i;
                y_values_N[i] = i;
                y_values_NLogN[i] = i * Math.Log(i);
            }
            
            //Create a graph and add two curves to it
             ZGraphForm ZGF = new ZGraphForm("Sample Graph", "N", "f(N)");
            ZGF.add_curve("f(N) = N", x_values, y_values_N,Color.Red);
            ZGF.add_curve("f(N) = N Log(N)", x_values, y_values_NLogN, Color.Blue);
            ZGF.Show();
        }

        private void Filter_Click(object sender, EventArgs e)
        {

            ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
            int Sort = 0;
            int Filter = 0;
            string Text = textBox1.Text;
            string Text2 = textBox2.Text;
            int Max_Size = int.Parse(Text);
            int T = int.Parse(Text2);
            string Text_Sort = comboBox1.Text;
            string Text_Filter = comboBox2.Text;
            if (Text_Sort == "1)Counting Sort")
            {
                Sort = 1;
            }
            if (Text_Sort == "2)Quick Sort")
            {
                Sort = 2;
            }
            if (Text_Sort == "3)Select K ")
            {
                Sort = 3;
            }
            if (Text_Sort == "4)Selection Sort")
            {
                Sort = 4;
            }

            if (Text_Filter == "1)Alpha-trim filter")
            {
                Filter = 1;
            }
            else if (Text_Filter == "2)Adaptive median filter")
            {
                Filter = 2;
            }

            if (Sort != 0 && Filter != 0)
            {
                int Start = System.Environment.TickCount;
                ImageOperations.ImageFilter(ImageMatrix, Max_Size, Sort,T, Filter);
                int End = System.Environment.TickCount;
                ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                label3.Text = "";
                double Time = End - Start;
                Time /= 1000;
                label3.Text = (Time).ToString();
                label3.Text += " s";
                label5.Text = "";
                label7.Text= "";
                int element =ImageOperations.kthLargestElement(ImageMatrix,Max_Size);
                label5.Text = (element).ToString();
                int element2 = ImageOperations.kthLowestElement(ImageMatrix,Max_Size);
                label7.Text = (element2).ToString();


              
            }
          
            

        }
        /*    private void FilterAgain_CLick(object sender, EventArgs e)
            {
                int Sort=0;
                int Filter=0;
                string Text = textBox1.Text;
                string Text2 = textBox2.Text;
                int Max_Size = int.Parse(Text);
                int T = int.Parse(Text2);
                string Text_Sort = comboBox1.Text;
                string Text_Filter = comboBox2.Text;
                if (Text_Sort == "1)Counting Sort")
                {
                    Sort = 1;
                }
                if (Text_Sort == "2)Quick Sort")
                {
                    Sort = 2;
                }
                if (Text_Sort == "3)Select K ")
                {
                    Sort = 3;
                }
                if (Text_Sort == "4)Selection Sort")
                {
                    Sort = 4;
                }
           
                if (Text_Filter == "1)Alpha-trim filter")
                {
                     Filter = 1;
                }
                else if (Text_Filter == "2)Adaptive median filter")
                {
                    Filter = 2;
                }



                if (Sort != 0 && Filter != 0)
                {
                    int Start = System.Environment.TickCount;
                    ImageOperations.ImageFilter(ImageMatrix, Max_Size, Sort,T, Filter);
                    int End = System.Environment.TickCount;
                    ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                    label3.Text = "";
                    double Time = End - Start;
                    Time /= 1000;
                    label3.Text = (Time).ToString();
                    label3.Text += " s";
                    label5.Text = "";
                    label7.Text = "";
                    int element = ImageOperations.kthLargestElement(ImageMatrix, Max_Size);
                    label5.Text = (element).ToString();
                    int element2 = ImageOperations.kthLowestElement(ImageMatrix, Max_Size);
                    label7.Text = (element2).ToString();
                }

            }
          */
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}