using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal.DataTypes
{
    /// <summary>
    /// Status codes available to a <see cref="Todo"/> item.
    /// </summary>    
    public enum TodoStatus
    {        
        NEEDS_ACTION,
        COMPLETED,
        IN_PROCESS,
        CANCELLED
    };
}
