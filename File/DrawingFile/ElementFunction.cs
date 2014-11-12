using System;
using LadderLogic.CTool;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class ElementFunction
	{
		public FunctionType Function { get; set; }

		public string Format { get; set; }
	}
}

