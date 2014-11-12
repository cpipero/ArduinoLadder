using System;
using System.Collections.Generic;

namespace LadderLogic.File.DrawingFile
{
	using Reader;

	[Serializable]
	public class DrawingElement : ICloneable
	{
		List<Drawable> _primitives;


		public List<Drawable> Primitives 
		{
			get
			{ 
				return _primitives; 
			} 

			set
			{ 
				_primitives = value;
				SetupContainer ();
			}
		}


		public ElementType Type { get; set; }


		public string Tooltip { get; set; }


		public ElementFunction [] Functions { get; set; }


		public int Order { get;set; }

		public void SetupContainer()
		{
			foreach (var p in Primitives) {
				p.Container = this;
			}
		}

		#region ICloneable implementation

		public object Clone ()
		{
			return ObjectCopier.Clone (this);
		}

		#endregion
	}
}

