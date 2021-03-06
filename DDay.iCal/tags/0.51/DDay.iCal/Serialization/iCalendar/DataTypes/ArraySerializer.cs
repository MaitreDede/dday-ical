using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;

namespace DDay.iCal.Serialization.iCalendar.DataTypes
{
    public class ArraySerializer : ISerializable 
    {
        #region Private Fields

        private Array m_Array;

        #endregion

        #region Constructors

        public ArraySerializer(Array array)
        {
            this.m_Array = array;
        }

        #endregion

        #region ISerializable Members

        virtual public string SerializeToString()
        {
            return string.Empty;
        }

        virtual public void Serialize(Stream stream, Encoding encoding)
        {
            foreach (object obj in m_Array)
            {
                ISerializable serializer = SerializerFactory.Create(obj);
                if (serializer != null)
                    serializer.Serialize(stream, encoding);
            }
        }

        public iCalObject Deserialize(Stream stream, Encoding encoding, Type iCalendarType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion        
    }
}
