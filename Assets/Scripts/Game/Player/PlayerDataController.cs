using System;
using System.Collections.Generic;
using Data;
using Fusion;
using Game.Ball;
using Game.Network;
using Game.Score;
using UnityEngine;

namespace Game.Player
{
    public class PlayerDataController : NetworkedStateHandler
    {
        public static event Action<List<PlayerData>> PlayerListChangedEvent;


        private List<PlayerData> playerDataList = new();


        [Networked(OnChanged = nameof(OnPlayerDataListChanged))]
        private NetworkString<_512> _playerDataJson { get; set; }

        public void AddPlayerData(PlayerData playerData)
        {
            playerDataList.Add(playerData);
            _playerDataJson = JsonUtility.ToJson(new PlayerDataParent(playerDataList));
        }


        public List<PlayerData> GetPlayerDataList()
        {
            return playerDataList;
        }


        public void UpdateScores(List<PlayerScore> playerScoreList)
        {
            foreach (var playerScore in playerScoreList)
            {
                foreach (var playerData in playerDataList)
                {
                    if (playerData.id == playerScore.playerId)
                    {
                        playerData.score = playerScore.score;
                    }
                }
            }
        }

        public int GetPlayerRemainedBall(int playerId)
        {
            if (playerDataList.Count == 0) return -1;

            foreach (var playerData in playerDataList)
            {
                if (playerData.id == playerId)
                {
                    return playerData.remainedBall;
                }
            }

            return -1;
        }


        public static void OnPlayerDataListChanged(Changed<PlayerDataController> playerController)
        {
            Debug.Log("Player data changed " + playerController.Behaviour._playerDataJson);


            var playerList = JsonUtility
                .FromJson<PlayerDataParent>(playerController.Behaviour._playerDataJson.ToString())
                .playerDataList;
            playerController.Behaviour.playerDataList = playerList;
            PlayerListChangedEvent?.Invoke(playerList);
        }


        private void OnEnable()
        {
            MyNetworkRunnerCallbacks.OnPlayerLeftEvent += HandleOnPlayerLeft;
            SpawnTeller.BallSpawned += OnBallSpawned;
            PlayerTurnController.TurnChanged += OnTurnChanged;
            PlayersScoreCalculator.PlayersScoreListChanged += OnPlayersScoreListChange;
            SubscribeStates();
        }

        private void OnDisable()
        {
            MyNetworkRunnerCallbacks.OnPlayerLeftEvent -= HandleOnPlayerLeft;
            SpawnTeller.BallSpawned -= OnBallSpawned;
            GameStateController.GameStateWaitingForRematching.Update -= WaitingForRematching;
            PlayerTurnController.TurnChanged -= OnTurnChanged;
            PlayersScoreCalculator.PlayersScoreListChanged -= OnPlayersScoreListChange;
            UnSubscribeStates();
        }


        // subscribers
        private void HandleOnPlayerLeft(PlayerRef player)
        {
            RemovePlayerData(player);
            _playerDataJson = JsonUtility.ToJson(new PlayerDataParent(playerDataList));
        }


        private void OnTurnChanged(int playerId)
        {
            foreach (var playerData in playerDataList)
            {
                if (playerId == playerData.id)
                {
                }
            }
        }


        private void OnBallSpawned(int playerId, string ballId)
        {
            for (int i = 0; i < playerDataList.Count; i++)
            {
                if (playerDataList[i].id == playerId)
                {
                    playerDataList[i].remainedBall -= 1;
                    break;
                }
            }

            _playerDataJson = JsonUtility.ToJson(new PlayerDataParent(playerDataList));
        }


        private void OnPlayersScoreListChange(PlayersScoreList playersScoreList)
        {
            foreach (var playerData in playerDataList)
            {
                foreach (var playerScore in playersScoreList.playerScores)
                {
                    if (playerScore.playerId == playerData.id)
                    {
                        playerData.score = playerScore.score;
                    }
                }
            }
        }


        protected override void WaitingForRematching(float remainedTime)
        {
            if (isWaitingForRematching == false)
            {
                foreach (var playerData in playerDataList)
                {
                    playerData.remainedBall = 1;
                }

                _playerDataJson = JsonUtility.ToJson(new PlayerDataParent(playerDataList));
            }

            base.WaitingForRematching(remainedTime);
        }


        private void RemovePlayerData(int playerId)
        {
            Debug.Log("Handle player left");
            for (int i = 0; i < playerDataList.Count; i++)
            {
                if (playerDataList[i].id == playerId)
                {
                    playerDataList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}