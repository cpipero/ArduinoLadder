using System.Linq;

namespace LadderLogic.Controller.State
{
	using File.DrawingFile;
	using Surface;

	public class EmptyState : State
	{
		public EmptyState () : base(StateType.EmptyState)
		{
		}

		#region implemented abstract members of State

		public override bool Handle (State previous, Segment prevSegment, Segment newSegment, bool left)
		{
			if (prevSegment != null && 
				(prevSegment.Type == ElementType.Line || 
					prevSegment.Type == ElementType.Cursor ||
					prevSegment.Type == ElementType.Properties)) {
				return true;
			}

			//deny add coil not to last column.
			if (prevSegment != null &&
			   (prevSegment.Type == ElementType.Coil || 
					prevSegment.Type == ElementType.NotCoil || 
					prevSegment.Type == ElementType.SetCoil || 
					prevSegment.Type == ElementType.ResetCoil) &&
			   newSegment != null &&
				newSegment.Position.X < (newSegment.Surface.Width )) {
				return true;
			}

			if (prevSegment != null) {

				if (prevSegment.Type == ElementType.OffTimer ||
				   prevSegment.Type == ElementType.OnTimer ||
				   prevSegment.Type == ElementType.PulseTimer ||
				   prevSegment.Type == ElementType.CycleTimer) {
					if (newSegment != null && (newSegment.Position.X == newSegment.Surface.Width)) {
						return true;
					}
				}
				//disable latch on last row
				if (newSegment != null && (prevSegment.Type == ElementType.Latch &&
				                           newSegment.Surface.Height <= newSegment.Position.Y)) {
					return true;
				}

				//disable latch on last column
				if (newSegment != null && (prevSegment.Type == ElementType.Latch &&
				                           newSegment.Surface.Width <= newSegment.Position.X)) {
					return true;
				}


				//disable latch on last column
				if (newSegment != null && (prevSegment.Type == ElementType.Latch &&
				                           newSegment.Position.X == 0)) {
					return true;
				}

				if (newSegment != null)
				{
					var bottom = newSegment.Surface.Get ().FirstOrDefault (s =>s.Position.X == newSegment.Position.X &&
					                                                           s.Position.Y == newSegment.Position.Y + 1);
					//disable latch if bootom segment was not empty
					if (prevSegment.Type == ElementType.Latch &&
					    (bottom == null || bottom.Type != ElementType.None)) {
						    return true;
					    }
				}
			}


			if (prevSegment != null
				&& newSegment != null
				&& prevSegment.IsPalette
				&& !newSegment.IsPalette) {
				var el = prevSegment
					.Primitives
					.FirstOrDefault (p => p.Container != null);

				if (el != null) {
					var newEl =  (DrawingElement)(el.Container.Clone ());
					newEl.SetupContainer ();
					newSegment
						.Surface
						.Add(newEl, newSegment.Position)
						.ConnectPower()
						.ConnectSiblings();
				}
			}
			base.Handle (previous, prevSegment, newSegment, left);
			return true;
		}

		#endregion
	}
}

