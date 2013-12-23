using System;
using System.Linq;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberTen : TaskNumberNine
    {
        protected override void CreateTaskParamsTemplate()
        {
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Период:" } });
            BaseAlgorithms.Task.TaskParams.Add(new TaskParam { TaskParamName = new TaskParamName { Name = "Процентная ставка:" } });
        }

        public override void SolveCp()
        {
            var combinations = DssDbEntities.Combinations.Local;
            double procent = Convert.ToDouble(BaseAlgorithms.Task.TaskParams.ToList()[1].Value);
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
