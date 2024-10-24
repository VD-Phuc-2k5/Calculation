using System;
using System.Windows.Forms;

namespace Calculation
{
    public partial class Form1 : Form
    {
        private string currentCalculation = "";
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            timer = new Timer { Interval = 100 };
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (currentCalculation.Length > 0 &&
                textBoxEntry.ClientSize.Width < TextRenderer.MeasureText(currentCalculation, textBoxEntry.Font).Width)
            {
                textBoxEntry.Text = currentCalculation;
                textBoxEntry.SelectionStart = textBoxEntry.Text.Length;
            }
        }

        private void buttonClick(object sender, EventArgs e)
        {
            currentCalculation += (sender as Button).Text;
            textBoxEntry.Text = currentCalculation;

            if (textBoxEntry.ClientSize.Width < TextRenderer.MeasureText(currentCalculation, textBoxEntry.Font).Width)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        private void buttonEqualClick(object sender, EventArgs e)
        {
            try
            {
                textBoxOutput.Text = ComputeH.CalculateExpression(currentCalculation).ToString();
                currentCalculation = textBoxOutput.Text;
            }
            catch
            {
                textBoxOutput.Text = "Error";
            }
        }

        private void ButtonClearClick(object sender, EventArgs e)
        {
            textBoxEntry.Text = "";
            textBoxOutput.Text = "";
            currentCalculation = "";
            timer.Stop();
        }

        private void Button_ClearEntry_Click(object sender, EventArgs e)
        {
            if (currentCalculation.Length > 0)
            {
                currentCalculation = currentCalculation.Remove(currentCalculation.Length - 1);
                textBoxEntry.Text = currentCalculation;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
        }
    }
}