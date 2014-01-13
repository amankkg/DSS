using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.PageUserElements;
using DecisionSupportSystem.ViewModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.Tasks
{
    public class TaskNumberTwelve : TaskNumberFive
    {
        protected override Action CreateActionTemplate()
        {
            return new Action
            {
                Name = "Действие",
                SavingId = SavingID,
                ActionParams = new Collection<ActionParam>{new ActionParam
                {
                    ActionParamName = new ActionParamName{Name = "Стоимость:"}
                }}
            };
        }

        protected override Event CreateEventTemplate()
        {
            return new Event
            {
                Name = "Событие",
                Probability = 1,
                SavingId = SavingID,
                EventParams = new Collection<EventParam>
                    {
                        new EventParam{EventParamName = new EventParamName { Name = "Доход:" }},
                        new EventParam{EventParamName = new EventParamName { Name = "Затрата:" }}
                    }
            };
        }

        protected override Combination CreateCombinationTemplate()
        {
            return new Combination { SavingId = this.SavingID };
        }

        protected override void InitViewModels()
        {
            ActionsViewModel = new ActionsViewModel(DssDbEntities.Actions.Local, ActionErrorCatcher)
            {ParamsVisibility = Visibility.Visible};
            ActionViewModel = new ActionViewModel(CreateActionTemplate(), ActionsViewModel, ActionErrorCatcher)
            {ParamsVisibility = Visibility.Visible, randomMax = 10000};
            EventsDepActionsViewModel = new EventsDepActionsViewModel(DssDbEntities, EventErrorCatcher)
            {ParamsVisibility = Visibility.Visible};
            EventDepActionViewModel = new EventDepActionViewModel(DssDbEntities.Actions.Local, CreateEventTemplate(), EventsDepActionsViewModel, EventErrorCatcher)
                {ParamsVisibility = Visibility.Visible, randomMax = 50000};
        }

        protected override void InitCombinationViewModel()
        {
            CombinationsViewModel = new CombinationsViewModel(DssDbEntities.Combinations.Local, CombinationErrorCatcher){ParamsVisibility = Visibility.Hidden};
        }

        public override void SolveCp()
        {
            var combinations = DssDbEntities.Combinations.Local;
            foreach (var combination in combinations)
            {
                var cost = combination.Action.ActionParams.ToList()[0].Value;
                var revenue = combination.Event.EventParams.ToList()[0].Value;
                var loss = combination.Event.EventParams.ToList()[1].Value;
                combination.Cp = revenue - cost - loss;
            }
        }

    }
}
