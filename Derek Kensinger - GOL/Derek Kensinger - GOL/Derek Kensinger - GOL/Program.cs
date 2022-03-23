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

        // Extension Method for shorthand for the Randomize feature
        // Converting a random int into a random boolean
        public static bool NextBool(this Random rando)
        {
            return rando.Next(0, 2) % 2 == 0;
        }

        // Extension Method for shorthand to loop through the universe
        // Use this instead of the double nested for-loop logic - I got fed up with rewriting it so I made this method
        // Action is a reference to a method, which is void and takes two ints as parameters, so that I can do custom functionality for the nested for loops.
        // Action allows the caller to change what "todo"
        public static void ForEach(this bool[,] array, Action<int, int> todo) 
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int x = 0; x < array.GetLength(0); x++) 
                {
                    todo(x, y);
                }
            }
        }

    }
}
