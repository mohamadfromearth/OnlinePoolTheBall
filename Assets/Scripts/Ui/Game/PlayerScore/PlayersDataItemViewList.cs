using System;
using System.Collections;
using System.Collections.Generic;
using Core.Util;
using Data;
using Game.Player;
using Game.Score;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui.Game
{
    public class PlayersDataItemViewList : MonoBehaviour
    {
        [FormerlySerializedAs("scoreItemViewPrefab")] [SerializeField]
        private PlayerDataItemView dataItemViewPrefab;

        private Dictionary<int, PlayerDataItemView> scoreItemViewDictionary =
            new Dictionary<int, PlayerDataItemView>();

        private List<PlayerData> playersData;

        private int turnPlayerId = -1;


        public static event Action OnPlayerScoreBoarEnabled;


        private void Start()
        {
        }

        private void OnEnable()
        {
            PlayerDataController.PlayerListChangedEvent += HandlePlayerListChangedEvent;
            PlayerTurnController.TurnChanged += OnTurnChanged;
            PlayersScoreCalculator.PlayersScoreListChanged += OnScoreChange;
        }

        private void OnDisable()
        {
            PlayerDataController.PlayerListChangedEvent -= HandlePlayerListChangedEvent;
            PlayerTurnController.TurnChanged -= OnTurnChanged;
            PlayersScoreCalculator.PlayersScoreListChanged -= OnScoreChange;
        }

        private void HandlePlayerListChangedEvent(List<PlayerData> playerDataList)
        {
            playersData = playerDataList;
            var removedPlayerIds = new List<int>();
            foreach (var playerScoreItemView in scoreItemViewDictionary)
            {
                var isExist = false;
                foreach (var playerData in playerDataList)
                {
                    if (playerData.id == playerScoreItemView.Key)
                    {
                        isExist = true;
                    }
                }

                if (!isExist)
                {
                    Destroy(playerScoreItemView.Value.gameObject);
                    removedPlayerIds.Add(playerScoreItemView.Key);
                }
            }

            foreach (var removedPlayerId in removedPlayerIds)
            {
                scoreItemViewDictionary.Remove(removedPlayerId);
            }

            foreach (var playerData in playerDataList)
            {
                if (scoreItemViewDictionary.TryGetValue(playerData.id, out var scoreItemView))
                {
                    scoreItemView.SetPlayerNameScoreText(playerData.name);
                    scoreItemView.SetPlayerRemainedBallText(playerData.remainedBall.ToString());
                    if (playerData.id == turnPlayerId)
                    {
                        scoreItemView.SetBackgroundColor(Color.green);
                    }
                    else
                    {
                        scoreItemView.SetBackgroundColor(Color.magenta);
                    }
                }
                else
                {
                    var scoreItem = Instantiate(dataItemViewPrefab, Vector3.zero, Quaternion.identity, transform);
                    scoreItem.SetPlayerNameScoreText(playerData.name);
                    scoreItem.SetPlayerRemainedBallText(playerData.remainedBall.ToString());
                    scoreItemViewDictionary[playerData.id] = scoreItem;
                    if (playerData.id == turnPlayerId)
                    {
                        scoreItem.SetBackgroundColor(Color.green);
                    }
                    else
                    {
                        scoreItem.SetBackgroundColor(Color.magenta);
                    }
                }
            }
        }

        private PlayerData GetPlayerDataById(int playerId)
        {
            foreach (var playerData in playersData)
            {
                if (playerId == playerData.id)
                {
                    return playerData;
                }
            }

            return null;
        }

        private void OnTurnChanged(int playerId)
        {
            turnPlayerId = playerId;
            HandlePlayerListChangedEvent(playersData);
        }

        private void OnScoreChange(PlayersScoreList playersScoreList)
        {
            foreach (var playerScore in playersScoreList.playerScores)
            {
                if (scoreItemViewDictionary.TryGetValue(playerScore.playerId, out var itemView))
                {
                    var playerData = GetPlayerDataById(playerScore.playerId);
                    if (playerData != null)
                    {
                        itemView.SetPlayerNameScoreText(playerData.name);
                        itemView.SetPlayerScoreText(Functions.SumerizeFloat(playerData.score).ToString());
                    }
                }
            }
        }
    }
}