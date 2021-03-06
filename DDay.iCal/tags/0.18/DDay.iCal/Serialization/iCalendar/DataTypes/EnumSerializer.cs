using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DDay.iCal.Objects;
using DDay.iCal.DataTypes;

namespace DDay.iCal.Serialization.iCalendar.DataTypes
{
    public class EnumSerializer : ISerializable 
    {
        #region Private Fields

        private Enum m_Enum;

        #endregion

        #region Constructors

        public EnumSerializer(Enum e)
        {
            this.m_Enum = e;
        }

        #endregion

        #region ISerializable Members

        virtual public string SerializeToString()
        {
            return Enum.GetName(m_Enum.GetType(), m_Enum).ToUpper() + "\r\n";
        }

        virtual public void Serialize(Stream stream, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(SerializeToString());
            if (data.Length > 0)
                stream.Write(data, 0, data.Length);
        }        

        #endregion
    }
}
