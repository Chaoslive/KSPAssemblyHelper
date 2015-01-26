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
        public static int maxCount { get; set; }
        static public XDocument PartsDataXML = XDocument.Load("PartsData.xml");
        static public List<Engine> EngineList = GetEngineList();
        static public List<Booster> BoosterList = GetBoosterList();
        static public List<Decoupler> DecouplerList = GetDecouplerList();

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
                    Count:0
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
                    Name:x.Attribute("Name").Value,
                    Size:(PartSize)Enum.Parse(typeof(PartSize),x.Attribute("Size").Value),
                    Cost:int.Parse(x.Attribute("Cost").Value),
                    WetMass:double.Parse(x.Attribute("WetMass").Value),
                    DryMass: double.Parse(x.Attribute("DryMass").Value),
                    Thrust:double.Parse(x.Attribute("Thrust").Value),
                    Isp:double.Parse(x.Attribute("ISP").Value),
                    Stage:0
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
                    Name:x.Attribute("Name").Value,
                    Size:(PartSize)Enum.Parse(typeof(PartSize),x.Attribute("Size").Value),
                    Cost:int.Parse(x.Attribute("Cost").Value),
                    Mass:double.Parse(x.Attribute("Mass").Value),
                    Thrust:double.Parse(x.Attribute("Thrust").Value),
                    Isp:double.Parse(x.Attribute("ISP").Value),
                    Stage:0
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
            Composite root = GetRandomPart(RandomPartOption.root);
            var count = root.GetChildCount() + 1;
            var r = 0;
            //Debug.Print("Children count = " + count.ToString());
            for (var i = 0; i < NumParts - 1; i++)
            {
                Debug.Assert(count == root.GetChildCount() + 1);
                var idx = MyRand.Next(count);
                Debug.Print("GetRandomPart() ID追加:" + idx);
                //Debug.Assert(idx != 2);
                var p = GetRandomPart();
                Debug.Assert(p.GetChildCount() == 0);
                r = root.AddCompositeByIndex(index: idx, composite:p );
                //r = root.AddCompositeByIndex(index:1, composite: GetRandomPart());
                Debug.Assert(r <= 0);
                count++;
            }
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
        /// パーツをランダムに返します
        /// </summary>
        /// <param name="Option">ジェネレートオプション all:すべて root:エンジンとデカプラを除く</param>
        /// <returns>生成されたパーツオブジェクト</returns>
        private static Composite GetRandomPart(RandomPartOption Option)
        {
            Composite r;
            int rndMax;
            int count;
            switch (Option)
            {
                case RandomPartOption.all:
                    rndMax = 4;
                    count = MyRand.Next(1, 9);
                    break;
                case RandomPartOption.root:
                    rndMax = 2;
                    count = 1;
                    break;
                default:
                    throw new Exception("Invalid RandomPartOption");
            }
            switch (MyRand.Next(rndMax))
            {
                case 0:
                    r = new FuelTank(Size:RandomPartSize(),WetMass:MyRand.NextDouble()*1000,Count:count);
                    break;
                case 1:
                    Booster b = model.BoosterList[MyRand.Next(model.BoosterList.Count)].Clone();
                    b.Stage = MyRand.Next(model.maxStage);
                    r = b;
                    break;
                case 2:
                    Engine s = model.EngineList[MyRand.Next(model.EngineList.Count)].Clone();
                    s.Stage = MyRand.Next(model.maxStage);
                    r = s;
                    break;
                case 3:
                    Decoupler d = model.DecouplerList[MyRand.Next(model.DecouplerList.Count)].Clone();
                    d.Stage = MyRand.Next(model.maxStage);
                    d.Count = MyRand.Next(model.maxCount);
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


        
    }
}