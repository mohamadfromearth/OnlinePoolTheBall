using System.Collections.Generic;
using System.Linq;
using Core.Util;
using Data;
using Game;
using Game.Player;
using Game.Score;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Ui.Game
{
    public class EndingPanel : NetworkedStateHandler
    {
        [SerializeField] private RankItem _rankItemPrefab;
        [SerializeField] private GameObject rankList;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI winnerText;
        [SerializeField] private TextMeshProUGUI rematchTimer;
        [SerializeField] private Image rematchTimerBg;


        [SerializeField] private RematchHandler _rematchHandler;
        private PlayerDataController _playerDataController;
        private Dictionary<int, RankItem> rankItemsDictionary = new();


        public async void OnLeaveClick()
        {
            await Runner.Shutdown();
            SceneManager.LoadScene("MenuScene");
        }


        public void OnRematchClick()
        {
            _rematchHandler.Rpc_rematch(Runner.LocalPlayer.PlayerId);
        }

        private void Start()
        {
            _playerDataController = FindObjectOfType<PlayerDataController>();
        }


        private void OnEnable()
        {
            SubscribeStates();
            RematchHandler.rematchDataListChange += ReadyPlayerListChanged;
        }

        private void OnDisable()
        {
            UnSubscribeStates();
            RematchHandler.rematchDataListChange -= ReadyPlayerListChanged;
        }


        protected override void WaitingForRematching(float remainedTime)
        {
            if (isWaitingForRematching == false)
            {
                rematchTimerBg.gameObject.SetActive(true); 
                rematchTimer.gameObject.SetActive(true);
            }

            var intRemainedTime = (int)remainedTime;
            rematchTimer.text = intRemainedTime.ToString();

            base.WaitingForRematching(remainedTime);
        }

        protected override void WaitingForPlaying(float remainedTime)
        {
            if (isWaitingForPlaying == false)
            {
                panel.gameObject.SetActive(false);
                ClearRankList();
                rematchTimerBg.gameObject.SetActive(false); 
                rematchTimer.gameObject.SetActive(false);
            }

            base.WaitingForPlaying(remainedTime);
        }

        protected override void WaitingForPlayers(float remainedTime)
        {
            if (isWaitingForPlayer == false)
            {
                panel.gameObject.SetActive(false);
            }

            base.WaitingForPlayers(remainedTime);
        }


        protected override void Playing(float remainedTime)
        {
            if (isPlaying == false)
            {
                panel.gameObject.SetActive(false);
            }

            base.Playing(remainedTime);
        }


        protected override void Ending(float remainedTime)
        {
            if (isEnding == false)
            {
                panel.gameObject.SetActive(true);
                Debug.Log("EndingPannel before fill ");

                _playerDataController.UpdateScores(FindObjectOfType<PlayersScoreCalculator>().GetPlayersScore());
                var playerRankingList = MapToRankItem(_playerDataController.GetPlayerDataList());

                foreach (var rankItemData in playerRankingList)
                {
                    RankItem rankItem = Instantiate(_rankItemPrefab, rankList.transform);
                    rankItem.setRankText(rankItemData.rank + " " + rankItemData.playerName);
                    rankItem.setScoreText(Functions.SumerizeFloat(rankItemData.score).ToString());
                    rankItem.SetRankItemRematchState(false);
                    rankItemsDictionary.Add(rankItemData.playerId, rankItem);
                }

                winnerText.text = playerRankingList[0].playerName + " Won";
            }

            base.Ending(remainedTime);
        }


        private void ClearRankList()
        {
            foreach (var keyValuePair in rankItemsDictionary)
            {
                Destroy(keyValuePair.Value.gameObject);
            }

            rankItemsDictionary = new();
        }


        private List<RankItemData> MapToRankItem(List<PlayerData> playerDataList)
        {
            var sortedPlayerData = playerDataList.OrderBy(player => player.score).ToList();
            List<RankItemData> rankItemList = new List<RankItemData>();
            var rank = 1;
            for (int i = sortedPlayerData.Count - 1; i >= 0; i--)
            {
                var playerData = sortedPlayerData[i];
                rankItemList.Add(new RankItemData(rank, playerData.name, playerData.score, playerData.id));
                rank++;
            }

            return rankItemList;
        }


        // subscribers
        private void ReadyPlayerListChanged(List<RematchData> rematchDataList)
        {
            foreach (var rematchData in rematchDataList)
            {
                rankItemsDictionary[rematchData.playerId].SetRankItemRematchState(rematchData.isReady);
            }
        }
    }
}