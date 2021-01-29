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

		[Type(1, "map", typeof(MapSchema<Player>))]
		public MapSchema<Player> players = new MapSchema<Player>();
	}
}
