using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SortAlgorithmVisualiser
{
    class BubbleSort : ISortEngine
    {
        private int[] Array;
        private Graphics g;
        private int maxVal;
        Brush whiteBrush = new SolidBrush(Color.FloralWhite);
        Brush blackBrush = new SolidBrush(Color.Black);

        public BubbleSort(int[] Array, Graphics g, int maxVal)
        {
            this.Array = Array;
            this.g = g;
            this.maxVal = maxVal;
        }

        public void NextStep()
        {
            for (int i = 0; i < Array.Length - 1; i++)
            {
                if (Array[i] > Array[i + 1])
                {
                    Swap(i, i + 1);
                }
            }
        }
        public void ReDraw()
        {
            for (int i = 0; i < Array.Length - 1; i++)
            {
                using (var brush = new SolidBrush(Color.White))
                {
                    g.FillRectangle(brush, i, maxVal - Array[i], 1, maxVal);
                }
            }
        }
        private void Swap(int i, int p)
        {
            int temp = Array[i];
            Array[i] = Array[i + 1];
            Array[i + 1] = temp;

            DrawBar(i, Array[i]);
            DrawBar(p, Array[p]);
        }
        private void DrawBar(int position, int height)
        {
            g.FillRectangle(blackBrush, position, 0, 1, maxVal);
            g.FillRectangle(whiteBrush, position, maxVal - Array[position], 1, maxVal);

        }
        public bool IsSorted()
        {
            for (int i = 0; i < Array.Length - 1; i++)
            {
                if (Array[i] > Array[i + 1]) return false;
            }
            return true;
        }
    }
}
