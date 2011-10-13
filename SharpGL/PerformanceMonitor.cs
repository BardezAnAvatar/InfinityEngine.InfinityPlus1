using System;
using System.Collections;

namespace SharpGL
{
	/// <summary>
	/// The PerformanceMonitor class is used to time code execution, to find bottlenecks
	/// and potential slow spots.
	/// </summary>
	public class PerformanceMonitor
	{
		public PerformanceMonitor()
		{
		}

		/// <summary>
		/// Use this function to monitor an event.
		/// </summary>
		/// <param name="eventName">The name of the event you are monitoring.</param>
		public virtual void Monitor(string eventName)
		{
			//	If we are already monitoring, then update the last event.
			if(currentEvent != null)
				EndMonitor();

			//	Create a new event.
			currentEvent = new Event();
			currentEvent.Name = eventName;

			//	Get the time.
			lastTime = DateTime.Now;
		}

		public virtual void EndMonitor()
		{
			if(lastTime != DateTime.MinValue)
			{
				//	Update the event's time.
				if(currentEvent != null)
				{
					TimeSpan span = new TimeSpan(DateTime.Now.Ticks - lastTime.Ticks);
					currentEvent.Time = span.Milliseconds;
					events.Add(currentEvent);
				}
			}
		}

		public virtual void Draw(System.Drawing.Graphics graphics)
		{
			System.Drawing.Font font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif,
				10);

			int y = 10;

			foreach(Event eventEach in events)
			{
				string info = eventEach.Name + " : " + eventEach.Time.ToString() + " Milliseconds.";
				graphics.DrawString(info, font, System.Drawing.Brushes.White, 10, y);
				y += 30;
			}
		}

		protected DateTime lastTime = DateTime.MinValue;
		protected Event currentEvent = null;

		protected EventCollection events = new EventCollection();

		public class Event
		{
			public Event()
			{
			}

			protected string name = "";

			protected int time = 0;

			public string Name
			{
				get {return name;}
				set {name = value;}
			}
			public int Time
			{
				get {return time;}
				set {time = value;}
			}
		}

		public class EventCollection : CollectionBase
		{
			public EventCollection(){}

			public virtual int Add(Event value)
			{
				return List.Add(value);
			}

			public virtual Event this[int index]
			{
				get {return (Event)List[index];}
				set {List[index] = value;}
			}

			public virtual void Remove(Event value)
			{
				List.Remove(value);
			}
		}
	}
}
