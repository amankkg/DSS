using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = System.Action;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberTen : TaskNumberNine
    {
        protected override void CreateTaskParamsTemplate()
        {
            BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Период:" } });
            BaseLayer.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Процентная ставка:" } });
        }

        public override void SolveCp()
        {
            var combinations = BaseLayer.DssDbContext.Combinations.Local;
            double procent = Convert.ToDouble(BaseLayer.Task.TaskParams.ToList()[1].Value);
            foreach (var combination in combinations)
            {
                if (combination.Action.ActionParams.ToList()[5].Value == -1)
                {
                    decimal credit = combination.Action.ActionParams.ToList()[1].Value;
                    double period = Convert.ToDouble(combination.Action.ActionParams.ToList()[0].Value);
                    decimal cost = credit * Convert.ToDecimal(Math.Pow((1 + procent / 100), period));
                    combination.Cp = combination.Action.ActionParams.ToList()[0].Value*
                                     combination.Event.EventParams.ToList()[Convert.ToInt32(combination.Action.ActionParams.ToList()[4].Value)].Value -
                                     cost;
                }
                else
                {
                    double periodBeforeExtend = Convert.ToDouble(combination.Action.ActionParams.ToList()[0].Value);
                    double periodAfterExtend = Convert.ToDouble(combination.Action.ActionParams.ToList()[2].Value);
                    decimal creditBeforeExtend = combination.Action.ActionParams.ToList()[1].Value;
                    decimal creditAfterExtend = combination.Action.ActionParams.ToList()[3].Value;
                    decimal costBeforeExtend = creditBeforeExtend * Convert.ToDecimal(Math.Pow((1 + procent / 100), periodBeforeExtend));
                    decimal costAfterExtend = creditAfterExtend * Convert.ToDecimal(Math.Pow((1 + procent / 100), periodAfterExtend));
                    combination.Cp = combination.Action.ActionParams.ToList()[0].Value*
                                     combination.Event.EventParams.ToList()[Convert.ToInt32(combination.Action.ActionParams.ToList()[4].Value)].Value +
                                     combination.Action.ActionParams.ToList()[2].Value*
                                     combination.Event.EventParams.ToList()[Convert.ToInt32(combination.Action.ActionParams.ToList()[5].Value)].Value -
                                     costBeforeExtend - costAfterExtend;
                }
            }
        }
        
    }
}
