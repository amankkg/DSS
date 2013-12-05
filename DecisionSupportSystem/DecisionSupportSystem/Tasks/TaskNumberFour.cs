using System.Linq;
using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberFour : TaskSpecific
    {
        protected override void CreateTaskParamsTemplate() { }
        protected override Action CreateActionTemplate()
        {
            return new Action { Name = "Действие", SavingId = base.SavingID};
        }
        protected override Event CreateEventTemplate()
        {
            return new Event { Name = "Событие", Probability = 1, SavingId = base.SavingID };
        }
        protected override Combination CreateCombinationTemplate()
        {
            var combinParamNameF = new CombinParamName {Name = "Увел. проц. ставки"};
            var combinParamNameS = new CombinParamName{Name = "Увел. ном. цены"};
            return new Combination
                {
                    SavingId = base.SavingID,
                    CombinParams = new Collection<CombinParam>
                        {
                            new CombinParam{CombinParamName = combinParamNameF},
                            new CombinParam{CombinParamName = combinParamNameS}
                        }
                };
        }
        
        public override void SolveCP()
        {
            var combinations = BaseLayer.DssDbContext.Combinations.Local;
            foreach (var combination in combinations)
            {
                combination.Cp = combination.CombinParams.ToList()[0].Value * (combination.CombinParams.ToList()[1].Value + 100) / 100;
            }
        }
        public override void NextBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            if (EventErrorCatcher.EntityGroupErrorCount != 0 || EventsViewModel.Events.Count == 0) return;
            CreateCombinations();
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageSolve(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateCombinations();
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
    }
}
