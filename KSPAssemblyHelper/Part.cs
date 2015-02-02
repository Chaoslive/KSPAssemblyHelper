using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace KSPAssemblyHelper
{
    abstract class Part : Composite
    {
        protected PartSize Size { get; set; }
        /// <summary>
        /// 対象配置の数
        /// </summary>
        //public int Count { get; set; }
        public double Mass { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }
        public int Stage { get; set; }

        public void Normalize()
        {
            var result = new List<Composite>();
            //if (this.IsRoot()) { return; }
            foreach (Part child in Children)
            {
                if (child.GetType().Name == "Decoupler" && child.Children.Count == 0)
                {
                    continue;
                }
                else
                {
                    result.Add(child);
                }
            }
            this.Children.Clear();
            this.Children = result;
            foreach (Part child in this.Children)
            {
                child.Normalize();
            }
        }

        abstract public double GetMassNow();

        public double GetMassNowAll(int Stage)
        {
            if ((this.GetType().Name == "Decoupler") && (this.Stage <= Stage)) { return 0; }
            double MassChildren = 0.0;
            foreach (Part child in Children)
            {
                MassChildren += child.GetMassNowAll(Stage);
            }
            return (this.GetMassNow() + MassChildren) * this.Count;
        }


        internal double GetMinimamBurnTime(int Stage)
        {
            double min = Double.MaxValue;
            if (this.TypeName == "Decoupler" && this.Stage <= Stage)
            {
                return min;
            }
            foreach (Part child in Children)
            {
                var m = child.GetMinimamBurnTime(Stage);
                if (m < min)
                {
                    min = m;
                }
            }
            double n;
            if (this.GetType().Name == "Booster" && this.Stage <= Stage)
            {
                Booster t = (Booster)this;
                n = t.FuelMassNow / (t.Thrust / (t.Isp * 9.81));
                if (n < min) { min = n; }
            }
            else if (this.GetType().Name == "FuelTank")
            {
                var KgpS = 0.0;
                List<Engine> childEngine = new List<Engine>();
                foreach (Part child in Children)
                {
                    if (child.GetType().Name == "Engine") { childEngine.Add((Engine)child); }
                }
                foreach (Engine child in childEngine)
                {
                    KgpS += child.Thrust / (child.Isp * 9.81);
                }
                var i = ((FuelTank)this).FuelMassNow / KgpS;
                if (i < min) { min = i; }
            }
            return min;
        }

            internal void BurnFuel(int Stage, double Time)
        {
            foreach (Part child in Children)
            {
                child.BurnFuel(Stage,Time);
            }
            if (this.GetType().Name == "Booster" && this.Stage <= Stage && ((IEngine)this).Enable)
            {
                Booster b = (Booster)this;
                b.FuelMassNow -= b.Thrust / (b.Isp * 9.81) * Time;
                if (b.FuelMassNow <= 0.0)
                {
                    b.Enable = false;
                }
            }
            else if (this.GetType().Name == "Engine" && this.Stage <= Stage && ((IEngine)this).Enable)
            {
                Engine e = (Engine)this;
                FuelTank t = (FuelTank)this.Parent;
                t.FuelMassNow -= e.Thrust/(e.Isp*9.81)*Time;
                if (t.FuelMassNow <= 0.0)
                {
                    e.Enable = false;
                }
            }
        }

            internal List<IEngine> GetEnableEngines(int Stage)
            {
                List<IEngine> ret = new List<IEngine>();
                foreach (Part child in Children)
                {
                    ret.AddRange(child.GetEnableEngines(Stage));
                    if (child.GetType().Name == "Engine" && child.Stage <= Stage && ((Engine)child).Enable)
                    {
                        for (var i = 0; i < child.RealCount(); i++)
                        {
                            ret.Add((Engine)child);
                        }
                    }
                    else if (child.GetType().Name == "Booster" && child.Stage <= Stage && ((Booster)child).Enable)
                    {
                        for (var i = 0; i < child.RealCount(); i++)
                        {
                            ret.Add((Booster)child);
                        }
                    }
                }
                return ret;
            }

            private int RealCount()
            {
                var cnt = this.Count;
                Part parent = (Part)this.Parent;
                while(parent != null){
                    cnt*=parent.Count;
                    parent = (Part)parent.Parent;
                }
                Debug.Assert(cnt != 0);
                return cnt;
            }

        /// <summary>
        /// ステージに有効なエンジン・ブースターが残っていればTrue
        /// </summary>
        /// <param name="Stage"></param>
        /// <returns></returns>
            internal bool IsStageContinue(int Stage)
            {
                List<Decoupler> LDec  = this.GetDecouplerByStage(Stage+1);
                if (LDec.Count == 0)
                {
                    if (this.HasEnableEngineStage(Stage)) { return true; } else { return false; }
                }
                foreach (Decoupler dec in LDec)
                {
                    if (dec.HasEnableEngineStage(Stage))
                    {
                        return true;
                    }
                }
                return false;
            }

            private List<Decoupler> GetDecouplerByStage(int Stage)
            {
                var ret = new List<Decoupler>();
                foreach (Part child in Children)
                {
                    ret.AddRange(child.GetDecouplerByStage(Stage));
                    if (child.GetType().Name == "Decoupler" && child.Stage == Stage)
                    {
                        ret.Add((Decoupler)child);
                    }
                }
                return ret;
            }

            internal bool HasEnableEngineStage(int Stage)
            {
                foreach (Part child in Children)
                {
                    if (child.HasEnableEngineStage(Stage)) { return true; }
                    if (child.GetType().Name == "Engine" || child.GetType().Name == "Booster")
                    {
                        if (child.Stage <= Stage && ((IEngine)child).Enable)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }


            public string TypeName {
                get {
                    return this.GetType().Name;
                }
            }
    }

}
