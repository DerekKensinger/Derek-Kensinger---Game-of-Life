﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Derek_Kensinger___GOL
{
    public partial class Form1 : Form
    {
        int widthincells
        {
            get => universe.GetLength(0);
            set => universe = new bool[value, heightincells]; 
        } 
        int heightincells
        {
            get => universe.GetLength(1);
            set => universe = new bool[widthincells, value];
        }

        // The universe array
        bool[,] universe = new bool[5, 5];
        // The scratchPad array
        //bool[,] scratchPad = new bool[5, 5];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;
        Color numColor = Color.Black;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running

            
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            bool[,] temp = new bool[universe.GetLength(0), universe.GetLength(1)];
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int count = CountNeighborsToroidal(x, y);
                    if (finiteToolStripMenuItem.Checked == true)    
                    {
                        count = CountNeighborsFinite(x, y);
                    }
                    //Apply the rules
                    if (count > 1 && count < 4)
                    {
                        if (count == 3)
                        {
                            temp[x, y] = true;
                        }
                        else
                        {
                            temp[x, y] = universe[x, y];
                        }
                    }
                    else
                    {
                        temp[x, y] = false;
                    }
                }
            }

            //// Swap from the universe to the scratchPad
            //bool[,] temp = universe;
            //universe = scratchPad;
            //scratchPad = temp;

            // Copy universe to the temp pad
            universe = temp;

            // Increment generation count
            generations++;

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        public void UpdateStatusStrip() {
            int count = 0;
            for (int x = 0; x < universe.GetLength(0); x++) {
                for (int y = 0; y < universe.GetLength(0); y++) {
                    if (universe[x, y] == true) {
                        count += 1;
                    }
                    //count += universe[x, y] ? 1 : 0;
                }
            }
            // Update status strip generations
            toolStripStatusLabelGenerations.Text =
                "Generations = " + generations.ToString() + " , " +
                "Living Cells = " + count + " , " + 
                "Interval = " + timer.Interval + " ms ";
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            //FLOATS will make the program look prettier

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // Creating a Font for Cells
            Font drawfont = new Font(FontFamily.GenericSerif, cellWidth / 4.0f);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // A Brush to change the colors of numbers in each cell???
            Brush numBrush = new SolidBrush(numColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    //RectangleF to work with floats in the future
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }
                    int count = CountNeighborsToroidal(x, y);
                    if (finiteToolStripMenuItem.Checked == true)
                    {
                        count = CountNeighborsFinite(x, y);
                    }
                    e.Graphics.DrawString(count.ToString(), drawfont, numBrush, cellRect);

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
            numBrush.Dispose();

            // Status strip displaying game specs
            UpdateStatusStrip();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        //Count Neighbors Method for Finite Mode
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        //Count Neighbors Method for Toroidal Mode
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }


        //Closes the window by clicking Exit from the File dropdown
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Creates a New universe
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            // Restarts the Generation count
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();

            // Restart the timer
            timer.Enabled = false;

        }

        // The Start Button
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true; // start timer running
        }

        // The Pause Button
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false; // pause timer running
        }

        // The Next Button
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration(); // calls the next generation
        }

        // Modal Dialog Box for Changing Font Color
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = numColor;
            if (DialogResult.OK == dialog.ShowDialog())
            {
                numColor = dialog.Color;
                graphicsPanel1.Invalidate();
            }
        }

        // Toolbar Options Menu to Change Font Color
        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = numColor;
            if (DialogResult.OK == dialog.ShowDialog())
            {
                numColor = dialog.Color;
                graphicsPanel1.Invalidate();
            }
        }

        // Modal Dialog Option for Changing Window Color
        private void windowColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;              
            }
        }

        // Toolbar Option for Changing Window Color
        private void windowColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
        }

        // Stores the Window Color when Closed
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.Save();
        }

        // Reset to Default Settings for Color
        private void resetColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;

        }

        // Revert to the Previous Color Settings
        private void reloadColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
        }

        /// <summary>
        /// Modal Dialog Box for Changing the Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modalToolStripMenuItem_Click(object sender, EventArgs e)

        {


            Settings dialog = new Settings();

            // Change the universe height in the Settings dialog box
            dialog.Height = heightincells;

            // Change the universe height in the Settings dialog box
            dialog.Width = widthincells;

            // Change the milliseconds between each generation
            dialog.Millisecond = timer.Interval;

            if (DialogResult.OK == dialog.ShowDialog())
            {
                bool[,] sketch = new bool[dialog.Height, dialog.Width];
                for (int x = 0; x < sketch.GetLength(0) && x < universe.GetLength(0); x++)
                {
                    for (int y = 0; y < sketch.GetLength(1) && y < universe.GetLength(1); y++)
                    {
                        sketch[x, y] = universe[x, y];
                    }
                }
                universe = sketch;
                timer.Interval = dialog.Millisecond;
                graphicsPanel1.Invalidate();

            }
        }
        /// <summary>
        /// Check Box to Switch between Tordial and Finite Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tordialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            finiteToolStripMenuItem.Checked = false;
            tordialToolStripMenuItem.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
        }

        /// <summary>
        /// Randomize the Universe by the Current Time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randomizeTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rando = new Random();
            universe.ForEach((x, y) => { 
                universe[x, y] = rando.NextBool();
            });

            graphicsPanel1.Invalidate();
        }

        private void randomizeSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGetNumber getNumber = new FormGetNumber();
            if (getNumber.ShowDialog() == DialogResult.OK)
            {

                Random rando = new Random((int)getNumber.numericUpDown1.Value);
                universe.ForEach((x, y) => {
                    universe[x, y] = rando.NextBool();
                });

                graphicsPanel1.Invalidate();
            }
        }
    }
}
