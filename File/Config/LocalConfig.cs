using System;

namespace LadderLogic.File.Config
{
	using DrawingFile;

	[Serializable]
	public class LocalConfig
	{
		public string ArduinoParameters { get; set; }


		public double SelectionOpacity { get; set; }


		public double PadOpacity { get; set; }


		public string LeftConnector { get; set; }


		public string RightConnector { get; set; }


		public string ConS { get; set; }


		public string ConR { get; set; }


		public string ConQ { get; set; }


		public string LeftPower { get; set; }


		public string RightPower { get; set; }


		public string Selected { get; set; }


		public string Caption { get; set; }


		public uint ContactsCount { get; set; }


		public uint RowsCount { get; set; }


		public uint SegmentsOrderMultiplier { get; set; }


		public string DrawingAreaGlade { get; set; }


		public string DrawingAreaWindow { get; set; }


		public string DrawingAreaWindowName { get; set; }


		public string UnhandledExceptionDialog { get; set; }


		public string UnhandledExceptionDialogName { get; set; }


		public string SourceDialog { get; set; }


		public string SourceDialogName { get; set; }


		public string AboutDialog { get; set; }


		public string AboutDialogName { get; set; }


		public string DrawingAreaTitle { get; set; }


		public string AppGlade { get; set; }


		public string AppWindow { get; set; }


		public string AppWindowName { get; set; }


		public string AppTitle { get; set; }


		public string ColumnLabel { get; set; }


		public string AxisFont { get; set; }


		public string AxisAlign { get; set; }


		public string AxisVAlign { get; set; }


		public int XAxisFontSize { get; set; }


		public int YAxisFontSize { get; set; }


		public double XAxisOffset { get; set; }


		public double YAxisOffset { get; set; }


		public string Icon { get; set; }


		public uint GridHMargin { get; set; }


		public uint GridVMargin { get; set; }


		public Color GridLinesColor { get; set; }


		public Color PowerColor { get; set; }


		public string PadSelected { get; set; }


		public Color SelectedBg { get; set; }


		public Color ConnectedColor { get; set; }


		public Color AxisColor { get; set; }


		public Color SelectedConnectorColor { get; set; }


		public Color LineColor { get; set; }


		public String [] PaletteFiles { get; set; }


		public string PlcInputs { get; set; }


		public string PlcOutputs { get; set; }
	}
}

