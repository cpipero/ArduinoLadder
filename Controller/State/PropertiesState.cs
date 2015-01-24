using System.Linq;

namespace LadderLogic.Controller.State
{
	using File.DrawingFile;
	using Surface;

	public class PropertiesState : State
	{
		public PropertiesState () : base(StateType.CursorState)
		{
		}


		public Segment Element { get; set; }

		#region implemented abstract members of State

		public override bool Handle (State previous, Segment prevSegment, Segment newSegment, bool left)
		{
			if (prevSegment != null && !prevSegment.IsPalette && prevSegment.Type != ElementType.None) {
				Element = prevSegment;
				Element.Selected = true;

				if (Element.Type == ElementType.Coil ||
				   Element.Type == ElementType.NotCoil ||
				   Element.Type == ElementType.SetCoil ||
				   Element.Type == ElementType.ResetCoil) {
					MainClass._main.BindDefaultOutputs ();
				}
				else{
					MainClass._main.BindDefaultInputs ();
				}

				MainClass._main.BindExistingVariables (Element);
				MainClass._main.BindFunction (newSegment);
				AppController.Instance.CheckUserVariables ();

				if (prevSegment.Variables.Any ()) {
					int index = 0;
					foreach (var v in prevSegment.Variables.ToList()) {
						MainClass._main.BindElementVariables (v.Type, v.Value, index++);
					}
					for (int i = index; i < 4; i++) {
						MainClass._main.BindElementVariables (VariableType.Default, string.Empty, i);
					}
				}

				AppController.Instance.EnableProperties (Element);
			}

			base.Handle (previous, prevSegment, newSegment, left);
			return true;
		}


		public override bool Leave ()
		{
			AppController.Instance.DisableProperties ();
			return base.Leave ();
		}

		#endregion
	}
}

