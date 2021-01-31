// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.10
// 

using Colyseus.Schema;

namespace Dossamer.Ggj2021 {
	public partial class MyRoomState : Schema {
		[Type(0, "string")]
		public string mySynchronizedProperty = default(string);

		[Type(1, "map", typeof(MapSchema<PlayerState>))]
		public MapSchema<PlayerState> playerStates = new MapSchema<PlayerState>();

		[Type(2, "map", typeof(MapSchema<Hex>))]
		public MapSchema<Hex> grid = new MapSchema<Hex>();

		[Type(3, "map", typeof(MapSchema<TileOccupant>))]
		public MapSchema<TileOccupant> tileOccupants = new MapSchema<TileOccupant>();
	}
}
