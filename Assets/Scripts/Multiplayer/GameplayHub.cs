using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Multiplayer
{
    public class GameplayHub : MonoBehaviour
    {
        [SerializeField] GameObject playerPrefab;

        private void Start()
        {
            Vector2 randomPosition = Random.insideUnitCircle * 5f;
            Vector3 playerPosition = new  Vector3(randomPosition.x,1f, randomPosition.y);
            PhotonNetwork.Instantiate(
                playerPrefab.name, 
                playerPosition, 
                Quaternion.identity, 
                0, 
                new object[] { PhotonNetwork.LocalPlayer.ActorNumber }
            );
        }
    }
}