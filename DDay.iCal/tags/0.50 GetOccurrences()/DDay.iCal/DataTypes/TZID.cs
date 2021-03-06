using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DDay.iCal.DataTypes
{    
    /// <summary>
    /// A time zone identifier, used to associate <see cref="Date_Time"/> (and other) objects
    /// with a specific time zone.
    /// </summary>
    public class TZID : iCalDataType
    {
        #region Private Fields

        private bool m_GloballyUnique = false;
        private string m_ID = string.Empty;
                
        #endregion

        #region Public Properties

        public bool GloballyUnique
        {
            get { return m_GloballyUnique; }
            set { m_GloballyUnique = value; }
        }

        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }


        #endregion

        #region Constructors

        public TZID() { }
        public TZID(string value)
            : this()
        {
            CopyFrom((TZID)Parse(value));
        }

        #endregion

        #region Overrides

        public override void CopyFrom(object obj)
        {
            base.CopyFrom(obj);
            if (obj is TZID)
            {
                TZID tzid = (TZID)obj;
                this.GloballyUnique = tzid.GloballyUnique;
                this.ID = tzid.ID;
            }
            base.CopyFrom(obj);
        }

        public override bool TryParse(string value, ref object obj)
        {
            TZID tzid = (TZID)obj;

            Match match = Regex.Match(value, @"(/)?([^\r]+)");
            if (match.Success)
            {
                if (match.Groups[1].Success)
                    tzid.GloballyUnique = true;
                tzid.ID = match.Groups[2].Value;
                return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is TZID)
            {
                TZID tzid = (TZID)obj;
                return (this.GloballyUnique.Equals(tzid.GloballyUnique) && this.ID.Equals(tzid.ID));
            }
            else if (obj is string)
            {
                object tzid = new TZID();
                if (((TZID)tzid).TryParse(obj.ToString(), ref tzid))
                    return tzid.Equals(this);                
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GloballyUnique.GetHashCode() ^ ID.GetHashCode();
        }

        public override string ToString()
        {
            return (GloballyUnique ? "/" : "") + ID;
        }

        #endregion

        #region Operators

        static public implicit operator TZID(string input)
        {
            return new TZID(input);
        }

        #endregion
    }
}
