using System;
using System.Xml.Serialization;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class TextElement : Drawable
	{
		public string Text { get; set; }


		[XmlIgnore]
		public string FormattedText { get; set; }


		public Point Start { get; set; }


		public string FontFamily { get; set; }


		public double FontSize { get; set; }


		public double ScaledFontSize { get; set; }


		public string Align { get; set; }


		public string VAlign { get; set; }


		public bool FixedSize { get; set; }


		public bool FixedX { get; set; }


		public bool FixedY { get; set; }
	}
}

