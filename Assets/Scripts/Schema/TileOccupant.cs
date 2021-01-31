// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.10
// 

using Colyseus.Schema;

namespace Dossamer.Ggj2021 {
	public partial class TileOccupant : Schema {
		[Type(0, "number")]
		public float occupantTypeId = default(float);

		[Type(1, "string")]
		public string tileId = default(string);

		[Type(2, "string")]
		public string textDescription = default(string);

		[Type(3, "boolean")]
		public bool isPositionUpdating = default(bool);

		[Type(4, "array", typeof(ArraySchema<Coordinate>))]
		public ArraySchema<Coordinate> moveQueue = new ArraySchema<Coordinate>();

		[Type(5, "number")]
		public float _id = default(float);
	}
}
