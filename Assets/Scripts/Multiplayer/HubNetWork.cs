using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class HubNetWork : MonoBehaviourPunCallbacks
    {
        [SerializeField] Button joinRoomButton;
        [SerializeField] Button leaveRoomButton;
        [SerializeField] GameObject roomItemPrefab;
        [SerializeField] string gameSceneName = "GameScene";
        [SerializeField] int maxPlayersPerRoom = 2;
        private void Start()
        {
            leaveRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Already connected to Photon Network.");
            }
            else
            {
                Debug.Log("Connecting to Photon Network...");
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
            }
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            
            Debug.Log($"Player entered room. {newPlayer.ActorNumber}");
            if(PhotonNetwork.CountOfPlayers >= maxPlayersPerRoom)
                PhotonNetwork.LoadLevel(gameSceneName);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Master Server.");
            PhotonNetwork.JoinLobby();
        }
        
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Photon Lobby.");
            joinRoomButton.gameObject.SetActive(true);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomName:"room 123",roomOptions: new RoomOptions()
            {
                MaxPlayers = maxPlayersPerRoom,
            });
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("Joined a room successfully.");
            joinRoomButton.gameObject.SetActive(false);
            leaveRoomButton.gameObject.SetActive(true);
            if (maxPlayersPerRoom == 1)
            {
                PhotonNetwork.LoadLevel(gameSceneName);
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            Debug.Log("Room list updated. Number of rooms: " + roomList.Count);
            Transform roomListParent = roomItemPrefab.transform.parent;
            while (roomListParent.childCount > 1)
            {
                Destroy(roomListParent.GetChild(1).gameObject);
            }
            foreach (RoomInfo room in roomList)
            { 
                var go = Instantiate(roomItemPrefab, roomListParent);
                go.name = room.Name;
                go.SetActive(true);
            }
        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                leaveRoomButton.gameObject.SetActive(false);
                joinRoomButton.gameObject.SetActive(true);
                Debug.Log("Left the room.");
            }
            else
            {
                Debug.LogWarning("Not currently in a room to leave.");
            }
        }
    }
}
