using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;
using DDay.iCal.Objects;

namespace DDay.iCal
{
    /// <summary>
    /// A class that represents an iCalendar object.  To load an iCalendar object, generally a
    /// static LoadFromXXX method is used.
    /// <example>
    ///     For example, use the following code to load an iCalendar object from a URL:
    ///     <code>
    ///        iCalendar iCal = iCalendar.LoadFromUri(new Uri("http://somesite.com/calendar.ics"));
    ///     </code>
    /// </example>
    /// Once created, an iCalendar object can be used to gather relevant information about
    /// events, todos, time zones, journal entries, and free/busy time.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following is an example of loading an iCalendar and displaying a text-based calendar.
    /// 
    /// <code>
    /// //
    /// // The following code loads and displays an iCalendar 
    /// // with US Holidays for 2006.
    /// //
    /// iCalendar iCal = iCalendar.LoadFromUri(new Uri("http://www.applegatehomecare.com/Calendars/USHolidays.ics"));
    /// iCal.Evaluate(
    ///     new Date_Time(2006, 1, 1, "US-Eastern", iCal),
    ///     new Date_Time(2006, 12, 31, "US-Eastern", iCal));
    /// 
    /// Date_Time dt = new Date_Time(2006, 1, 1, "US-Eastern", iCal);
    /// while (dt.Year == 2006)
    /// {
    ///     // First, display the current date we're evaluating
    ///     Console.WriteLine(dt.Local.ToShortDateString());
    /// 
    ///     // Then, iterate through each event in our iCalendar
    ///     foreach (Event evt in iCal.Events)
    ///     {
    ///         // Determine if the event occurs on the specified date
    ///         if (evt.OccursOn(dt))
    ///         {
    ///             // Display the event summary
    ///             Console.Write("\t" + evt.Summary);
    /// 
    ///             // Display the time the event happens (unless it's an all-day event)
    ///             if (evt.Start.HasTime)
    ///             {
    ///                 Console.Write(" (" + evt.Start.Local.ToShortTimeString() + " - " + evt.End.Local.ToShortTimeString());
    ///                 if (evt.Start.TimeZoneInfo != null)
    ///                     Console.Write(" " + evt.Start.TimeZoneInfo.TimeZoneName);
    ///                 Console.Write(")");
    ///             }
    /// 
    ///             Console.Write(Environment.NewLine);
    ///         }
    ///     }
    /// 
    ///     // Move to the next day
    ///     dt = dt.AddDays(1);
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// The following example loads all active to-do items from an iCalendar:
    /// 
    /// <code>
    /// //
    /// // The following code loads and displays active todo items from an iCalendar
    /// // for January 6th, 2006.
    /// //
    /// iCalendar iCal = iCalendar.LoadFromUri(new Uri("http://somesite.com/calendar.ics"));
    /// iCal.Evaluate(
    ///     new Date_Time(2006, 1, 1, "US-Eastern", iCal),
    ///     new Date_Time(2006, 1, 31, "US-Eastern", iCal));
    /// 
    /// Date_Time dt = new Date_Time(2006, 1, 6, "US-Eastern", iCal);
    /// foreach(Todo todo in iCal.Todos)
    /// {
    ///     if (todo.IsActive(dt))
    ///     {
    ///         // Display the todo summary
    ///         Console.WriteLine(todo.Summary);
    ///     }
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public class iCalendar : ComponentBase, IDisposable
    {
        #region Readonly Fields

        static private readonly string _Version = "2.0";
        static private readonly string _ProdID = "-//DDay.iCal//NONSGML ddaysoftware.com//EN";

        #endregion

        #region Constructors

        /// <summary>
        /// To load an existing an iCalendar object, use one of the provided LoadFromXXX methods.
        /// <example>
        /// For example, use the following code to load an iCalendar object from a URL:
        /// <code>
        ///     iCalendar iCal = iCalendar.LoadFromUri(new Uri("http://somesite.com/calendar.ics"));
        /// </code>
        /// </example>
        /// </summary>
        public iCalendar() : base(null)
        {
            this.Name = "VCALENDAR";
            UniqueComponents = new UniqueComponentList<UniqueComponent>(this);
            Events = new UniqueComponentList<Event>(this);
            FreeBusy = new List<FreeBusy>();
            Journals = new UniqueComponentList<Journal>(this);
            TimeZones = new List<DDay.iCal.Components.TimeZone>();
            Todos = new UniqueComponentList<Todo>(this);

            // Set default values for these required properties
            // NOTE: fixes bug #1672047
            Version = _Version;
            ProductID = _ProdID;
            
            object[] attrs = GetType().GetCustomAttributes(typeof(ComponentBaseTypeAttribute), false);
            if (attrs.Length > 0)
            {
                foreach (ComponentBaseTypeAttribute attr in attrs)
                    m_ComponentBaseCreate = attr.Type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            }
            
            if (m_ComponentBaseCreate == null)
                m_ComponentBaseCreate = typeof(ComponentBase).GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);            
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Adds an <see cref="iCalObject"/>-based component to the
        /// appropriate collection.  Currently, the iCalendar component
        /// supports the following components:
        ///     <list type="bullet">        
        ///         <item><see cref="Event"/></item>
        ///         <item><see cref="FreeBusy"/></item>
        ///         <item><see cref="Journal"/></item>
        ///         <item><see cref="DDay.iCal.Components.TimeZone"/></item>
        ///         <item><see cref="Todo"/></item>
        ///     </list>
        /// </summary>
        /// <param name="child"></param>
        public override void AddChild(iCalObject child)
        {
            base.AddChild(child);
            child.Parent = this;

            if (child is UniqueComponent)
                UniqueComponents.Add((UniqueComponent)child);

            Type type = child.GetType();
            if (type == typeof(Event) || type.IsSubclassOf(typeof(Event))) Events.Add((Event)child);
            else if (type == typeof(FreeBusy) || type.IsSubclassOf(typeof(FreeBusy))) FreeBusy.Add((FreeBusy)child);
            else if (type == typeof(Journal) || type.IsSubclassOf(typeof(Journal))) Journals.Add((Journal)child);
            else if (type == typeof(DDay.iCal.Components.TimeZone) || type.IsSubclassOf(typeof(DDay.iCal.Components.TimeZone))) TimeZones.Add((DDay.iCal.Components.TimeZone)child);
            else if (type == typeof(Todo) || type.IsSubclassOf(typeof(Todo))) Todos.Add((Todo)child);
        }

        /// <summary>
        /// Removes an <see cref="iCalObject"/>-based component from all
        /// of the collections that this object may be contained in within
        /// the iCalendar.
        /// </summary>
        /// <param name="child"></param>
        public override void RemoveChild(iCalObject child)
        {
            base.RemoveChild(child);

            if (child is UniqueComponent)
                UniqueComponents.Remove((UniqueComponent)child);

            Type type = child.GetType();
            if (type == typeof(Event) || type.IsSubclassOf(typeof(Event))) Events.Remove((Event)child);
            else if (type == typeof(FreeBusy) || type.IsSubclassOf(typeof(FreeBusy))) FreeBusy.Remove((FreeBusy)child);
            else if (type == typeof(Journal) || type.IsSubclassOf(typeof(Journal))) Journals.Remove((Journal)child);
            else if (type == typeof(DDay.iCal.Components.TimeZone) || type.IsSubclassOf(typeof(DDay.iCal.Components.TimeZone))) TimeZones.Remove((DDay.iCal.Components.TimeZone)child);
            else if (type == typeof(Todo) || type.IsSubclassOf(typeof(Todo))) Todos.Remove((Todo)child);
        }

        /// <summary>
        /// Resolves each UID in the UniqueComponents list
        /// to a valid UID.  When the UIDs are updated, each
        /// UniqueComponentList that contains the UniqueComponent
        /// will be updated as well.
        /// </summary>
        public override void OnLoad(EventArgs e)
        {
            UniqueComponents.ResolveUIDs();

            base.OnLoad(e);            
        }

        /// <summary>
        /// Creates a typed copy of the iCalendar.
        /// </summary>
        /// <returns>An iCalendar object.</returns>
        public iCalendar Copy()
        {
            return (iCalendar)base.Copy();
        }
        
        #endregion

        #region Private Fields

        private UniqueComponentList<UniqueComponent> m_UniqueComponents;
        private UniqueComponentList<Event> m_Events;
        private List<FreeBusy> m_FreeBusy;
        private UniqueComponentList<Journal> m_Journal;
        private List<DDay.iCal.Components.TimeZone> m_TimeZone;
        private UniqueComponentList<Todo> m_Todo;
        private MethodInfo m_ComponentBaseCreate;

        // The buffer size used to convert streams from UTF-8 to Unicode
        private const int bufferSize = 8096;

        #endregion

        #region Public Properties

        public UniqueComponentList<UniqueComponent> UniqueComponents
        {
            get { return m_UniqueComponents; }
            set { m_UniqueComponents = value; }
        }

        /// <summary>
        /// A collection of <see cref="Event"/> components in the iCalendar.
        /// </summary>
        public UniqueComponentList<Event> Events
        {
            get { return m_Events; }
            set { m_Events = value; }
        }

        /// <summary>
        /// A collection of <see cref="DDay.iCal.Components.FreeBusy"/> components in the iCalendar.
        /// </summary>
        public List<FreeBusy> FreeBusy
        {
            get { return m_FreeBusy; }
            set { m_FreeBusy = value; }
        }
        
        /// <summary>
        /// A collection of <see cref="Journal"/> components in the iCalendar.
        /// </summary>
        public UniqueComponentList<Journal> Journals
        {
            get { return m_Journal; }
            set { m_Journal = value; }
        }

        /// <summary>
        /// A collection of <see cref="DDay.iCal.Components.TimeZone"/> components in the iCalendar.
        /// </summary>
        public List<DDay.iCal.Components.TimeZone> TimeZones
        {
            get { return m_TimeZone; }
            set { m_TimeZone = value; }
        }

        /// <summary>
        /// A collection of <see cref="Todo"/> components in the iCalendar.
        /// </summary>
        public UniqueComponentList<Todo> Todos
        {
            get { return m_Todo; }
            set { m_Todo = value; }
        }

        public string Version
        {
            get
            {
                if (Properties.ContainsKey("VERSION"))
                    return ((Property)Properties["VERSION"]).Value;
                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Properties["VERSION"] = new Property(this, "VERSION", _Version);
                else Properties["VERSION"] = new Property(this, "VERSION", value);
            }
        }

        public string ProductID
        {
            get
            {
                if (Properties.ContainsKey("PRODID"))
                    return ((Property)Properties["PRODID"]).Value;
                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Properties["PRODID"] = new Property(this, "PRODID", _ProdID);
                else Properties["PRODID"] = new Property(this, "PRODID", value);
            }            
        }

        public string Scale
        {
            get
            {
                if (Properties.ContainsKey("CALSCALE"))
                    return ((Property)Properties["CALSCALE"]).Value;
                return null;
            }
            set
            {                
                Properties["CALSCALE"] = new Property(this, "CALSCALE", value);
            }             
        }

        public string Method
        {
            get
            {
                if (Properties.ContainsKey("METHOD"))
                    return ((Property)Properties["METHOD"]).Value;
                return null;
            }
            set
            {
                Properties["METHOD"] = new Property(this, "METHOD", value);
            }
        }

        #endregion

        #region Static Public Methods

        /// <summary>
        /// Loads an <see cref="iCalendar"/> from the file system.
        /// </summary>
        /// <param name="Filepath">The path to the file to load.</param>
        /// <returns>An <see cref="iCalendar"/> object</returns>
        static public iCalendar LoadFromFile(string Filepath) { return LoadFromFile(typeof(iCalendar), Filepath); }
        static public iCalendar LoadFromFile(Type iCalendarType, string Filepath)
        {            
            FileStream fs = new FileStream(Filepath, FileMode.Open);

            iCalendar iCal = LoadFromStream(iCalendarType, fs);
            fs.Close();
            return iCal;
        }

        /// <summary>
        /// Loads an <see cref="iCalendar"/> from an open stream.
        /// </summary>
        /// <param name="s">The stream from which to load the <see cref="iCalendar"/> object</param>
        /// <returns>An <see cref="iCalendar"/> object</returns>
        static public iCalendar LoadFromStream(Stream s) { return LoadFromStream(typeof(iCalendar), s); }
        static public iCalendar LoadFromStream(Type iCalendarType, Stream s)
        {            
            TextReader tr = new StreamReader(s, Encoding.UTF8);

            // Create a lexer for our memory stream
            iCalLexer lexer = new iCalLexer(tr);
            iCalParser parser = new iCalParser(lexer);

            // Determine the calendar type we'll be using when constructing
            // iCalendar objects...
            parser.iCalendarType = iCalendarType;

            // Parse the iCalendar!
            iCalendar iCal = parser.icalobject();

            // Close our memory stream
            tr.Close();

            // Return the parsed iCalendar
            return iCal;
        }

        /// <summary>
        /// Loads an <see cref="iCalendar"/> from a given Uri.
        /// </summary>
        /// <param name="url">The Uri from which to load the <see cref="iCalendar"/> object</param>
        /// <returns>An <see cref="iCalendar"/> object</returns>
        static public iCalendar LoadFromUri(Uri uri) { return LoadFromUri(typeof(iCalendar), uri); }
        static public iCalendar LoadFromUri(Type iCalendarType, Uri uri)
        {
            return LoadFromUri(uri, null, null);            
        }

        /// <summary>
        /// Loads an <see cref="iCalendar"/> from a given Uri, using a 
        /// specified <paramref name="username"/> and <paramref name="password"/>
        /// for credentials.
        /// </summary>
        /// <param name="url">The Uri from which to load the <see cref="iCalendar"/> object</param>
        /// <returns>an <see cref="iCalendar"/> object</returns>
        static public iCalendar LoadFromUri(Uri uri, string username, string password) { return LoadFromUri(typeof(iCalendar), uri, username, password); }
        static public iCalendar LoadFromUri(Type iCalendarType, Uri uri, string username, string password)
        {
            try
            {
                WebClient client = new WebClient();
                if (username != null &&
                    password != null)
                    client.Credentials = new System.Net.NetworkCredential(username, password);

                byte[] bytes = client.DownloadData(uri);
                MemoryStream ms = new MemoryStream();
                ms.SetLength(bytes.Length);
                bytes.CopyTo(ms.GetBuffer(), 0);

                return LoadFromStream(iCalendarType, ms);
            }
            catch (System.Net.WebException ex)
            {
                return null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the <see cref="DDay.iCal.Components.TimeZone" /> object for the specified
        /// <see cref="TZID"/> (Time Zone Identifier).
        /// </summary>
        /// <param name="tzid">A valid <see cref="TZID"/> object, or a valid <see cref="TZID"/> string.</param>
        /// <returns>A <see cref="TimeZone"/> object for the <see cref="TZID"/>.</returns>
        public DDay.iCal.Components.TimeZone GetTimeZone(TZID tzid)
        {
            foreach (DDay.iCal.Components.TimeZone tz in TimeZones)
            {
                if (tz.TZID.Equals(tzid))
                {
                    return tz;
                }
            }
            return null;
        }

        /// <summary>
        /// Evaluates component recurrences for the given range of time.
        /// <example>
        ///     For example, if you are displaying a month-view for January 2007,
        ///     you would want to evaluate recurrences for Jan. 1, 2007 to Jan. 31, 2007
        ///     to display relevant information for those dates.
        /// </example>
        /// </summary>
        /// <param name="FromDate">The beginning date/time of the range to test.</param>
        /// <param name="ToDate">The end date/time of the range to test.</param>                
        public void Evaluate(Date_Time FromDate, Date_Time ToDate)
        {
            foreach (iCalObject obj in Children)
            {
                if (obj is RecurringComponent)
                    ((RecurringComponent)obj).Evaluate(FromDate, ToDate);
            }
        }

        //public ArrayList GetTodos(string category)
        //{
        //    ArrayList t = new ArrayList();
        //    foreach (Todo todo in Todos)
        //    {
        //        if (todo.Categories != null)
        //        {
        //            foreach (TextCollection cat in todo.Categories)
        //            {
        //                foreach (Text text in cat.Values)
        //                {
        //                    if (text.Value == category)
        //                        t.Add(todo);
        //                }
        //            }
        //        }
        //    }

        //    return t;
        //}

        /// <summary>
        /// Merges the current <see cref="iCalendar"/> with another iCalendar.
        /// <note>
        ///     Since each object is associated with one and only one iCalendar object,
        ///     the <paramref name="iCal"/> that is passed is automatically Disposed
        ///     in the process, because all of its objects are re-assocated with the new iCalendar.
        /// </note>
        /// </summary>
        /// <param name="iCal">The iCalendar to merge with the current <see cref="iCalendar"/></param>
        public void MergeWith(iCalendar iCal)
        {
            if (iCal != null)
            {
                foreach (UniqueComponent uc in iCal.UniqueComponents)
                    this.AddChild(uc);
                iCal.Dispose();
            }
        }

        /// <summary>
        /// Invokes the object creation of the selected ComponentBase class.  By default,
        /// this is the ComponentBase class itself; however, this can be changed to allow
        /// for custom objects to be created in lieu of Event, Todo, Journal, etc. components.
        /// </summary>
        /// <param name="parent">The parent of the object to create.</param>
        /// <param name="name">The name of the iCal object.</param>
        /// <returns></returns>
        public ComponentBase Create(iCalObject parent, string name)
        {
            if (m_ComponentBaseCreate == null)
                throw new ArgumentException("Create() cannot be called without a valid ComponentBase Create() method attached");
            else return (ComponentBase)m_ComponentBaseCreate.Invoke(null, new object[] { parent, name });
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Children.Clear();
            Events.Clear();
            FreeBusy.Clear();
            Journals.Clear();
            Todos.Clear();
        }

        #endregion
    }
}
