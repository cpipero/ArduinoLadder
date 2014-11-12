using System.Xml.Serialization;

namespace LadderLogic.Surface
{
	public class Position
	{
		protected bool Equals(Position other)
		{
			return X == other.X && Y == other.Y;
		}


		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Position) obj);
		}


		public override int GetHashCode()
		{
			unchecked
			{
				return ((int) X*397) ^ (int) Y;
			}
		}


		public Position ()
		{
		}


		public Position (uint x, uint y)
		{
			X = x;
			Y = y;
		}


		public uint X { get; set; }


		public uint Y { get; set; }
		

		[XmlIgnore]
		public uint GeometryXStart { get; set; }


		[XmlIgnore]
		public uint GeometryYStart { get; set; }


		[XmlIgnore]
		public uint GeometryXEnd { get; set; }


		[XmlIgnore]
		public uint GeometryYEnd { get; set; }


		[XmlIgnore]
		public uint GeometryWidth {get { return GeometryXEnd - GeometryXStart; }}


		[XmlIgnore]
		public uint GeometryHeight {get { return GeometryYEnd - GeometryYStart; }}
		
		public static bool operator == (Position position1, Position position2)
        {
            return position1.X == position2.X && position1.Y == position2.Y;
        }


        public static bool operator != (Position position1, Position position2)
        {
            return position1.X != position2.X || position1.Y != position2.Y;
        }
	}
}

