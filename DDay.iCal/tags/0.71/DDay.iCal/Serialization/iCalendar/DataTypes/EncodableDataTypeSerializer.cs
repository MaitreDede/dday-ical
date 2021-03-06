using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;
using System.Text.RegularExpressions;
using DDay.iCal.Serialization.iCalendar.Components;

namespace DDay.iCal.Serialization.iCalendar.DataTypes
{
    public class EncodableDataTypeSerializer : FieldSerializer
    {
        #region Private Fields

        private EncodableDataType _DataType;

        #endregion

        #region Constructors

        public EncodableDataTypeSerializer(EncodableDataType dataType) : base(dataType)
        {
            _DataType = dataType;
        }

        #endregion

        #region Protected Methods

        protected string Encode(string value)
        {
            UTF8Encoding encoding = new UTF8Encoding();

            switch (_DataType.Encoding)
            {
                case "BASE64": return Convert.ToBase64String(encoding.GetBytes(value));
                case "7BIT":
                case "8BIT":
                    value = Regex.Replace(value, @"[^\r]\n", "\r\n");
                    value = Regex.Replace(value, @"\r[^\n]", "\r\n");                    
                    
                    bool is7Bit = _DataType.Encoding.Equals("7BIT");

                    List<byte> data = new List<byte>(encoding.GetBytes(value));
                    for (int i = data.Count - 1; i >= 0; i--)
                    {
                        if (data[i] == 0)
                            data.RemoveAt(i);

                        if (is7Bit && data[i] > 127)
                            data.RemoveAt(i);
                    }

                    return encoding.GetString(data.ToArray());
                default:
                    return value;
            }
        }

        protected string Encode(byte[] data)
        {
            // NOTE: we can only properly serialize a byte array
            // into BASE64 - anything else will have the chance
            // of data loss.
            switch (_DataType.Encoding)
            {
                case "BASE64": return Convert.ToBase64String(data);
                default:
                    return null;
            }
        }

        #endregion

        #region Overrides

        public override string SerializeToString()
        {
            return Encode(DataType.ToString());
        }

        public override void Serialize(Stream stream, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(SerializeToString());
            if (data.Length > 0)
                stream.Write(data, 0, data.Length);
        }

        #endregion
    }
}
