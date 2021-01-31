// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.10
// 

using Colyseus.Schema;

namespace Dossamer.Ggj2021 {
	public partial class Hex : Schema {
		[Type(0, "number")]
		public float x = default(float);

		[Type(1, "number")]
		public float y = default(float);

		[Type(2, "number")]
		public float _id = default(float);

		[Type(3, "boolean")]
		public bool isOccupied = default(bool);

		[Type(4, "ref", typeof(TileOccupant))]
		public TileOccupant occupant = new TileOccupant();
	}
}
