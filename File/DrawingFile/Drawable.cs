using System;
using System.Xml.Serialization;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	[XmlInclude(typeof(LineElement)), 
		XmlInclude(typeof(ArcElement)),
		XmlInclude(typeof(Arc3PointsElement)),
		XmlInclude(typeof(TextElement)),
		XmlInclude(typeof(Connector)),
		XmlInclude(typeof(RectangleElement))]
	public class Drawable
	{
		public Drawable ()
		{
			Alpha = 1;
		}


		public double Alpha { get; set; }


		public Color Foregraund { get; set; }


		public double Weight { get; set; }


		public uint ZIndex { get; set; }


		[XmlIgnore]
		public DrawingElement Container { get; set; }


		public string Marker { get;set; }
	}
}

