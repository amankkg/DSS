using System.Collections.Generic;
using DecisionSupportSystem.DbModel;

namespace DecisionSupportSystem.MainClasses
{
    // Данный класс описывает действие, которое имеет несколько зависимых от него событий 
    public class ActionForSecondType : Action
    {
        public List<Event> Events { get; set; }

        public ActionForSecondType()
        {
            Events = new List<Event>();
        }
    }
}
