namespace LadderLogic.CTool.FunctionRule
{
	public class NcContactRule : ContactRule
	{
		public NcContactRule()
		{
			ElementType = File.DrawingFile.ElementType.NcContact;
			First = FunctionType.In;
			Next = FunctionType.AndBit;
			Sibling = FunctionType.OrBit;
		}
	}
}

