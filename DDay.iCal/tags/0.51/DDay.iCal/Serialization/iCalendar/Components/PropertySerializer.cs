using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DDay.iCal.DataTypes;
using DDay.iCal.Components;

namespace DDay.iCal.Serialization.iCalendar.Components
{
    public class PropertySerializer : ISerializable, IParameterSerializable
    {
        #region Private Fields

        private Property m_Property;

        #endregion

        #region Constructors

        public PropertySerializer(Property property)
        {
            this.m_Property = property;
        }

        #endregion

        #region ISerializable Members

        public string SerializeToString()
        {
            string value = m_Property.Name;
            if (m_Property.Parameters.Count > 0)
            {
                List<string> parameters = new List<string>();

                // NOTE: fixed a bug where the SerializeToString() did not function properly
                // It used the following line instead:
                // foreach (Parameter p in m_Property.Parameters)
                // { ...
                // }
                //
                // Since m_Property.Parameters is a Hashtable, this would always fail.
                foreach (DictionaryEntry de in m_Property.Parameters)
                {
                    Parameter p = de.Value as Parameter;
                    if (p != null)
                        parameters.Add(p.Name + "=" + string.Join(",", p.Values.ToArray()));
                }

                value += ";" + string.Join(";", parameters.ToArray());
            }
            return value + ":" + m_Property.Value + "\r\n";
        }

        public void Serialize(Stream stream, Encoding encoding)
        {
            byte[] prop = encoding.GetBytes(SerializeToString());
            stream.Write(prop, 0, prop.Length);
        }

        public iCalObject Deserialize(Stream stream, Encoding encoding, Type iCalendarType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IParameterSerializable Members

        public List<Parameter> Parameters
        {
            get
            {
                List<Parameter> Parameters = new List<Parameter>();
                foreach (DictionaryEntry de in m_Property.Parameters)
                {
                    if (!DisallowedParameters.Contains(de.Value as Parameter))
                        Parameters.Add(de.Value as Parameter);
                }
                return Parameters;
            }
        }

        public List<Parameter> DisallowedParameters
        {
            get { return new List<Parameter>(); }
        }

        #endregion        
    }
}
