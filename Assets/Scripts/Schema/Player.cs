// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.10
// 

using Colyseus.Schema;

namespace Dossamer.Ggj2021 {
	public partial class Player : Schema {
		[Type(0, "number")]
		public float x = default(float);

		[Type(1, "number")]
		public float y = default(float);

		[Type(2, "boolean")]
		public bool isClientUpdating = default(bool);

		[Type(3, "array", typeof(ArraySchema<Coordinate>))]
		public ArraySchema<Coordinate> moveQueue = new ArraySchema<Coordinate>();
	}
}
