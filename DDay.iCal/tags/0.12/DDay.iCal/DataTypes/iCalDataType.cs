using System;
using System.Collections;
using System.Text;
using System.Reflection;
using DDay.iCal.Objects;

namespace DDay.iCal.DataTypes
{
    public class iCalDataType
    {
        #region Private Fields

        private ContentLine m_ContentLine = null;

        #endregion

        #region Overridable Properties & Methods

        virtual public ContentLine ContentLine
        {
            get { return m_ContentLine; }
            set { m_ContentLine = value; }
        }

        virtual public void CopyFrom(object obj) { }
        virtual public bool TryParse(string value, ref object obj) { return false; }
        
        virtual public object Parse(string value)
        {
            Type t = GetType();
            object obj = Activator.CreateInstance(t);
            if (!TryParse(value, ref obj))
                throw new ArgumentException(t.Name + ".Parse cannot parse the value '" + value + "' because it is not formatted correctly.");
            return obj;
        }

        #endregion

        #region Content Validation

        public void CheckRange(string name, ICollection values, int min, int max)
        {
            bool allowZero = (min == 0 || max == 0) ? true : false;
            foreach(int value in values)
                CheckRange(name, value, min, max, allowZero);
        }
        public void CheckRange(string name, int value, int min, int max)
        {
            CheckRange(name, value, min, max, (min == 0 || max == 0) ? true : false);
        }
        public void CheckRange(string name, int value, int min, int max, bool allowZero)
        {
            if (value != int.MinValue && (value < min || value > max || (!allowZero && value == 0)))
                throw new ArgumentException(name + " value " + value + " is out of range. Valid values are between " + min + " and " + max + (allowZero ? "" : ", excluding zero (0)") + ".");
        }

        public void CheckMutuallyExclusive(string name1, string name2, object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null)
                return;
            else
            {
                bool has1 = false,
                    has2 = false;

                Type t1 = obj1.GetType(),
                    t2 = obj2.GetType();

                FieldInfo fi1 = t1.GetField("MinValue");
                FieldInfo fi2 = t1.GetField("MinValue");

                has1 = fi1 == null || !obj1.Equals(fi1.GetValue(null));
                has2 = fi2 == null || !obj2.Equals(fi2.GetValue(null));
                if (has1 && has2)
                    throw new ArgumentException("Both " + name1 + " and " + name2 + " cannot be supplied together; they are mutually exclusive.");
            }
        }

        #endregion
    }
}
