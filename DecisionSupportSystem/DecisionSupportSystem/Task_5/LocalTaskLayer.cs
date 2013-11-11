using System.Collections.Generic;
using DecisionSupportSystem.DbModel;
using DecisionSupportSystem.MainClasses;
using DecisionSupportSystem.Task_4;
using DecisionSupportSystem.ViewModels;

namespace DecisionSupportSystem.Task_5
{
    public class LocalTaskLayer : TaskCombinationsView
    {
        public EventsDependingActionListViewModel EventsDependingActionListViewModel { get; set; }
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
