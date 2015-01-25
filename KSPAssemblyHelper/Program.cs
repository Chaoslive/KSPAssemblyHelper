using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KSPAssemblyHelper
{
    enum PartSize
    {
        Tiny,
        Small,
        Large,
        Extra
    }
    static class MyRand
    {
        static System.Random rnd = new System.Random();
        static public int Next(int maxValue){
            return rnd.Next(maxValue);
        }
        static public int Next()
        {
            return rnd.Next();
        }
        static public int Next(int minValue,int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }
        static public double NextDouble()
        {
            return rnd.NextDouble();
        }
    }   
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //以下テスト
            /*
            Composite c1 = new Composite();
            c1.Name = "Comp_1";
            Composite c2 = new Composite();
            c2.Name="Comp_2";
            c1.AddComposite(c2);
            c2.AddComposite(new Composite());
            c1.PrintChildrenNames();
             */
        }
    }
}
