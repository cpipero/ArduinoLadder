using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class Connector : Drawable
	{
		public Point Center { get; set; }


		public double Radius { get; set; }


		public double GeometryRadius { get; set; }


		public HashSet<String> ConnectedTo { get; set; }
		

		[XmlIgnore]
		public bool Selected { get; set; }
	}
}

