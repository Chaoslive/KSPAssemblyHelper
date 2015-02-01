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
            tree = model.GetRandomAssembly(NumParts: 3);
            treeView1.Nodes.Add(tree);
            foreach (TreeNode n in treeView1.Nodes)
            {
                n.ExpandAll();
            }
            treeView1.Refresh();
            foreach(var x in model.EngineList)
            {
                //Debug.Print(x.Name);
            }
        }

        private void btnSampleAsm_Click(object sender, EventArgs e)
        {
            Composite tree;
            tree = model.TestAsm.RootPart;
            treeView1.Nodes.Add(tree);
            foreach (TreeNode n in treeView1.Nodes)
            {
                n.ExpandAll();
            }
            treeView1.Refresh();
            MessageBox.Show(model.GetDeltaV(model.TestAsm).ToString());
        }
    }
}
