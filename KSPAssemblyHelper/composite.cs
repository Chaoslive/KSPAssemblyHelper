using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Composite
    {
        public string Name { get; set; }
        Composite Parent;
        List<Composite> Children = new List<Composite>();

        internal void PrintName()
        {
            Debug.Print(this.Name);
        }

        internal void AddComposite(Composite Child)
        {
            this.Children.Add(Child);
            Child.Parent = this;
        }

        internal void RemoveComposite()
        {
            this.Parent.Children.Remove(this);
        }

        internal void PrintChildrenNames()
        {
            foreach (Composite Child in this.Children)
            {
                Child.PrintChildrenNames();
            }
            Debug.Print(this.Name);
        }
    }
}
