using LadderLogic.File.DrawingFile;

namespace LadderLogic.Controller
{
	using File;
	using Reader;
	using Surface;

	public class DrawingAreaController
	{
		static DrawingAreaController _instance;


		DrawingAreaController ()
		{
		}


		public static DrawingAreaController Instance 
		{
			get 
			{
				return _instance = _instance ?? new DrawingAreaController ();
			}
		}


		public PrimitivesSurface Surface { get; set; }


		public void Initialize()
		{
			var el = ConfigManager.Read<DrawingElement> (("Element" + System.IO.Path.DirectorySeparatorChar + "sample.xml").GetAbsolutePath());
			el.SetupContainer ();
			Surface = new PrimitivesSurface () { IsPalette = true };
			Surface.Add (el, new Position{ X = 0, Y = 0 });
		}
	}
}

