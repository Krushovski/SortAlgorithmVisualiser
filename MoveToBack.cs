using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortAlgorithmVisualiser
{
    class MoveToBack : ISortEngine
    {
        private int[] Array;
        private Graphics g;
        private int maxVal;
        Brush whiteBrush = new SolidBrush(Color.FloralWhite);
        Brush blackBrush = new SolidBrush(Color.Black);
        private int CurrentListPointer = 0;
        public MoveToBack(int[] Array, Graphics g, int maxVal)
        {
            this.Array = Array;
            this.g = g;
            this.maxVal = maxVal;
        }

        public void NextStep()
        {
            if (CurrentListPointer >= Array.Length - 1) CurrentListPointer = 0;
            if (Array[CurrentListPointer] > Array[CurrentListPointer + 1]) Rotate(CurrentListPointer);
            CurrentListPointer++;
        }

        private void Rotate(int currentListPointer)
        {
            int temp = Array[CurrentListPointer];
            int endPoint = Array.Length - 1;
            for (int i = CurrentListPointer; i < endPoint; i++)
            {
                Array[i] = Array[i + 1];
                DrawBar(i, Array[i]);
            }
            Array[endPoint] = temp;
            DrawBar(endPoint, Array[endPoint]);
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
