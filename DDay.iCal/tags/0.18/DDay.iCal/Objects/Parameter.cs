using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal.Objects
{
    /// <summary>
    /// A class that provides additional information about a <see cref="ContentLine"/>.
    /// </summary>
    /// <remarks>
    /// <example>
    /// For example, a DTSTART line may look like this: <c>DTSTART;VALUE=DATE:20060116</c>.  
    /// The <c>VALUE=DATE</c> portion is a <see cref="Parameter"/> of the DTSTART value.
    /// </example>
    /// </remarks>
    public class Parameter : iCalObject
    {
        #region Public Fields

        public List<string> Values = new List<string>();

        #endregion

        #region Constructors

        public Parameter(iCalObject parent) : base(parent) { }
        public Parameter(iCalObject parent, string name) : base(parent, name)
        {
            AddToParent();
        }

        #endregion

        #region Public Methods

        public void CopyFrom(object obj)
        {
            if (obj is Parameter)
            {
                Values.Clear();

                Parameter p = (Parameter)obj;
                foreach (string value in p.Values)
                    Values.Add(value);
            }
        }

        #endregion

        #region Protected Methods

        protected void AddToParent()
        {
            if (Parent != null &&                
                Name != null &&
                !Parent.Parameters.ContainsKey(Name))
                Parent.Parameters[Name] = this;
        }        

        #endregion
    }
}
