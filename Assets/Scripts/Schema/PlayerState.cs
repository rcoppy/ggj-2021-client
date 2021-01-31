// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.10
// 

using Colyseus.Schema;

namespace Dossamer.Ggj2021 {
	public partial class PlayerState : Schema {
		[Type(0, "ref", typeof(TileOccupant))]
		public TileOccupant pieceOnBoard = new TileOccupant();
	}
}
