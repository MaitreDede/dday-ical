using System;
using DDay.iCal.Components;

namespace Example3
{
    /// <summary>
    /// A custom ComponentBase class, which in turn constructs our custom objects
    /// </summary>
    class CustomComponentBase : ComponentBase
    {
        public CustomComponentBase(iCalObject parent) : base(parent) { }
        static public new ComponentBase Create(iCalObject parent, string name)
        {
            switch (name)
            {
                case "VEVENT":
                    // For event objects, use our custom event class
                    return new CustomEvent(parent);                    
                default:
                    // Otherwise, use the default classes
                    return ComponentBase.Create(parent, name);
            }
        }
    }
}
