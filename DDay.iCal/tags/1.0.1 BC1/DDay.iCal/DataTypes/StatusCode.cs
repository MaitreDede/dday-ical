using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using DDay.iCal.Serialization;
using System.IO;
using DDay.iCal.Serialization.iCalendar;

namespace DDay.iCal
{    
    /// <summary>
    /// An iCalendar status code.
    /// </summary>
#if DATACONTRACT
    [DataContract(Name = "StatusCode", Namespace = "http://www.ddaysoftware.com/dday.ical/2009/07/")]
#endif
    [Serializable]
    public class StatusCode : 
        EncodableDataType,
        IStatusCode
    {
        #region Private Fields

        private int[] m_Parts;

        #endregion

        #region Public Properties

#if DATACONTRACT
        [DataMember(Order = 1)]
#endif
        public int[] Parts
        {
            get { return m_Parts; }
            set { m_Parts = value; }
        }

        public int Primary
        {
            get
            {
                if (m_Parts.Length > 0)
                    return m_Parts[0];
                return 0;
            }
        }

        public int Secondary
        {
            get
            {
                if (m_Parts.Length > 1)
                    return m_Parts[1];
                return 0;
            }
        }

        public int Tertiary
        {
            get
            {
                if (m_Parts.Length > 2)
                    return m_Parts[2];
                return 0;
            }
        }

        #endregion

        #region Constructors

        public StatusCode() { }
        public StatusCode(string value)
            : this()
        {
            StatusCodeSerializer serializer = new StatusCodeSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        #endregion

        #region Overrides

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);
            if (obj is IStatusCode)
            {
                IStatusCode sc = (IStatusCode)obj;
                Parts = new int[sc.Parts.Length];
                sc.Parts.CopyTo(Parts, 0);
            }
        }

        public override string ToString()
        {
            StatusCodeSerializer serializer = new StatusCodeSerializer();
            return serializer.SerializeToString(this);
        }

        #endregion
    }
}
