using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KSPAssemblyHelper
{
    class Composite
    {
        public string Name { get; set; }
        //public int id { get; set; }
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

        internal int GetChildCount()
        {
            int count = 0;
            foreach(Composite Child in Children){
                count += Child.GetChildCount();
            }
            return count + this.Children.Count;
        }
        /// <summary>
        /// ツリーの指定したノードの子リストの最後にノードを追加します
        /// </summary>
        /// <param name="index">追加される親ノードの番号</param>
        /// <param name="composite">追加するノード</param>
        /// <returns></returns>
        internal int AddCompositeByIndex(int index,Composite composite)
        {
            Debug.Print("対象ID:" + index);
            int r = index - 1;
            if (index <= 0)
            {
                Debug.Print("子に加えました");
                AddComposite(composite);
                return 0;
            }
            else
            {                
                foreach (Composite child in Children)
                {
                    Debug.Print("子に渡します r:" + r.ToString());
                    r = child.AddCompositeByIndex(index:r, composite: composite);
                    Debug.Print("子の返り値は r:" + r.ToString());
                    if (r == 0)
                    {
                        Debug.Print("追加に成功しました");
                        return 0;
                    }
                    else
                    {
                        r--;
                    }
                }
                r++;
                Debug.Print("すべての子を探索しました 返り値 r:" + r.ToString());
                return r;
            }
            
        }
        
        public static implicit operator TreeNode(Composite comp){
            List<TreeNode> ChildList = new List<TreeNode>();
            foreach (Composite child in comp.Children)
            {
                ChildList.Add(child);
            }      
            return new TreeNode(comp.Name,ChildList.ToArray());
        }
    }
}
