using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SocketIO;



public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;
    public Canvas canvas;
    public InputField playerNameInput;
    public GameObject player;
    public SocketIOComponent socket;
    public Camera camaraFondo;

    void Awake()
    {
      if(instance == null)
          instance = this;
      else if (instance != this)
          Destroy(gameObject);
      DontDestroyOnLoad (gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
      //subscribe to the barious wbsocket vents
      socket.On("play", OnPlay);
      socket.On("other player connected", OnOtherPlayerConnect);
      socket.On("player move", OnPlayerMove);
      socket.On("player turn", OnPlayerTurn);
      socket.On("player disconnected", OnPlayerDisconnect);

    }

    // Update is called once per frame
    public void JoinGame()
    {
        StartCoroutine(ConnectToServer());
    }

    #region Commands

    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);

        socket.Emit("player connect");

        yield return new WaitForSeconds(1f);

        string playerName = playerNameInput.text;
        List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;

        PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints);

        string data= JsonUtility.ToJson(playerJSON);
        socket.Emit("play", new JSONObject(data));
        canvas.gameObject.SetActive(false);
        camaraFondo.gameObject.SetActive(false);

    }
    #endregion

    public void CommandMove (Vector3 vec3)
    {
      string data = JsonUtility.ToJson(new PositionJSON(vec3));
      socket.Emit("player move", new JSONObject(data));
    }

    public void CommandTurn (Quaternion quat)
    {
      string data = JsonUtility.ToJson(new RotationJSON(quat));
      socket.Emit("player turn", new JSONObject(data));
    }

    #region Listening

    void OnPlay(SocketIOEvent socketIOEvent)
    {
      print("You joined");
      string data = socketIOEvent.data.ToString();
      UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
      string name= (currentUserJSON.name).ToString();


      Vector3 position= new Vector3 (currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
      Quaternion rotation = Quaternion.Euler(currentUserJSON.rotation[0], currentUserJSON.rotation[1], currentUserJSON.rotation[2]);
      GameObject p = Instantiate(player,position,rotation) as GameObject;
      PlayerController pc = p.GetComponent<PlayerController>();
      Transform t = p.transform.Find("Healthbar Canvas");
      Transform t1 = t.transform.Find("Player Name");
      Text playerName = t1.GetComponent<Text>();
      playerName.text = name;
      pc.isLocalPlayer = true;
      p.name = name;
    }
    void OnOtherPlayerConnect(SocketIOEvent socketIOEvent)
    {
      print("Someone else joined");
      string data = socketIOEvent.data.ToString();
      UserJSON userJSON = UserJSON.CreateFromJSON(data);
      string name= (userJSON.name).ToString();
      Vector3 position= new Vector3 (userJSON.position[0], userJSON.position[1], userJSON.position[2]);
      Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
      GameObject o = GameObject.Find(name) as GameObject;
      if (o != null)
      {
        return;
      }
      GameObject p = Instantiate (player, position, rotation) as GameObject;
      PlayerController pc = p.GetComponent<PlayerController>();
      Transform t = p.transform.Find("Healthbar Canvas");
      Transform t1 = t.transform.Find("Player Name");
      Text playerName = t1.GetComponent<Text>();
      playerName.text = name;
      pc.isLocalPlayer = false;
      p.name = name;

    }
    void OnPlayerMove(SocketIOEvent socketIOEvent)
    {
      string data = socketIOEvent.data.ToString();
      UserJSON userJSON = UserJSON.CreateFromJSON(data);
      Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
      if (name == playerNameInput.text)
      {
        return;
      }
      GameObject p = GameObject.Find(name) as GameObject;
      if(p != null)
      {
        p.transform.position = position;
      }
    }
    void OnPlayerTurn(SocketIOEvent socketIOEvent)
    {
      string data = socketIOEvent.data.ToString();

      UserJSON userJSON = UserJSON.CreateFromJSON(data);
      string name= (userJSON.name).ToString();
      Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
      if (name == playerNameInput.text)
      {
        return;
      }
      GameObject p = GameObject.Find(name) as GameObject;
      if(p != null)
      {
        p.transform.rotation = rotation;
      }
    }
    void OnPlayerDisconnect(SocketIOEvent socketIOEvent)
    {
      print("user disconnected");
      string data = socketIOEvent.data.ToString();
      UserJSON userJSON = UserJSON.CreateFromJSON(data);
      Destroy(GameObject.Find((userJSON.name).ToString()));
    }

    #endregion

    #region JSONMessageClasses
    public class PlayerJSON
    {
        public string name;
        public List<PointJSON> playerSpawnPoints;


        public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoints)
        {
            playerSpawnPoints = new List<PointJSON>();
            // enemySpawnPoints = new List<PointJSON>();
            name = _name;
            foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints)
            {
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                playerSpawnPoints.Add(pointJSON);
            }

        }
    }
    [Serializable]
    public class PointJSON
    {
        public float[] position;
        public float[] rotation;
        public PointJSON(SpawnPoint spawnPoint)
        {
            position = new float[]
            {
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y,
                spawnPoint.transform.position.z
            };
            rotation = new float[]
            {
                spawnPoint.transform.eulerAngles.x,
                spawnPoint.transform.eulerAngles.y,
                spawnPoint.transform.eulerAngles.z
            };

        }
    }
    [Serializable]
    public class PositionJSON
    {
        public float[] position;
        public PositionJSON(Vector3 _position)
        {
            position = new float[]{_position.x, _position.y, _position.z};
        }
    }
    [Serializable]
    public class RotationJSON
    {
        public float[] rotation;
        public RotationJSON(Quaternion _rotation)
        {
            rotation = new float[]
            {
                _rotation.eulerAngles.x, _rotation.eulerAngles.y, _rotation.eulerAngles.z
            };
        }
    }
    [Serializable]
    public class UserJSON
    {
        public float name;
        public float[] rotation;
        public float[] position;
        public int health;

        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }

    }
    // public class EnemiesJSON
    // {
    //     public List<UserJSON> enemies;
    //     public static EnemiesJSON CreateFromJSON(string data)
    //     {
    //         return JsonUtility.FromJSON<EnemiesJSON>(data);
    //     }
    // }


    #endregion
}
