﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager toStatic;

    [Header("Firsttime Login")]
    public GameObject EnterGamePanel;
    public InputField playerNameInput;
    public Text playerName;

    [Header("Connection Panel & Lobby Panel")]
    public GameObject ConnectionStatusPanel;
    public GameObject LobbyPanel;


    [Header("Create Room UI Panel")]
    public GameObject CreateRoom;
    public InputField roomNameInputField;
    public InputField roomPasswordInputField;
    public bool playertoHost; //change status player to be host

    [Header("Create Random Join Room UI Panel")]
    public GameObject CreateJoinRandomRoom;
 

    [Header("List Room UI Panel")]
    public GameObject ListRoom;
    public GameObject roomListEntryPrefab;
    public GameObject roomListParent;
    public GameObject playerListPrefab;
    public List<GameObject> playerSits;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObject;
    private Dictionary<int, GameObject> playerListGameObjects;

    #region Unity Methods
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        toStatic = this;
        playertoHost = false;

        ActivatePanel(EnterGamePanel.name);

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObject = new Dictionary<string, GameObject>();
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName +  " is Connected to photon sever.");
        //ActivatePanel(ConnectionStatusPanel.name);
        ActivatePanel(LobbyPanel.name);
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Internect");
        ActivatePanel(ConnectionStatusPanel.name);
    }


    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRandomFailed(returnCode, message);
    //    Debug.Log(message);
    //    CreateAndJoinRoom();
    //}

    #endregion

    #region Private methods
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created Room");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " join to " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(LobbyPanel.name);
        ActivatePanel(GameManager.toStatic.PokdengGame.name);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            GameManager.toStatic.PokdengStart.SetActive(true);
        else
            GameManager.toStatic.PokdengStart.SetActive(false);


         if (playerListGameObjects == null)
             playerListGameObjects = new Dictionary<int, GameObject>();

        foreach (Photon.Realtime.Player _player in PhotonNetwork.PlayerList)
        {
            GameObject playerGameObject = Instantiate(GameManager.toStatic.playerPrefab);
            playerSits.Add(playerGameObject);
            playerSits[0].gameObject.tag = "Host";
            playerSits[0].transform.SetPositionAndRotation(new Vector2(GameObject.Find("Host-Position").transform.position.x,
                                                                       GameObject.Find("Host-Position").transform.position.y),
                                                                       Quaternion.Euler(new Vector2(0, 0)));
            for (int i=1;i<playerSits.Count;i++)
            {
                playerSits[i].transform.SetPositionAndRotation(new Vector2(GameObject.Find("Position-Player-"+i).transform.position.x,
                                                                           GameObject.Find("Position-Player-" + i).transform.position.y),
                                                                           Quaternion.Euler(new Vector2(0,0)));
            }

            playerGameObject.GetComponent<PlayerSetup>().Initialize(_player.ActorNumber, _player.NickName);
            playerListGameObjects.Add(_player.ActorNumber, playerGameObject);
            
        }      

        
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " is member of " + PhotonNetwork.CurrentRoom.PlayerCount);

        GameObject playerGameObject = Instantiate(GameManager.toStatic.playerPrefab);
        playerSits.Add(playerGameObject);
        playerSits[0].transform.SetPositionAndRotation(new Vector2(GameObject.Find("Host-Position").transform.position.x,
                                                                   GameObject.Find("Host-Position").transform.position.y),
                                                                   Quaternion.Euler(new Vector2(0, 0)));
        for (int i = 1; i < playerSits.Count; i++)
        {
            playerSits[i].transform.SetPositionAndRotation(new Vector2(GameObject.Find("Position-Player-" + i).transform.position.x,
                                                                       GameObject.Find("Position-Player-" + i).transform.position.y),
                                                                       Quaternion.Euler(new Vector2(0, 0)));
        }

        playerGameObject.GetComponent<PlayerSetup>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);
        playerListGameObjects.Add(newPlayer.ActorNumber, playerGameObject);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //when player leave room => active startgame button
        }

        
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(LobbyPanel.name);
        foreach (GameObject playerlistGameObject in playerListGameObjects.Values)
          Destroy(playerlistGameObject);

        playerSits.Clear();
        playerListGameObjects.Clear();
        playerListGameObjects = null;

        PKManager.toStatic.playerEnterRoom = false;

    }

    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cachedRoomList.Clear();
    }

   
    void OnJoinRoomButtonClicked(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(_roomName);
        

    }

    void ClearRoomListView()
    {
        foreach (var roomListGameObject in roomListGameObject.Values)
        {
            Destroy(roomListGameObject);
        }

        roomListGameObject.Clear();
    }


    #endregion

    #region Public Methods
    public void ActivatePanel(string panelActivated)
    {
        EnterGamePanel.SetActive(panelActivated.Equals(EnterGamePanel.name));
        ConnectionStatusPanel.SetActive(panelActivated.Equals(ConnectionStatusPanel.name));
        LobbyPanel.SetActive(panelActivated.Equals(LobbyPanel.name));
        CreateRoom.SetActive(panelActivated.Equals(CreateRoom.name));

        GameManager.toStatic.PokdengGame.SetActive(panelActivated.Equals(GameManager.toStatic.PokdengGame.name));

        if (LobbyPanel.active)
            OnShowRoom();


    }

    public void ConnectToPhotonSever()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            ActivatePanel(ConnectionStatusPanel.name);
            ActivatePanel(EnterGamePanel.name);
        }
    }
    #endregion

    #region UI Button Callbacks
    public void OnEnterFirstNameClicked()
    {
        string _playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(_playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = _playerName;
            PhotonNetwork.ConnectUsingSettings();

            playerName.text = _playerName;

        }
        else
            Debug.Log("Playername is invalid!");
    }

    public void OnEnterCreateRoomClicked()
    {
        string roomName = roomNameInputField.text;
		if (!string.IsNullOrEmpty(roomName) && roomNameInputField.text != null) //User have to fill room name
        {
            roomName = "" + Random.Range(0, 10000) + " " + roomName;
            
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10; //set count of play in room
            roomOptions.IsOpen = true;    //on off room
            roomOptions.IsVisible = true;

            //playertoHost = true;

            PhotonNetwork.CreateRoom(roomName, roomOptions);

            //PKManager.toStatic.Invoke("InstanceHost",1);
       
        }
        else
            Debug.Log("Room Name is empty");

        
    }

    public void OnEnterCancelCreateRoomClicked()
    {
        ActivatePanel(CreateRoom.name);
        ActivatePanel(LobbyPanel.name);
    }

    public void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();  
    }

    public void OnShowRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            
        }
        ListRoom.SetActive(true) ;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);

            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                    cachedRoomList.Remove(room.Name);
            }
            else
            {
                if(cachedRoomList.ContainsKey(room.Name)) //update cachedRoom list      
                    cachedRoomList[room.Name] = room;
                else
                    cachedRoomList.Add(room.Name, room); // add the new room to the cached room
                
            }
            
        }

        foreach(RoomInfo room in cachedRoomList.Values)
        {
            GameObject roomListEntry = Instantiate(roomListEntryPrefab);
            roomListEntry.transform.SetParent(roomListParent.transform);
            roomListEntry.transform.localScale = Vector3.one;

            roomListEntry.transform.Find("Text-Name").GetComponent<Text>().text = room.Name;
            roomListEntry.transform.Find("Text-CountPlayer").GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;
            roomListEntry.transform.Find("BT-JoinRoom").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomButtonClicked(room.Name));

            roomListGameObject.Add(room.Name, roomListEntry);

        }
    }

    #endregion

}


         /*
            GameObject playerListGameObject = Instantiate(playerListPrefab);
            playerListGameObject.transform.SetParent(playerlistContent.transform);
            playerListGameObject.transform.localScale = Vector3.one;

            playerListGameObject.transform.Find("Text-Name").GetComponent<Text>().text = _player.NickName;

            if (_player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                playerListGameObject.transform.Find("Text-Status").GetComponent<Text>().text = "YOU";
            else
                playerListGameObject.transform.Find("Text-Status").GetComponent<Text>().text = "Other";

            playerListGameObjects.Add(_player.ActorNumber, playerListGameObject);


         */