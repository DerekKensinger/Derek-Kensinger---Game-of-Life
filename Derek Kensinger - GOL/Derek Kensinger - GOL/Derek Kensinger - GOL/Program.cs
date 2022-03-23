using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Derek_Kensinger___GOL
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        // Converting a random int into a random boolean
        public static bool NextBool(this Random rng) {
            return rng.Next(0, 2) % 2 == 0;
        }

        // Use this instead of the double nested for-loop logic
        public static void ForEach(this bool[,] array, Action<int, int> todo) {
            for (int y = 0; y < array.GetLength(1); y++) {
                for (int x = 0; x < array.GetLength(0); x++) {
                    todo(x, y);
                }
            }
        }

    }
}
