using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace KSPAssemblyHelper
{
    enum RandomPartOption
    {
        all,
        root
    }
    static class model
    {
        /// <summary>
        /// 指定された数のパーツを含むツリー構造をランダムに作成する
        /// </summary>
        /// <param name="NumParts">パーツ数</param>
        /// <returns>ツリー構造のルートオブジェクト</returns>
        static public Composite GetRandomAssembly(int NumParts)
        {
            Composite root = GetRandomPart(RandomPartOption.root);
            int count = root.GetChildCount() + 1;
            int r = 0;
            //Debug.Print("Children count = " + count.ToString());
            for (int i = 0; i < NumParts-1; i++)
            {
                
                Debug.Assert(count == root.GetChildCount()+1);
                int idx = MyRand.Next(count);
                Debug.Print("GetRandomPart() ID追加:" + idx);
                //Debug.Assert(idx != 2);
                r = root.AddCompositeByIndex(index:idx,composite:GetRandomPart());
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
            switch (Option)
            {
                case RandomPartOption.all:
                    rndMax = 4;
                    break;
                case RandomPartOption.root:
                    rndMax = 2;
                    break;
                default:
                    throw new Exception("Invalid RandomPartOption");
            }
            switch (MyRand.Next(rndMax))
            {
                case 0:
                    r = new FuelTank();
                    break;
                case 1:
                   r = new Booster();
                    break;
                case 2:
                     r = new Engine();
                    break;
                case 3:
                    r = new Decoupler();
                    break;
                default:
                    throw (new Exception("Unknown part type"));
            }
            return r;
        }

    }
}