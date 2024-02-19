using System;
using System.Collections.Generic;
using Data;
using Game.Ball;
using Game.Core;
using UnityEngine;

namespace Game.Score
{
    public class PlayersScoreCalculator : StateHandler
    {
        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private float minY;
        [SerializeField] private float minScore;
        [SerializeField] private float maxScore;
        [SerializeField] private float maxZ;


        public static event Action<PlayersScoreList> PlayersScoreListChanged;


        struct BallData
        {
            public int playerId;
            public string ballId;
            public Vector3 position;
            public float score;
        }


        private List<PlayerScore> playersScore = new();
        private Dictionary<int, Dictionary<string, BallData>> playersBalls = new();


        public List<PlayerScore> GetPlayersScore()
        {
            return playersScore;
        }


        private float ConvertPosToScore(Vector3 position)
        {
            float score = 0f;

            if (position.y < minY)
            {
                return score;
            }

            score = Mathf.Lerp(minScore, maxScore, position.z / maxZ);

            return score;
        }


        private void OnEnable()
        {
            PositionTeller.PositionValue += HandlePosition;
            SpawnTeller.BallSpawned += OnBallSpawned;
            SubscribeStates();
        }

        private void OnDisable()
        {
            PositionTeller.PositionValue -= HandlePosition;
            SpawnTeller.BallSpawned -= OnBallSpawned;
            UnSubscribeStates();
        }


        private void Update()
        {
            if (playersBalls.Count == 0) return;

            var playerScoreList = ToPlayerScoreList(playersBalls);
            playersScore = playerScoreList.playerScores;
            if (playerScoreList.playerScores.Count == 0)
            {
                return;
            }

            PlayersScoreListChanged?.Invoke(playerScoreList);
        }


        // subscribers

        private void OnBallSpawned(int playerId, string ballId)
        {
            Dictionary<string, BallData> balldataDic = new();
            if (playersBalls.TryGetValue(playerId, out var ballDatas))
            {
                balldataDic = ballDatas;
            }

            var ballData = new BallData();
            ballData.ballId = ballId;
            ballData.score = 0;
            ballData.position = Vector3.zero;
            balldataDic.Add(ballId, ballData);

            playersBalls[playerId] = balldataDic;
        }

        protected override void WaitingForRematching(float remainedTime)
        {
            if (isWaitingForRematching == false)
            {
                playersBalls = new();
            }

            base.WaitingForRematching(remainedTime);
        }


        private void HandlePosition(int playerId, string ballId, Vector3 position)
        {
            Debug.Log("Postion teller invoking");
            if (playersBalls.TryGetValue(playerId, out var ballDatas))
            {
                Debug.Log("Postion teller found player");

                if (ballDatas.TryGetValue(ballId, out var ballData))
                {
                    ballData.position = position;
                    ballData.score = ConvertPosToScore(ballData.position);
                    ballDatas[ballId] = ballData;
                    Debug.Log("Postion teller update score" + ballData.score);
                }
            }
        }


        //


        private PlayersScoreList ToPlayerScoreList(Dictionary<int, Dictionary<string, BallData>> ballData)
        {
            PlayersScoreList playersScoreList = new PlayersScoreList();
            playersScoreList.playerScores = new();
            foreach (var keyValuePair in ballData)
            {
                var score = 0f;
                foreach (var valuePair in keyValuePair.Value)
                {
                    score += valuePair.Value.score;
                    Debug.Log("ToPlayerScore score::" + score);
                }

                playersScoreList.playerScores.Add(new PlayerScore(keyValuePair.Key, score));
            }

            return playersScoreList;
        }
    }
}