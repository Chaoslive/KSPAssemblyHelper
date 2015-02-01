using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
namespace KSPAssemblyHelper
{
    enum RandomPartOption
    {
        all,
        root,
        BoosterChild,
        FuelTankChild,
        DecouplerChild,
        EngineChild
    }
    static class model
    {
        public static int maxStage { get; set; }
        public static int maxDecouplerCount { get; set; }
        public static int maxEngineCount { get; set; }
        public static double Payload { get; set; }
        static public XDocument PartsDataXML = XDocument.Load("PartsData.xml");
        static public List<Engine> EngineList = GetEngineList();
        static public List<Booster> BoosterList = GetBoosterList();
        static public List<Decoupler> DecouplerList = GetDecouplerList();
        static public Assembly TestAsm = GetTestAsm();

        private static Assembly GetTestAsm()
        {
            Part root =new FuelTank(PartSize.Small, 1125, 1);
            Engine e = model.EngineList[0].Clone();
            e.Stage = 0;
            e.Count = 1;
            Decoupler d = model.DecouplerList[0].Clone();
            d.Stage = 1;
            d.Count = 2;
            Booster b = model.BoosterList[0].Clone();
            b.Stage = 0;
            d.AddComposite(b);
            root.AddComposite(d);
            root.AddComposite(e);
            var asm = new Assembly(RootPart: root);
            return asm;
        }

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static model()
        {
            maxStage = 1;
            maxDecouplerCount = 8;
            maxEngineCount = 4;
        }

        private static List<Decoupler> GetDecouplerList()
        {
            var rec = PartsDataXML.Descendants("Decoupler");
            var list = new List<Decoupler>();
            foreach (var x in rec)
            {
                list.Add(new Decoupler(
                    Name: x.Attribute("Name").Value,
                    Size: (PartSize)Enum.Parse(typeof(PartSize), x.Attribute("Size").Value),
                    Cost: int.Parse(x.Attribute("Cost").Value),
                    Mass: double.Parse(x.Attribute("Mass").Value),
                    Stage: 0,
                    Count: 0
                    ));
                Debug.Print("パーツリストに登録(Decoupler):" + x.Attribute("Name").Value);
            }
            return list;
        }

        private static List<Booster> GetBoosterList()
        {
            var rec = PartsDataXML.Descendants("Booster");
            var list = new List<Booster>();
            foreach (var x in rec)
            {
                list.Add(new Booster(
                    Name: x.Attribute("Name").Value,
                    Size: (PartSize)Enum.Parse(typeof(PartSize), x.Attribute("Size").Value),
                    Cost: int.Parse(x.Attribute("Cost").Value),
                    WetMass: double.Parse(x.Attribute("WetMass").Value),
                    DryMass: double.Parse(x.Attribute("DryMass").Value),
                    Thrust: double.Parse(x.Attribute("Thrust").Value),
                    Isp: double.Parse(x.Attribute("ISP").Value),
                    Stage: 0
                    ));
                Debug.Print("パーツリストに登録(Booster):" + x.Attribute("Name").Value);
            }
            return list;
        }

        public static List<Engine> GetEngineList()
        {
            var rec = PartsDataXML.Descendants("Engine");
            var list = new List<Engine>();
            foreach (var x in rec)
            {
                list.Add(new Engine(
                    Name: x.Attribute("Name").Value,
                    Size: (PartSize)Enum.Parse(typeof(PartSize), x.Attribute("Size").Value),
                    Cost: int.Parse(x.Attribute("Cost").Value),
                    Mass: double.Parse(x.Attribute("Mass").Value),
                    Thrust: double.Parse(x.Attribute("Thrust").Value),
                    Isp: double.Parse(x.Attribute("ISP").Value),
                    Stage: 0,
                    Count: 1
                    ));
                Debug.Print("パーツリストに登録(Engine):" + x.Attribute("Name").Value);
            }
            return list;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 指定された数のパーツを含むツリー構造をランダムに作成する
        /// </summary>
        /// <param name="NumParts">パーツ数</param>
        /// <returns>ツリー構造のルートオブジェクト</returns>
        static public Composite GetRandomAssembly(int NumParts)
        {
            Part root = GetRandomPart(RandomPartOption.root);
            Composite cmp = null;
            var count = root.GetChildCount() + 1;
            var r = 0;
            //Debug.Print("Children count = " + count.ToString());
            for (var i = 0; i < NumParts - 1; i++)
            {
                Debug.Assert(count == root.GetChildCount() + 1);
                int idx;
                do
                {
                    idx = MyRand.Next(count);
                    Composite cmpOut;
                    root.GetNodeByIndex(Index: idx, ReturnComposite: out cmpOut);
                } while ((cmp.GetType().Name == "Engine") || ((cmp.GetType().Name == "Decoupler") && (cmp.Children.Count > 0)));
                if (cmp == null)
                {
                    throw (new Exception("Invalid Index"));
                }
                Part p;
                Debug.Print(cmp.GetType().Name);
                switch (cmp.GetType().Name)
                {
                    case "FuelTank":
                        p = GetRandomPart(Option: RandomPartOption.FuelTankChild);
                        break;
                    case "Booster":
                        p = GetRandomPart(RandomPartOption.BoosterChild);
                        break;
                    case "Decoupler":
                        p = GetRandomPart(RandomPartOption.DecouplerChild);
                        break;
                    default:
                        throw (new Exception("Unknown Part Type"));
                }
                Debug.Assert(p.GetChildCount() == 0);
                if (p == null)
                {
                    throw (new Exception("Invalid Part(Null)"));
                }
                r = root.AddCompositeByIndex(index: idx, composite: p);
                //r = root.AddCompositeByIndex(index:1, composite: GetRandomPart());
                Debug.Assert(r <= 0);
                count++;
            }
            root.Normalize();
            //Debug.Assert(root.GetChildCount() ==3);
            return root;
        }

        /// <summary>
        /// パーツをランダムに返します
        /// </summary>
        /// <returns>生成されたパーツオブジェクト</returns>
        private static Composite GetRandomPart()
        {
            return GetRandomPart(RandomPartOption.all);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        private static Part GetRandomPart(RandomPartOption Option)
        {
            Part r;
            int rndMax, rndMin;
            int count;
            switch (Option)
            {
                case RandomPartOption.all:
                    rndMin = 0;
                    rndMax = 4;
                    count = MyRand.Next(1, 9);
                    break;
                case RandomPartOption.root:
                    rndMin = 0;
                    rndMax = 2;
                    count = 1;
                    break;
                case RandomPartOption.BoosterChild:
                    rndMin = 3;
                    rndMax = 4;
                    count = MyRand.Next(1, 9);
                    break;
                case RandomPartOption.DecouplerChild:
                    rndMin = 0;
                    rndMax = 2;
                    count = 1;
                    break;
                case RandomPartOption.EngineChild:
                    return null;
                case RandomPartOption.FuelTankChild:
                    rndMin = 2;
                    rndMax = 4;
                    count = MyRand.Next(1, 9);
                    break;
                default:
                    throw new Exception("Invalid RandomPartOption");
            }
            switch (MyRand.Next(rndMin, rndMax))
            {
                case 0:
                    r = new FuelTank(Size: RandomPartSize(), WetMass: MyRand.NextDouble() * 1000, Count: count);
                    break;
                case 1:
                    Booster b = model.BoosterList[MyRand.Next(model.BoosterList.Count)].Clone();
                    b.Stage = MyRand.Next(model.maxStage+1);
                    r = b;
                    break;
                case 2:
                    Engine s = model.EngineList[MyRand.Next(model.EngineList.Count)].Clone();
                    s.Stage = MyRand.Next(model.maxStage+1);
                    //s.Count = MyRand.Next(model.maxEngineCount) + 1;
                    s.Count = 1;
                    r = s;
                    break;
                case 3:
                    Decoupler d = model.DecouplerList[MyRand.Next(model.DecouplerList.Count)].Clone();
                    d.Stage = MyRand.Next(model.maxStage+1);
                    d.Count = MyRand.Next(model.maxDecouplerCount)+1;
                    r = d;
                    break;
                default:
                    throw (new Exception("Unknown part type"));
            }
            Debug.Assert(r.GetChildCount() == 0);
            return r;
        }

        private static PartSize RandomPartSize()
        {
            return Enum
                .GetValues(typeof(PartSize))
                .Cast<PartSize>()
                .OrderBy(x => MyRand.Next())
                .FirstOrDefault();
        }

        public static double GetDeltaV(Assembly Assembly)
        {
            double DeltaV = 0.0;
            for (int stage = 0; stage <= maxStage; stage++)
            {
                Assembly.DoStage();
                DeltaV += Assembly.PreDeltaV;
            }
                return DeltaV;
        }


        
    }
}