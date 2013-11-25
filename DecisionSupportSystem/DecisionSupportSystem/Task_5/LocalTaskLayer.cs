using System.Collections.Generic;
using System.Linq;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.Task_4;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_5
{
    public class LocalTaskLayer : TaskCombinationsView, ITaskLayer
    {
        public EventsDependingActionListViewModel EventsDependingActionListViewModel { get; set; }
        public List<Combination> FictiveCombinationsList { get; set; }

        public LocalTaskLayer(BaseLayer baseLayer, EventsDependingActionListViewModel eventsDependingActionListViewModel)
        {
            EventsDependingActionListViewModel = eventsDependingActionListViewModel;
            base.BaseLayer = baseLayer; 
        }

        public override void CreateCombinations()
        {
            LoadCombinations();
            var lastCombList = CreateLastCombinationList();
            var actions = EventsDependingActionListViewModel.EventsDependingActions;
            foreach (var eventsDependingAction in actions)
            {
                var events = eventsDependingAction.EventListViewModel.Events;
                if(events.Count == 0)
                    events.Add(null);
                foreach (var ev in events)
                {
                    if (!HaveAction(eventsDependingAction.Action, lastCombList) || !HaveEvent(ev, lastCombList))
                    {
                        var combination = new Combination();
                        base.BaseLayer.BaseMethods.AddCombination(combination, eventsDependingAction.Action, ev, BaseLayer.Task, 0);
                        var debit = new CombinParam { Combination = combination, };
                        var credit = new CombinParam { Combination = combination, };
                        BaseLayer.BaseMethods.AddCombinationParams(new List<CombinParam> { debit, credit });

                        CombinationWithParamViews.Add(new CombinationWithParamView
                            {
                                Combination = combination,
                                Procent = debit,
                                NominalPrice = credit
                            });
                    }
                }
            }
            CreateFictiveCombinationsList();
        }

        public void CreateFictiveCombinationsList()
        {
            FictiveCombinationsList = new List<Combination>();
            var combins = BaseLayer.DssDbContext.Combinations.Local.ToList();
            foreach (var act in BaseLayer.DssDbContext.Actions.Local)
                foreach (var ev in BaseLayer.DssDbContext.Events.Local)
                {
                    var combination = combins.Select(c => c).Where(c => c.Action == act && c.Event == ev).ToList();
                    if (combination.Count == 0)
                        FictiveCombinationsList.Add(new Combination
                            {
                                Action = act,
                                Event = ev,
                                Cp = 0
                            });
                    else
                        FictiveCombinationsList.Add(combination[0]);
                }
        }

        public override void SolveCp()
        {
            foreach (var combinationWithParamView in CombinationWithParamViews)
            {
                combinationWithParamView.Combination.Cp = combinationWithParamView.Procent.Value -
                                                          combinationWithParamView.NominalPrice.Value;
            }
        }
    }
}
