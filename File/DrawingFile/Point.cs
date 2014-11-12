using System;
using System.Xml.Serialization;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class Point
	{
		public override int GetHashCode()
		{
			unchecked
			{
				return (X.GetHashCode()*397) ^ Y.GetHashCode();
			}
		}

		public double X { get; set; }


		public double Y { get; set; }


		[XmlIgnore]
		public double GeometryX { get; set; }


		[XmlIgnore]
		public double GeometryY { get; set; }
	}
}

