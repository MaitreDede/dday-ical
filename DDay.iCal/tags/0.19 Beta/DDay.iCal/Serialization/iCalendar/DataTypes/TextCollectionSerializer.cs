using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DDay.iCal.DataTypes;
using DDay.iCal.Objects;

namespace DDay.iCal.Serialization.iCalendar.DataTypes
{
    public class TextCollectionSerializer : FieldSerializer 
    {
        #region Private Fields

        private TextCollection m_TC;

        #endregion

        #region Constructors

        public TextCollectionSerializer(TextCollection tc)
            : base(tc)
        {
            this.m_TC = tc;
        }

        #endregion

        #region ISerializable Members

        public override string SerializeToString()
        {
            string value = string.Empty;
            int i = 0;
            foreach (Text text in m_TC.Values)
            {
                ISerializable serializer = new TextSerializer(text);
                if (serializer != null)
                {
                    if (i++ > 0)                
                        value += ",";
                    value += serializer.SerializeToString();
                }
            }

            return value;
        }

        #endregion
    }
}
