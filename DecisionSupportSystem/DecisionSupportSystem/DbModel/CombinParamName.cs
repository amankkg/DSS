//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DecisionSupportSystem.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class CombinParamName
    {
        public CombinParamName()
        {
            this.CombinParams = new HashSet<CombinParam>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<CombinParam> CombinParams { get; set; }
    }
}
