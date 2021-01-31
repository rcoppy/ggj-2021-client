using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : Singleton<Constants> {

	public enum OccupantTypes
	{
		Monster = 0, // auto-increment, order matters; make sure this matches the corresponding enum on the server side
		FinalBoss,
		Player,
		Wall,
		Loot,
		Chest,
		Shopkeep,
		Fire,
		Blank
	}

	[LabeledArray(typeof(OccupantTypes))]
	public GameObject[] occupantTypePrefabs = new GameObject[System.Enum.GetNames(typeof(OccupantTypes)).Length];

	/*[LabeledArray(new string[] { "Monster", "Final Boss", "Player", "Wall", "Loot", "Chest", "Shopkeep", "Fire", "Blank" })]
	public GameObject[] occupantTypePrefabs = new GameObject[System.Enum.GetNames(typeof(OccupantTypes)).Length];
*/
	/*[LabeledArray(typeof(OccupantTypes))]
	public int[] enumLabeledValues;*/

	/*// need to set these in the editor
	public Dictionary<OccupantTypes, GameObject> OccupantTypePrefabs = new Dictionary<OccupantTypes, GameObject>() {
		{ OccupantTypes.Monster, null },
		{ OccupantTypes.FinalBoss, null },
		{ OccupantTypes.Player, null },
		{ OccupantTypes.Wall, null },
		{ OccupantTypes.Loot, null },
		{ OccupantTypes.Chest, null },
		{ OccupantTypes.Shopkeep, null },
		{ OccupantTypes.Fire, null },
		{ OccupantTypes.Blank, null }
	};*/
}
