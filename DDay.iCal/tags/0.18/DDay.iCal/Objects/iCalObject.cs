using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DDay.iCal.DataTypes;
using DDay.iCal.Serialization;

namespace DDay.iCal.Objects
{
    /// <summary>
    /// The base class for all iCalendar objects, components, and data types.
    /// </summary>
    public class iCalObject
    {
        #region Public Fields

        public iCalObject Parent = null;

        #endregion

        #region Public Events

        public event EventHandler Load;

        #endregion

        #region Private Fields

        private ArrayList m_Children = new ArrayList();
        private string m_name;
        private Hashtable m_Properties = new Hashtable();
        private Hashtable m_Parameters = new Hashtable();

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a list of properties that are associated with the iCalendar object.
        /// </summary>
        public Hashtable Properties
        {
            get { return m_Properties; }
            set { m_Properties = value; }
        }

        /// <summary>
        /// Returns a list of parameters that are associated with the iCalendar object.
        /// </summary>
        public Hashtable Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }

        /// <summary>
        /// A collection of <see cref="iCalObject"/>s that are children 
        /// of the current object.
        /// </summary>
        public ArrayList Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="iCalObject"/>.  For iCalendar components,
        /// this is the RFC 2445 name of the component.
        /// <example>
        ///     <list type="bullet">
        ///         <item>Event - "VEVENT"</item>
        ///         <item>Todo - "VTODO"</item>
        ///         <item>TimeZone - "VTIMEZONE"</item>
        ///         <item>etc.</item>
        ///     </list>
        /// </example>
        /// </summary>        
        virtual public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Returns the <see cref="DDay.iCal.iCalendar"/> that this <see cref="iCalObject"/>
        /// belongs to.
        /// </summary>
        public iCalendar iCalendar
        {
            get
            {
                iCalObject obj = this;
                while (obj.Parent != null)
                    obj = obj.Parent;

                if (obj is iCalendar)
                    return obj as iCalendar;
                return null;
            }
        }

        #endregion

        #region Constructors

        internal iCalObject() { }
        public iCalObject(iCalObject parent)
        {
            Parent = parent;
            if (parent != null)
            {
                if (!(this is Property) &&
                    !(this is Parameter))
                    parent.AddChild(this);
            }
        }
        public iCalObject(iCalObject parent, string name)
            : this(parent)
        {
            Name = name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an <see cref="iCalObject"/>-based object as a child
        /// of the current object.
        /// </summary>
        /// <param name="child">The <see cref="iCalObject"/>-based object to add.</param>
        virtual public void AddChild(iCalObject child)
        {
            Children.Add(child);
        }

        /// <summary>
        /// Removed an <see cref="iCalObject"/>-based object from the <see cref="Children"/>
        /// collection.
        /// </summary>
        /// <param name="child"></param>
        virtual public void RemoveChild(iCalObject child)
        {
            if (Children.Contains(child))
                Children.Remove(child);
        }

        /// <summary>
        /// For iCalendar components, automatically finds and retrieves fields that
        /// match the field specified in the <see cref="ContentLine"/>, and sets
        /// their value.
        /// <example>
        /// For example, if a public DTStart field exists in the specified component,
        /// (i.e. <c>public Date_Time DTStart;</c>)
        /// and a content line of <c>DTSTART;TZID=US-Eastern:20060830T090000</c> is
        /// encountered, this method will automatically set the value of the
        /// DTStart field to Aug. 30, 2006, 9:00 AM in the US-Eastern TimeZone.
        /// </example>
        /// <note>
        ///     It should not be necessary to invoke this method manually as it
        ///     is handled automatically during the iCalendar parsing.
        /// </note>
        /// </summary>
        /// <param name="cl">The <see cref="ContentLine"/> to process.</param>
        virtual public void SetContentLineValue(ContentLine cl)
        {
            if (cl.Name != null)
            {
                string name = cl.Name.Replace("-","");
                Type type = GetType();

                //
                // Find the public field that matches the name of our content line (ignoring case)
                //
                FieldInfo field = type.GetField(name, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.Static);
                PropertyInfo property = null;
                
                if (field == null)
                    property = type.GetProperty(name, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Static);

                if (field != null ||
                    property != null)
                {
                    // Get the field/property's value
                    object value = field == null ? property.GetValue(this, null) : field.GetValue(this);
                    Type itemType = field == null ? property.PropertyType : field.FieldType;
                    object[] itemAttributes = field == null ? property.GetCustomAttributes(true) : field.GetCustomAttributes(true);

                    Type elementType = itemType.IsArray ? itemType.GetElementType() : itemType;

                    // If it's an iCalDataType, or an array of iCalDataType, then let's fill it!
                    if (itemType.IsSubclassOf(typeof(iCalDataType)) ||
                        (itemType.IsArray && itemType.GetElementType().IsSubclassOf(typeof(iCalDataType))))
                    {
                        iCalDataType icdt = null;
                        if (!itemType.IsArray)
                            icdt = (iCalDataType)value;
                        if (icdt == null)
                            icdt = (iCalDataType)Activator.CreateInstance(elementType);

                        // Assign custom attributes for the specific field
                        icdt.Attributes = itemAttributes;

                        // Set the content line for the object.  On most objects, this
                        // triggers the object to parse the content line with Parse().
                        icdt.ContentLine = cl;

                        // It's an array, let's add an item to the end
                        if (itemType.IsArray)
                        {
                            ArrayList arr = new ArrayList();
                            if (value != null)
                                arr.AddRange((ICollection)value);
                            arr.Add(icdt);
                            if (field != null)
                                field.SetValue(this, arr.ToArray(elementType));
                            else
                                property.SetValue(this, arr.ToArray(elementType), null);
                        }
                        // Otherwise, set the value directly!
                        else 
                        {
                            if (field != null)
                                field.SetValue(this, icdt);
                            else property.SetValue(this, icdt, null);
                        }
                    }
                    else
                    {
                        FieldInfo minValue = itemType.GetField("MinValue");
                        object minVal = (minValue != null) ? minValue.GetValue(null) : null;

                        if (itemType.IsArray)
                        {
                            ArrayList arr = new ArrayList();
                            if (value != null)
                                arr.AddRange((ICollection)value);
                            arr.Add(cl.Value);

                            if (field != null)
                                field.SetValue(this, arr.ToArray(elementType));
                            else property.SetValue(this, arr.ToArray(elementType), null);
                        }
                        // Always assign enum values
                        else if (itemType.IsEnum)
                        {
                            if (field != null)
                                field.SetValue(this, Enum.Parse(itemType, cl.Value.Replace("-", "_")));
                            else property.SetValue(this, Enum.Parse(itemType, cl.Value.Replace("-", "_")), null);
                        }
                        // Otherwise, set the value directly!
                        else if (value == null || value.Equals(minVal))
                        {
                            if (field != null)
                                field.SetValue(this, cl.Value);
                            else property.SetValue(this, cl.Value, null);
                        }
                        else ;// FIXME: throw single-value exception, if "strict" parsing is enabled
                    }
                }
                else
                {
                    // This is a non-standard property.  Let's load it into memory,
                    // So we can serialize it later
                    Property p = new Property(cl);
                    p.AddToParent();
                }
            }
        }

        /// <summary>
        /// Invokes the <see cref="Load"/> event handler when the object has been fully loaded.
        /// This is automatically called when processing objects that inherit from 
        /// <see cref="ComponentBase"/> (i.e. all iCalendar components).
        /// </summary>
        virtual public void OnLoad(EventArgs e)
        {
            if (this.Load != null)
                this.Load(this, e);
        }

        #endregion
    }
}
