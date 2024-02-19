using System;
using System.Linq;
using Fusion;
using Game.Ball;
using UnityEngine;

namespace Game.Player
{
    public class PlayerTurnController : NetworkedStateHandler
    {
        // Actions
        public static event Action<int> TurnChanged;


        // dependency
        private RemainedBall _remainedBall;
        private PlayerDataController _playerDataController;


        // networked
        [Networked(OnChanged = nameof(OnCurrentPlayerIdChanged))]
        private int currentPlayerId { get; set; }

        private int currentPlayerIndex = 0;


        private void Start()
        {
            _remainedBall = FindObjectOfType<RemainedBall>();
            _playerDataController = FindObjectOfType<PlayerDataController>();
        }

        private void OnEnable()
        {
            SubscribeStates();

            StopTeller.Stop += BallStopped;
        }

        private void OnDisable()
        {
            UnSubscribeStates();
            StopTeller.Stop -= BallStopped;
        }


        // onChanged
        private static void OnCurrentPlayerIdChanged(Changed<PlayerTurnController> changed)
        {
            var behaviour = changed.Behaviour;
            Debug.Log("PlayerTurn " + behaviour.currentPlayerId);
            TurnChanged.Invoke(behaviour.currentPlayerId);
        }

        //  subscribers


        protected override void Playing(float remainedTime)
        {
            if (Object.HasStateAuthority == false) return;
            if (isPlaying == false)
            {
                var activePlayers = Runner.ActivePlayers.ToList();
                currentPlayerId = activePlayers[0].PlayerId;
            }

            base.Playing(remainedTime);
        }



        private void GetTurn()
        {
            var activePlayers = Runner.ActivePlayers.ToList();


            if (currentPlayerIndex + 1 < activePlayers.Count)
            {
                currentPlayerIndex++;
            }
            else
            {
                currentPlayerIndex = 0;
            }

            var playerId = activePlayers[currentPlayerIndex];

            var remainedBall = _playerDataController.GetPlayerRemainedBall(playerId);

            if (remainedBall == 0)
            {
                GetTurn();
            }
            else
            {
                currentPlayerId = playerId;
            }
        }

        private void BallStopped(int playerId)
        {
            if (HasStateAuthority == false || _remainedBall.GetRemainedBall() == 0) return;
            GetTurn();
        }
    }
}