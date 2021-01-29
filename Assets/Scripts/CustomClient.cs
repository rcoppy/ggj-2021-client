using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

using Colyseus;
using Colyseus.Schema;

using GameDevWare.Serialization;

using Settworks.Hexagons;

namespace Dossamer.Ggj2021
{
	[Serializable]
	class Metadata
	{
		public string str;
		public int number;
	}

	[Serializable]
	class CustomRoomAvailable : RoomAvailable
	{
		public Metadata metadata;
	}

	class CustomData
	{
		public int integer;
		public string str;
	}

	class TypeMessage
	{
		public bool hello;
	}

	enum MessageType
	{
		ONE = 0
	};
	class MessageByEnum
	{
		public string str;
	}

	public class CustomClient : MonoBehaviour
	{

		// UI Buttons are attached through Unity Inspector
		public Button
			m_ConnectButton,
			m_CreateButton,
			m_JoinOrCreateButton,
			m_JoinButton,
			m_ReconnectButton,
			m_SendMessageButton,
			m_LeaveButton,
			m_GetAvailableRoomsButton;
		public InputField m_EndpointField;
		public Text m_IdText, m_SessionIdText;

		public string roomName = "demo";

		protected Client client;
		protected Room<MyRoomState> room;

		// protected Room<IndexedDictionary<string, object>> roomFossilDelta;
		protected Room<object> roomNoneSerializer;

		// protected IndexedDictionary<Entity, GameObject> entities = new IndexedDictionary<Entity, GameObject>();

		protected IndexedDictionary<Hex, GameObject> hexTiles = new IndexedDictionary<Hex, GameObject>();

		[SerializeField]
		protected GameObject hexMeshObject;

		[SerializeField]
		protected GameObject gridContainerObject;

		// Use this for initialization
		void Start()
		{
			/* Demo UI */
			m_ConnectButton.onClick.AddListener(ConnectToServer);

			// m_CreateButton.onClick.AddListener(CreateRoom);
			m_JoinOrCreateButton.onClick.AddListener(JoinOrCreateRoom);
			// m_JoinButton.onClick.AddListener(JoinRoom);
			m_ReconnectButton.onClick.AddListener(ReconnectRoom);
			/*m_SendMessageButton.onClick.AddListener(SendMessage);*/
			m_LeaveButton.onClick.AddListener(LeaveRoom);
			// m_GetAvailableRoomsButton.onClick.AddListener(GetAvailableRooms);
		}

		async void ConnectToServer()
		{
			/*
			 * Get Colyseus endpoint from InputField
			 */
			string endpoint = m_EndpointField.text;

			Debug.Log("Connecting to " + endpoint);

			/*
			 * Connect into Colyeus Server
			 */
			client = ColyseusManager.Instance.CreateClient(endpoint);

			// await client.Auth.Login();

			// var friends = await client.Auth.GetFriends();

			// Update username
			/*client.Auth.Username = "Jake";
			await client.Auth.Save();*/
		}

		public async void CreateRoom()
		{
			room = await client.Create<MyRoomState>(roomName, new Dictionary<string, object>() { });
			//roomNoneSerializer = await client.Create("no_state", new Dictionary<string, object>() { });
			//roomFossilDelta = await client.Create<IndexedDictionary<string, object>>("fossildelta", new Dictionary<string, object>() { });
			RegisterRoomHandlers();
		}

		public async void JoinOrCreateRoom()
		{
			Debug.Log("Trying to join or create");
			room = await client.JoinOrCreate<MyRoomState>(roomName, new Dictionary<string, object>() { });
			RegisterRoomHandlers();
			Debug.Log("Success!");
		}

		public async void JoinRoom()
		{
			room = await client.Join<MyRoomState>(roomName, new Dictionary<string, object>() { });
			RegisterRoomHandlers();
		}

		async void ReconnectRoom()
		{
			string roomId = PlayerPrefs.GetString("roomId");
			string sessionId = PlayerPrefs.GetString("sessionId");
			if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(roomId))
			{
				Debug.Log("Cannot Reconnect without having a roomId and sessionId");
				return;
			}

			room = await client.Reconnect<MyRoomState>(roomId, sessionId);

			Debug.Log("Reconnected into room successfully.");
			RegisterRoomHandlers();
		}

		public void RegisterRoomHandlers()
		{
			m_SessionIdText.text = "sessionId: " + room.SessionId;

			room.State.grid.OnAdd += OnHexTileAdd;
			room.State.grid.OnRemove += OnHexTileRemove;
			room.State.TriggerAll();

			PlayerPrefs.SetString("roomId", room.Id);
			PlayerPrefs.SetString("sessionId", room.SessionId);
			PlayerPrefs.Save();

			room.OnLeave += (code) => Debug.Log("ROOM: ON LEAVE");
			room.OnError += (code, message) => Debug.LogError("ERROR, code =>" + code + ", message => " + message);
			room.OnStateChange += OnStateChangeHandler;

			room.OnMessage((Message message) =>
			{
				Debug.Log("Received Schema message:");
				Debug.Log(message.num + ", " + message.str);
			});

			room.OnMessage<MessageByEnum>((byte)MessageType.ONE, (message) =>
		   {
			   Debug.Log(">> Received message by enum/number => " + message.str);
		   });

			room.OnMessage<TypeMessage>("type", (message) =>
			{
				Debug.Log("Received 'type' message!");
				Debug.Log(message);
			});

			_ = room.Send((byte)MessageType.ONE, new MessageByEnum { str = "Sending message by enum/number" });
		}


		async void LeaveRoom()
		{
			await room.Leave(false);

			// Destroy game tiles
			DestroyGrid();
		}

		async void GetAvailableRooms()
		{
			var roomsAvailable = await client.GetAvailableRooms<CustomRoomAvailable>(roomName);

			Debug.Log("Available rooms (" + roomsAvailable.Length + ")");
			for (var i = 0; i < roomsAvailable.Length; i++)
			{
				Debug.Log("roomId: " + roomsAvailable[i].roomId);
				Debug.Log("maxClients: " + roomsAvailable[i].maxClients);
				Debug.Log("clients: " + roomsAvailable[i].clients);
				Debug.Log("metadata.str: " + roomsAvailable[i].metadata.str);
				Debug.Log("metadata.number: " + roomsAvailable[i].metadata.number);
			}
		}

		/*void SendMessage()
		{
			if (room != null)
			{
				room.Send("schema");
				room.Send("move_right", new CustomData()
				{
					integer = 100,
					str = "Hello world!"
				});
			}
			else
			{
				Debug.Log("Room is not connected!");
			}
		}*/

		void OnStateChangeHandler(MyRoomState state, bool isFirstState)
		{
			// Setup room first state
			Debug.Log("State has been updated!");

			if (isFirstState)
			{
				/*InitHexGrid();*/
			} else
			{

			}
		}

		/*void InitHexGrid()
		{
			foreach (var hex in room.State.grid.Values)
			{
				hex
			}
		}*/

		void DestroyGrid()
		{
			foreach (KeyValuePair<Hex, GameObject> entry in hexTiles)
			{
				Destroy(entry.Value);
			}

			hexTiles.Clear();
		}

		Vector3 HexCartesianOffsetToWorldPosition(Hex hex)
		{
			Vector2 worldCoord = HexCoord.AtOffset((int)hex.x, (int)hex.y).Position();

			return new Vector3(worldCoord.x, 0, worldCoord.y);	
		}

		void OnHexTileAdd(Hex hex, string key)
		{
			GameObject mesh = GameObject.Instantiate(hexMeshObject);

			mesh.transform.position = HexCartesianOffsetToWorldPosition(hex);

			Debug.Log("Tile add! x => " + hex.x + ", y => " + hex.y);

			mesh.transform.parent = gridContainerObject.transform;
			

			// Add "player" to map of players
			hexTiles.Add(hex, mesh);

			// On entity update...
			hex.OnChange += (List<Colyseus.Schema.DataChange> changes) =>
			{
				mesh.transform.position = HexCartesianOffsetToWorldPosition(hex);
			};
		}

		void OnHexTileRemove(Hex hex, string key)
		{
			GameObject cube;
			hexTiles.TryGetValue(hex, out cube);
			Destroy(cube);

			hexTiles.Remove(hex);
		}

		/*void OnEntityAdd(Entity entity, string key)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

			Debug.Log("Player add! x => " + entity.x + ", y => " + entity.y);

			cube.transform.position = new Vector3(entity.x, entity.y, 0);

			// Add "player" to map of players
			entities.Add(entity, cube);

			// On entity update...
			entity.OnChange += (List<Colyseus.Schema.DataChange> changes) =>
			{
				cube.transform.Translate(new Vector3(entity.x, entity.y, 0));
			};
		}
*/
		/*void OnEntityRemove(Entity entity, string key)
		{
			GameObject cube;
			entities.TryGetValue(entity, out cube);
			Destroy(cube);

			entities.Remove(entity);
		}*/

		void OnApplicationQuit()
		{
		}
	}
}
