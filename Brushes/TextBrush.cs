using Cairo;

namespace LadderLogic.Brushes
{
	using File.DrawingFile;

	public static class TextBrush
	{
		public static void Draw (Context grw, TextElement text)
		{
			double dY = 0;

			if (text.Foregraund != null) {
				grw.SetSourceRGB (
					text.Foregraund.Red, 
					text.Foregraund.Green, 
					text.Foregraund.Blue);
			}

			grw.SelectFontFace (
				text.FontFamily, 
				FontSlant.Normal, 
				(FontWeight)text.Weight);

			grw.SetFontSize (text.ScaledFontSize);
			var te = grw.TextExtents (text.Text);

			double dX = 0;
			if (text.Align != null) {
				switch (text.Align.ToLowerInvariant()) {
				case AlignType.Center:
					dX = 0.5 * te.Width;

					break;
				case AlignType.Right:
					dX = te.Width;

					break;
				case AlignType.Left:
					dX = 0;
					break;
				default:
					//throw new ArgumentException ("Invalid argument: " + text.Align);
					return;
				}
			}

			if (text.VAlign != null) {
				switch (text.VAlign.ToLowerInvariant ()) {
				case VAlignType.Top:
					dY = te.YBearing;
					break;
				case VAlignType.Center:
					dY = 0.5 * te.YBearing;
					break;
				case VAlignType.Bottom:
					dY = 0;
					break;
				default:
					//throw new ArgumentException ("Invalid argument: " + text.VAlign);
					return;
				}
			}

			grw.MoveTo (
				text.Start.GeometryX - dX, 
				text.Start.GeometryY - dY);

			grw.ShowText (text.FormattedText);

			grw.Stroke ();
		}
	}
}

