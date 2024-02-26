using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SortAlgorithmVisualiser
{
    public partial class Form1 : Form
    {
        Graphics g;
        int[] Array;
        BackgroundWorker bgw = null;
        bool Paused = false;
        public Form1()
        {
            InitializeComponent();
            PopulateDropDown();
        }

        private void PopulateDropDown()
        {
            List<string> ClassList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(ISortEngine).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
            ClassList.Sort();
            foreach (string entry in ClassList)
            {
                comboBox1.Items.Add(entry);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Array == null) btnReset_Click(null, null);
            bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerAsync(argument: comboBox1.SelectedItem);
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!Paused)
            {
                bgw.CancelAsync();
                Paused = true;
            }
            else
            {
                if (bgw.IsBusy) return;
                int NumEntries = panel1.Width;
                int MaxVal = panel1.Height;
                Paused = false;
                for (int i = 0; i < NumEntries; i++)
                {                 
                    using(var brush = new SolidBrush(Color.Black))
                        g.FillRectangle(brush, i, 0, 1, MaxVal);
                    using (var brush = new SolidBrush(Color.White))
                        g.FillRectangle(brush, i, MaxVal - Array[i], 1, MaxVal);
                }
                bgw.RunWorkerAsync(argument: comboBox1.SelectedItem);
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            g = panel1.CreateGraphics();
            int numEntries = panel1.Width;
            int maxVal = panel1.Height;
            Array = new int[numEntries];
            using (var brush = new SolidBrush(Color.Black))
            {
                g.FillRectangle(brush, 0, 0, numEntries, maxVal);
            }
            var rnd = new Random();
            for (int i = 0; i < numEntries; i++)
            {
                Array[i] = rnd.Next(0, maxVal);
            }
            using (var brush = new SolidBrush(Color.White))
            {
                for (int i = 0; i < numEntries; i++)
                {
                    g.FillRectangle(brush, i, maxVal - Array[i], 1, maxVal);
                }
            }
        }


        public void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            string SortEngineName = (string)e.Argument;
            Type type = Type.GetType("SortAlgorithmVisualiser." + SortEngineName);
            var ctors = type.GetConstructors();
            try
            {
                ISortEngine se =
                    (ISortEngine)ctors[0].Invoke(new object[] { Array, g, panel1.Height });
                while (!se.IsSorted() && (!bgw.CancellationPending))
                {
                    se.NextStep();
                }
            }
            catch(Exception ex)
            {

            }
        }



    }
}
