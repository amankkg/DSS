using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionSupportSystem.DbModel;
using Action = DecisionSupportSystem.DbModel.Action;

namespace DecisionSupportSystem.MainClasses
{
    public interface ITaskSpecific
    {
        Action TemplateAction { get; set; }
        Event TemplateEvent { get; set; }
    }
}
