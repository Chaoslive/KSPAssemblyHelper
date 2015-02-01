using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;

namespace KSPAssemblyHelper
{
    class FuelTank : Part
    {

        double WetMass;
        double DryMass;
        public double FuelMassNow{get;set;}
        
        
        public FuelTank(PartSize Size, double WetMass,int Count)
        {
            this.Size = Size;
            NormalizeMass(Size: Size, AbnormalWetMass: WetMass, WetMass: out this.WetMass, DryMass: out this.DryMass);
            this.Name = "FuelTank(" + this.Size.ToString() + "):" + this.WetMass.ToString();
            this.FuelMassNow = WetMass - DryMass;
            this.Count = 1;
        }

        public override double GetMassNow()
        {
            //Debug.Print(this.Name + "Mass:" + (DryMass + (WetMass - DryMass) * FuelRate).ToString());
            return (DryMass + FuelMassNow);
        }

        /// <summary>
        /// 渡された湿重量を指定サイズの最小タンクの整数倍になおす
        /// </summary>
        /// <param name="Size">タンクのサイズ</param>
        /// <param name="AbnormalWetMass">正規化される湿重量</param>
        /// <param name="WetMass">正規化された湿重量が格納される変数</param>
        /// <param name="DryMass">正規化された乾燥重量が格納される変数</param>
        private static void NormalizeMass(PartSize Size, double AbnormalWetMass, out double WetMass, out double DryMass)
        {
            double unitWet;
            double unitDry;
            switch (Size)
            {
                case PartSize.Tiny:
                    unitWet = Properties.Settings.Default.UnitMass_Tiny_Wet;
                    unitDry = Properties.Settings.Default.UnitMass_Tiny_Dry;
                    break;
                case PartSize.Small:
                    unitWet = Properties.Settings.Default.UnitMass_Small_Wet;
                    unitDry = Properties.Settings.Default.UnitMass_Small_Dry;
                    break;
                case PartSize.Large:
                    unitWet = Properties.Settings.Default.UnitMass_Large_Wet;
                    unitDry = Properties.Settings.Default.UnitMass_Large_Dry;
                    break;
                case PartSize.Extra:
                    unitWet = Properties.Settings.Default.UnitMass_Extra_Wet;
                    unitDry = Properties.Settings.Default.UnitMass_Extra_Dry;
                    break;
                default:
                    throw new Exception("INvalid part size");
            }
            var tankNum = (int)((AbnormalWetMass )/unitWet);
            WetMass = unitWet * tankNum;
            DryMass = unitDry * tankNum;
            
            
            //var rec =
            //    (from n in model.PartsDataXML.Descendants("FuelTank")

            //     select n.Attribute("Cost").Value.Cast<double>());
            //Debug.Print("結果件数:" + rec.Count().ToString());
            //foreach (var x in rec)
            //{
            //    Debug.Print(x.ToString());
            //}
            //Debug.Print(rec.ToString());
        }

        
        void PrintSize()
        {
            Debug.Print(Size.ToString());
        }
    }


}
