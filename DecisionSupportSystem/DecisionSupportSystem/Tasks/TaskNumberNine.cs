using System.Linq;
using System.Collections.ObjectModel;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberNine : TaskSpecific
    {
        protected override void CreateTaskParamsTemplate()
        {
        }

        protected override Action CreateActionTemplate()
        {
            return new Action
            {
                Name = "Действие",
                SavingId = SavingID,
                ActionParams = new Collection<ActionParam>{
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Срок:"}
                        },
                    new ActionParam
                        {
                            ActionParamName = new ActionParamName{Name = "Затрата:"}
                        }
                }
            };
        }

        protected override Event CreateEventTemplate()
        {
            var eventParamName = new EventParamName { Name = "Событие для расширения" };
            return new Event
            {
                Name = "Событие",
                Probability = 1,
                SavingId = this.SavingID,
                EventParams = new Collection<EventParam>
                    {
                        new EventParam{EventParamName = eventParamName
                    }}
            };
        }

        protected override Combination CreateCombinationTemplate()
        {
            var combinParamNameS = new CombinParamName { Name = "Доход:" };
            return new Combination
            {
                SavingId = base.SavingID,
                CombinParams = new Collection<CombinParam>
                        {
                            new CombinParam{CombinParamName = combinParamNameS}
                        }
            };
        }
        protected override void InitViewModels()
        {
            base.InitViewModels();
            ActionsWithExtensionsViewModel = new ActionsWithExtensionsViewModel(BaseLayer, ActionErrorCatcher);
            ActionWithExtensionViewModel = new ActionWithExtensionViewModel(CreateActionTemplate(), ActionsWithExtensionsViewModel, ActionErrorCatcher);
            EventsWithExtensionsViewModel = new EventsWithExtensionsViewModel(BaseLayer, EventErrorCatcher);
            EventWithExtensionViewModel = new EventWithExtensionViewModel(CreateEventTemplate(), EventsWithExtensionsViewModel, EventErrorCatcher);
        }
        public override void SolveCP()
        {
             var combinations = BaseLayer.DssDbContext.Combinations.Local;
            foreach (var combination in combinations)
            {
                if (combination.Action.ExtendableAction == null)
                {
                    decimal period = combination.Action.ActionParams.ToList()[0].Value;
                    decimal debit = combination.CombinParams.ToList()[0].Value;
                    decimal credit = combination.Action.ActionParams.ToList()[1].Value;
                    combination.Cp = period * debit - credit;
                }
                else
                {
                    decimal beforeExtendPeriod = combination.Action.ActionParams.ToList()[0].Value;
                    decimal afterExtendPeriod = combination.Action.ExtendableAction.ActionParams.ToList()[0].Value -
                                                beforeExtendPeriod;
                    decimal beforeExtendCredit = combination.Action.ExtendableAction.ActionParams.ToList()[1].Value;
                    decimal afterExtendCredit = combination.Action.ActionParams.ToList()[1].Value;
                    var extendableEvent = BaseLayer.DssDbContext.Events.Local.ToList().First(ev => ev.EventParams.ToList()[0].Value == 1);
                    decimal beforeExtendDebit = combinations.Where(c => c.Action == combination.Action.ExtendableAction &&
                                                c.Event == extendableEvent).Select(c => c).First().CombinParams.ToList()[0].Value;
                    decimal afterExtendDebit = combination.CombinParams.ToList()[0].Value;
                    combination.Cp = (beforeExtendPeriod*beforeExtendDebit - beforeExtendCredit) +
                                     (afterExtendPeriod*afterExtendDebit - afterExtendCredit);
                }
            }
        }

        public override void NextBtnClick_OnPageMain(object sender, System.Windows.RoutedEventArgs e)
        {
            if (TaskParamErrorCatcher.EntityErrorCount != 0) return;
            ContentPage.Content = new PageActionWithExtensionUE { DataContext = this };
            Navigate();
        }
        public override void NextBtnClick_OnPageActions(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ActionErrorCatcher.EntityGroupErrorCount != 0 || ActionsWithExtensionsViewModel.Actions.Count == 0) return;
            ContentPage.Content = new PageEventWithExtensionUE { DataContext = this };
            Navigate();
        }
        public override void PrevBtnClick_OnPageEvents(object sender, System.Windows.RoutedEventArgs e)
        {
            ContentPage.Content = new PageActionWithExtensionUE { DataContext = this };
            Navigate(); 
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
            ContentPage.Content = new PageCombinationWithParamUE { DataContext = this };
            Navigate();
        }
    }
}
