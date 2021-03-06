﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Win32.POINT;

// The namespace is just CoC.Bot.Data in order to simplify usage of them
namespace CoC.Bot.Data
{
	/// <summary>
	/// It's a set of coordinates, targetting a point to be clicked
	/// </summary>
	public class ClickablePoint 
	{
		public Point Point { get; private set; }

		public ClickablePoint()
		{
			Point = Point.Empty;
		}

		public ClickablePoint(Point point)			
		{			
			Point = point;
		}

		public ClickablePoint(int x, int y)
		{
			Point = new Point(x,y);
		}

		
		virtual public bool IsEmpty { get { return Point.IsEmpty; } }
		
		public static ClickablePoint Empty = new ClickablePoint(Point.Empty);

		public static explicit operator ClickablePoint(Point p)
		{
			ClickablePoint cp = new ClickablePoint(p);
			return cp;
		}

		public static implicit operator Point(ClickablePoint cp)  // So we can use a ClickablePoint where a Point is needed
		{
			return cp.Point;  
		}
		
	}
}
