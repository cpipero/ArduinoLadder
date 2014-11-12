namespace LadderLogic.CTool.FunctionRule
{
	public class NoContactRule : ContactRule
	{
		public NoContactRule()
		{
			ElementType = File.DrawingFile.ElementType.NoContact;
			First = FunctionType.InNot;
			Next = FunctionType.AndNotBit;
			Sibling = FunctionType.OrNotBit;
		}
	}
}

