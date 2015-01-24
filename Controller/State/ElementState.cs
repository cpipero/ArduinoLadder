using System.Linq;

namespace LadderLogic.Controller.State
{
	using Surface;
	using File.DrawingFile;

	public class ElementState : State
	{
		public ElementState () : base(StateType.ElementState)
		{
		}

		#region implemented abstract members of State

		public override bool Handle (State previous, Segment prevSegment, Segment newSegment, bool left)
		{
			if (newSegment != null && !newSegment.IsPalette && newSegment.Type != ElementType.None) {

				if (newSegment.Type == ElementType.Coil ||
					newSegment.Type == ElementType.NotCoil || 
					newSegment.Type == ElementType.SetCoil ||
					newSegment.Type == ElementType.ResetCoil) {
					MainClass._main.BindDefaultOutputs ();
				} else {
					MainClass._main.BindDefaultInputs ();
				}

				MainClass._main.BindExistingVariables (newSegment);
				MainClass._main.BindFunction (newSegment);
				AppController.Instance.CheckUserVariables ();

				if (newSegment.Variables.Any ()) {
					int index = 0;
					foreach (var v in newSegment.Variables) {
						MainClass._main.BindElementVariables (v.Type, v.Value, index++);
					}
					for (int i = index; i < 4; i++) {
						MainClass._main.BindElementVariables (VariableType.Default, string.Empty, i);
					}
				}
				MainClass._main.DisableProperties ();
			}

			base.Handle (previous, prevSegment, newSegment, left);
			return true;
		}

		#endregion
	}
}

