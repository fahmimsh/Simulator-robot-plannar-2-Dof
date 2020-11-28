using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*Program by:fahmi mashuri
  source : https://github.com/fahmimsh/Simulator-robot-plannar-2-Dof
  created time : 24/11/2020 */
namespace Simulator_robot_planar_2_DoF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string in_teta1 = "0.00", in_teta2 = "0.00", in_X = "0.00", in_Y = "0.00";
        private bool parsing1, parsing2, parsing3, parsing4;
        private double L1 = 4, L2 = 4, Teta1 = 0.0, Teta2 = 0.0, x1 = 4.0, x2 = 8.0, y1 = 0.0, y2 = 0.0;
        private double x2_inv = 8.0, y2_inv = 0.0, x1_inv = 0.0, y1_inv = 0.0;
        private double inv_Teta1 = 0.0, inv_Teta2 = 0.0, rad1 = 0.0, rad2 = 0.0;
        /*--------------FORWAWD KINEMATIKA------------------*/
        private void hitungEoe()
        {
            x1 = L1 * Math.Cos(Teta1 * Math.PI / 180);
            y1 = L1 * Math.Sin(Teta1 * Math.PI / 180);

            x2 = x1 + L2 * Math.Cos((Teta1 + Teta2) * Math.PI / 180);
            y2 = y1 + L2 * Math.Sin((Teta1 + Teta2) * Math.PI / 180);
        }
        /*---------------------------INVERS KINEMATIKA------------------------*/
        private void Hitungansudut()
        {
            if (x2_inv == 0) x2_inv = 0.00000000000001;
            rad2 = Math.Acos(((x2_inv * x2_inv) + (y2_inv * y2_inv) - (L1 * L1) - (L2 * L2)) / (2 * L1 * L2));
            rad1 = Math.Atan2(y2_inv, x2_inv) - Math.Atan2((L2 * Math.Sin(rad2)), (L1 + (L2 * Math.Cos(rad2))));
            inv_Teta1 = rad1 * 180 / Math.PI;
            inv_Teta2 = rad2 * 180 / Math.PI;

            x1_inv = L1 * Math.Cos(inv_Teta1 * Math.PI / 180); 
            y1_inv = L1 * Math.Sin(inv_Teta1 * Math.PI / 180);  
        }
        /*---------------------------INPUT NILAI------------------------*/
        private void button1_Click(object sender, EventArgs e)
        {
            hitungEoe();
            textBox1.Text = Math.Round((decimal)x2, 2).ToString();
            textBox2.Text = Math.Round((decimal)y2, 2).ToString();
            knobControl1.Value = Convert.ToInt32(x2);
            knobControl2.Value = Convert.ToInt32(y2);

            double[] data1 = { x1, y1, x2, y2 };
            drawArm(0, 1, data1);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            in_teta1 = textBox3.Text;
            parsing1 = double.TryParse(in_teta1, out Teta1);
            if (parsing1){
                Console.WriteLine(Teta1);
            } else{
                Console.WriteLine("Masukkan input nilai teta1");
            } Console.ReadLine();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            in_teta2 = textBox4.Text;
            parsing2 = double.TryParse(in_teta2, out Teta2);
            if (parsing2){
                Console.WriteLine(Teta2);
            } else{
                Console.WriteLine("Masukkan input nilai teta2");
            } Console.ReadLine();
        }
        private void knobteta1_ValueChanged(object Sender)
        {
            Teta1 = (double)knobteta1.Value;
            hitungEoe();
            tampilan();
        }
        private void knobteta2_ValueChanged(object Sender)
        {
            Teta2 = (double)knobteta2.Value;
            hitungEoe();
            tampilan();
        }
        private void tampilan()
        {
            hitungEoe();
            double[] data1 = { x1, y1, x2, y2 };
            drawArm(0, 1, data1);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            in_X = textBox1.Text;
            parsing3 = double.TryParse(in_X, out x2_inv);
            if (parsing3){
                Console.WriteLine(x2_inv);
                if (Math.Sqrt(Math.Pow(x2_inv, 2) + Math.Pow(y2_inv, 2)) > L1 + L2){
                    MessageBox.Show("TURUNKAN NILAI POSISI X!!!", "Batas Maksimum nilai");
                    x2_inv = 8;
                } else if (Math.Sqrt(Math.Pow(x2_inv, 2) + Math.Pow(y2_inv, 2)) < (L1 - L2)){
                    MessageBox.Show("Panjang lengan tidak mencapai posisi yang dimasukkan", "BATAS MINIMUM LENGAN");
                    x2_inv = 8;
                }
            } else{
                Console.WriteLine("Masukkan input nilai PosisiX");
            } Console.ReadLine();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            in_Y = textBox2.Text;
            parsing4 = double.TryParse(in_Y, out y2_inv);
            if (parsing4){
                Console.WriteLine(y2_inv);
                if (Math.Sqrt(Math.Pow(x2_inv, 2) + Math.Pow(y2_inv, 2)) > L1 + L2){
                    MessageBox.Show("TURUNKAN NILAI POSISI Y!!", "Batas maksimum nilai");
                    y2_inv = 0;
                } else if (Math.Sqrt(Math.Pow(x2_inv, 2) + Math.Pow(y2_inv, 2)) < (L1 - L2)){
                    MessageBox.Show("Panjang lengan tidak mencapai posisi yang dimasukkan", "BATAS MINIMUM LENGAN");
                    y2_inv = 0;
                }
            }
            else{
                Console.WriteLine("Masukkan input nilai PosisiY");
            }Console.ReadLine();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Hitungansudut();
            textBox3.Text = Math.Round((decimal)inv_Teta1, 1).ToString();
            textBox4.Text = Math.Round((decimal)inv_Teta2, 1).ToString();
            knobteta1.Value = Convert.ToInt32(inv_Teta1);
            knobteta2.Value = Convert.ToInt32(inv_Teta2);

            double[] value2 = { x1_inv, y1_inv, x2_inv, y2_inv };
            drawArm(0, 1, value2);
        }
        private void knobControl1_ValueChanged(object Sender)
        {
            x2_inv = (double)knobControl1.Value;
            Hitungansudut();
        }
        private void knobControl2_ValueChanged(object Sender)
        {
            y2_inv = (double)knobControl2.Value;
            Hitungansudut();
        }
        //-------------------------------setting----------------------------//
        private void drawArm(int line, int dot, double[] data)
        {
            chart1.Series[line].Points[1].XValue = data[0];
            chart1.Series[line].Points[1].YValues[0] = data[1];
            chart1.Series[dot].Points[1].XValue = data[0];
            chart1.Series[dot].Points[1].YValues[0] = data[1];

            chart1.Series[line].Points[2].XValue = data[2];
            chart1.Series[line].Points[2].YValues[0] = data[3];
            chart1.Series[dot].Points[2].XValue = data[2];
            chart1.Series[dot].Points[2].YValues[0] = data[3];
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            else
                chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if ((double)numericUpDown1.Value + L2 > 8)
            {
                MessageBox.Show("TURUNKAN NILAI Panjang Lengan 2 !!", "Batas maksimum nilai");
                numericUpDown1.Value = (decimal)L1; 
            } else{
                L1 = (double)numericUpDown1.Value;
                tampilan();
            }
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if ((double)numericUpDown2.Value + L1 > 8)
            {
                MessageBox.Show("TURUNKAN NILAI Panjang lengan 1  !!!", "Batas Maksimum nilai");  
                numericUpDown2.Value = (decimal)L2; 
            } else{
                L2 = (double)numericUpDown2.Value; 
                tampilan(); 
            }
        }
    }
}
