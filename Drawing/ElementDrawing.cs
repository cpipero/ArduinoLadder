using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using System;
using System.Threading;
using Cairo;

namespace LadderLogic.Drawing
{
	using Brushes;
	using Controller;
	using File.DrawingFile;
	using Geometry;
	using Surface;
	using Drawable = File.DrawingFile.Drawable;

	public class ElementDrawing : DrawingArea
	{
		PrimitivesSurface _surface;


		Proection _pro;


		readonly uint _hMargin;


		readonly uint _vMargin;


		public event EventHandler CreateCode;


		public event EventHandler UploadCode;


		public ElementDrawing (PrimitivesSurface surface, 
			uint hMargin, 
			uint vMargin)
		{
			Surface = surface;

			_hMargin = hMargin;
			_vMargin = vMargin;
			TooltipText = surface.Tooltip;
			AddEvents ((int)EventMask.ButtonPressMask);
			ButtonPressEvent += OnMouseClick;
		}


		public PrimitivesSurface Surface { 
			get
			{ 
				return _surface; 
			} 

			set
			{ 
				_surface = value;
				if (_surface == null)
				{
					return;
				}
				_surface.QueueDraw = QueueDraw;

				_pro = new Proection (Surface);
			} 
		}


		void OnMouseClick(object o, ButtonPressEventArgs args)
		{
			var x =  args.Event.X;
			var y = args.Event.Y;
			if (Surface == null)
			{
				return;
			}

			var segment = Surface.Get ().FirstOrDefault (seg => 
				x >= seg.Position.GeometryXStart
				&& x <= seg.Position.GeometryXEnd 
				&& y >= seg.Position.GeometryYStart
				&& y <= seg.Position.GeometryYEnd);

			if (segment == null)
			{
				return;
			}

			if (!segment.Surface.IsButton) {
				AppController.Instance.PrevSegment = AppController.Instance.ClearSelection ();
				segment.Selected = true;
				AppController.Instance.NewSegment = segment;

				AppController.Instance.SetCurrentState ((x - segment.Position.GeometryXStart) < (segment.Position.GeometryWidth / 2));
			} else {
				if (segment.Type == ElementType.Create && CreateCode != null) {
					CreateCode (this, EventArgs.Empty);
				}

				segment.Selected = true;
				QueueDraw ();

				var timer = new System.Threading.Timer(obj => 
					{ 
						segment.Selected = false;
						QueueDraw (); 
					}, null, 150, System.Threading.Timeout.Infinite);
			}

		}


		protected override bool OnExposeEvent(EventExpose args)
		{
			var baseResult = base.OnExposeEvent (args);

			//ReadGepmetry
			int fX, fY, fWidth,fHeight,fDepth;

			GdkWindow.GetGeometry(out fX,out fY,out fWidth,out fHeight,out fDepth);
			_pro.SetSize ((uint)fWidth, (uint)fHeight, _hMargin, _vMargin);
			if (Surface == null)
			{
				return baseResult;
			}

			var grw = CairoHelper.Create (GdkWindow);
			DrawPrimitives(_pro.Get (), grw);
			DrawLines(_pro.Lines(), grw);
			DrawConnectors(_pro.Connectors(), grw);
			grw.Dispose ();

			return baseResult;
		}


		public void ExportToPng(string path)
		{
			//ReadGepmetry
			int fX, fY, fWidth,fHeight,fDepth;

			GdkWindow.GetGeometry(out fX,out fY,out fWidth,out fHeight,out fDepth);
			using (ImageSurface draw = new ImageSurface (Format.Argb32, fWidth, fHeight)) {
				using (Context gr = new Context (draw)) {
					DrawPrimitives(_pro.Get (), gr);
					DrawLines(_pro.Lines(), gr);
					DrawConnectors(_pro.Connectors(), gr);

					draw.WriteToPng (path);
				}
			}
		}


		static void DrawLines(IEnumerable<Line> lines, Cairo.Context grw)
		{
			foreach (var l in lines) {
				SurfaceLineBrush.Draw (grw, l);
			}
		}


		static void DrawConnectors(IEnumerable<Connector> connectors, Cairo.Context grw)
		{
			foreach (var c in connectors) {
				ConnectorBrush.Draw (grw, c);
			}
		}

		static void DrawPrimitives(IEnumerable<Drawable> primitives, Cairo.Context grw)
		{
			foreach(var p in primitives)
			{
				var l = p as LineElement;
				if (l != null) {
					LineBrush.Draw (grw, l);
				}

				var a = p as ArcElement;
				if (a != null) {
					ArcBrush.Draw (grw, a);
				}

				var a3 = p as Arc3PointsElement;
				if (a3 != null) {
					Arc3PointsBrush.Draw (grw, a3);
				}

				var t = p as TextElement;
				if (t != null) {
					TextBrush.Draw (grw, t);
				}


				var r = p as RectangleElement;
				if (r != null) {
					RectangleBrush.Draw (grw, r);
				}
			}
		}
	}
}

