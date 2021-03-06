using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal.DataTypes
{
    /// <summary>
    /// Represents an RFC 2445 floating-point decimal value.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class Float : iCalDataType
    {
        #region Private Fields

        private double m_Value;

        #endregion

        #region Public Properties

        public double Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        #endregion

        #region Constructors

        public Float() { }
        public Float(string value)
            : this()
        {
            CopyFrom(Parse(value));
        }

        #endregion

        #region Overrides

        public override void CopyFrom(object obj)
        {
            if (obj is double)
            {
                Float i = (Float)obj;
                Value = i.Value;
            }
            base.CopyFrom(obj);
        }

        public override bool TryParse(string value, ref object obj)
        {   
            double i;
            bool retVal = double.TryParse(value, out i);
            ((Float)obj).Value = i;
            return retVal;
        }

        public override string ToString()
        {
            return Value.ToString("0.######");
        }

        #endregion

        #region Operators

        static public implicit operator double(Float i)
        {
            return i.Value;
        }

        #endregion
    }
}
