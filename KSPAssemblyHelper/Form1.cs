using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KSPAssemblyHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Composite tree ;
            tree = model.GetRandomAssembly(NumParts: 10);
            treeView1.Nodes.Add(tree);
            foreach(var x in model.EngineList)
            {
                Debug.Print(x.Name);
            }
        }
    }
}
