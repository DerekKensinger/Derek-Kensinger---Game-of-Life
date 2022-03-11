using System;
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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }
        public int Height
        {
            get
            {return (int)numericUpDownHeight.Value;}
            set
            { numericUpDownHeight.Value = value;}
        }

        public int Width
        {
            get
            {return (int)numericUpDownWidth.Value;}
            set
            {numericUpDownWidth.Value = value;}
        }

        public int Millisecond
        {
            get
            {return (int)numericUpDownMilliseconds.Value;}
            set 
            { numericUpDownMilliseconds.Value = value; }
        }
    }
}
