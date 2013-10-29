using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;

namespace DecisionSupportSystem.Task_4
{
    public class CombinationWithParamView
    {
        public Combination Combination { get; set; }
        public CombinParam Procent { get; set; }
        public CombinParam NominalPrice { get; set; } 
    }

    public class Task4CombinationsView
    {
        public List<CombinationWithParamView> Temps = new List<CombinationWithParamView>();
        public BaseLayer BaseTaskLayer { get; set; }

        public Task4CombinationsView(BaseLayer baseTaskLayer)
        {
            BaseTaskLayer = baseTaskLayer;
        }

        public void AddCombinParams()
        {
            var NameProcent = new CombinParamName{Name = "Procent"};
            var NameNominalPrice = new CombinParamName{Name = "NominalPrice"};

            BaseTaskLayer.BaseMethods.AddCombinParamNames(new List<CombinParamName>{NameProcent, NameNominalPrice});

            var combins = BaseTaskLayer.DssDbContext.Combinations.Local.ToList();

            foreach (var combin in combins)
            {
                var procent = new CombinParam
                    {
                        Combination = combin, 
                        CombinParamName = NameProcent
                    };
                var nominalprice = new CombinParam
                    {
                        Combination = combin, 
                        CombinParamName = NameNominalPrice
                    };

                BaseTaskLayer.BaseMethods.AddCombinationParams(new List<CombinParam> { procent, nominalprice });

                Temps.Add(new CombinationWithParamView
                    {
                        Combination = combin,
                        Procent = procent, 
                        NominalPrice = nominalprice
                    });
            }
        }

        public void SolveCp()
        {
            foreach (var temp in Temps)
            {
                temp.Combination.Cp = temp.Procent.Value*(temp.NominalPrice.Value + 100)/100;
            }
        }
    }
}
