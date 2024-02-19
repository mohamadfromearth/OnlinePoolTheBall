using System;
using Fusion;
using Game.Ball;
using Game.Network;
using UnityEngine;


namespace Game
{
    public class GameStateController : NetworkBehaviour, IGameStateController
    {
        private const int STATE_WAITING_FOR_PLAYERS = 1;
        private const int STATE_WAITING_FOR_PLAYING = 2;
        private const int STATE_PLAYING = 3;
        private const int STATE_ENDING = 4;
        private const int STATE_WAITING_FOR_REMATCHING = 5;


        private GameStateWaitingForPlayers waitingForPlayers = new GameStateWaitingForPlayers();
        private GameStateWaitingForPlaying waitingForPlaying = new GameStateWaitingForPlaying();
        private GameStatePlaying playing = new GameStatePlaying();
        private GameStateEnding ending = new GameStateEnding();
        private GameStateWaitingForRematching rematching = new GameStateWaitingForRematching();
        private int playersCount = 0;

        // dependencies
        private RemainedBall _remainedBall;


        [Networked(OnChanged = nameof(OnStateJsonChanged))]
        private NetworkString<_128> currentStateDataJson { get; set; }

        [Networked] private TickTimer stateTimer { get; set; }

        private StateData currentStateData = new StateData(STATE_WAITING_FOR_PLAYERS, 0f);


        private IGameState _gameState;

        private void Start()
        {
            _remainedBall = FindObjectOfType<RemainedBall>();
        }


        [SerializeField] private float waitingForGameStartTime = 20;


        public override void Spawned()
        {
            PlayerJoined(Object.InputAuthority);
            ChangeGameState(waitingForPlayers);
        }


        public override void FixedUpdateNetwork()
        {
            if (_gameState is GameStateWaitingForPlaying && stateTimer.Expired(Runner))
            {
                ChangeGameState(playing);
            }

            if (_gameState is GameStateWaitingForRematching && stateTimer.Expired(Runner))
            {
                ChangeGameState(waitingForPlaying);
            }

            var remainedTime = stateTimer.RemainingTime(Runner);
            _gameState?.Run(remainedTime ?? 0f);


            currentStateData.remainedTime += Runner.DeltaTime;
        }

        private void OnEnable()
        {
            MyNetworkRunnerCallbacks.OnPlayerJoinedEvent += PlayerJoined;
            MyNetworkRunnerCallbacks.OnPlayerLeftEvent += PlayerLeft;
            StopTeller.Stop += BallStopped;
            RematchHandler.Rematching += Rematching;
        }

        private void OnDisable()
        {
            MyNetworkRunnerCallbacks.OnPlayerJoinedEvent -= PlayerJoined;
            MyNetworkRunnerCallbacks.OnPlayerLeftEvent -= PlayerLeft;
            StopTeller.Stop -= BallStopped;
            RematchHandler.Rematching -= Rematching;
        }


        private bool isPassedTimeReachedToStartWaitingTime()
        {
            return currentStateData.remainedTime >= waitingForGameStartTime;
        }

        private void ChangeGameState(IGameState gameState)
        {
            if (Object.HasStateAuthority == false) return;
            currentStateData.remainedTime = 0f;
            _gameState = gameState;
            currentStateData.state = GetGameStateId(gameState);
            if (_gameState is GameStateWaitingForPlaying)
            {
                stateTimer = TickTimer.CreateFromSeconds(Runner, 5f);
            }

            if (_gameState is GameStateWaitingForRematching)
            {
                stateTimer = TickTimer.CreateFromSeconds(Runner, 10f);
            }

            currentStateDataJson = JsonUtility.ToJson(currentStateData);
        }

        private int GetGameStateId(IGameState gameState)
        {
            int stateId = STATE_WAITING_FOR_PLAYERS;
            switch (gameState)
            {
                case GameStateWaitingForPlayers:
                    stateId = STATE_WAITING_FOR_PLAYERS;
                    break;
                case GameStateWaitingForPlaying:
                    stateId = STATE_WAITING_FOR_PLAYING;
                    break;
                case GameStatePlaying:
                    stateId = STATE_PLAYING;
                    break;
                case GameStateEnding:
                    stateId = STATE_ENDING;
                    break;
                case GameStateWaitingForRematching:
                    stateId = STATE_WAITING_FOR_REMATCHING;
                    break;
            }

            return stateId;
        }

        // onChanged
        private static void OnStateJsonChanged(Changed<GameStateController> changed)
        {
            Debug.Log(":::OnStateChanged");
            var behaviour = changed.Behaviour;
            StateData stateData = JsonUtility.FromJson<StateData>(behaviour.currentStateDataJson.ToString());
            behaviour._gameState = behaviour.GetCurrentGameState(stateData);
            behaviour.currentStateData = stateData;
        }

        private IGameState GetCurrentGameState(StateData stateData)
        {
            IGameState gameState = waitingForPlayers;
            switch (stateData.state)
            {
                case STATE_WAITING_FOR_PLAYERS:
                    gameState = waitingForPlayers;
                    break;
                case STATE_WAITING_FOR_PLAYING:
                    gameState = waitingForPlaying;
                    break;
                case STATE_PLAYING:
                    gameState = playing;
                    break;
                case STATE_ENDING:
                    gameState = ending;
                    break;
                case STATE_WAITING_FOR_REMATCHING:
                    gameState = rematching;
                    break;
            }

            return gameState;
        }


// subscribers

        private void PlayerJoined(PlayerRef playerRef)
        {
            playersCount++;
            if (playersCount <= 1) return;
            if (_gameState != waitingForPlayers) return;
            ChangeGameState(waitingForPlaying);
            Debug.Log(":::GameState playerJoined");
        }

        private void PlayerLeft(PlayerRef playerRef)
        {
            playersCount--;
            if (playersCount > 1) return;
            Debug.Log("Player left " + playersCount);
            ChangeGameState(waitingForPlayers);
        }


        private void BallStopped(int playerId)
        {
            if (_remainedBall.GetRemainedBall() == 0)
            {
                ChangeGameState(ending);
            }
        }


        private void Rematching()
        {
            ChangeGameState(rematching);
        }


        #region GameStates

        public class GameStateWaitingForPlayers : IGameState
        {
            public static event Action<float> Update;

            public void Run(float passedTime)
            {
                Update?.Invoke(passedTime);
            }
        }

        public class GameStateWaitingForPlaying : IGameState
        {
            public static event Action<float> Update;

            public void Run(float passedTime)
            {
                Update?.Invoke(passedTime);
            }
        }

        public class GameStatePlaying : IGameState
        {
            public static event Action<float> Update;

            public void Run(float passedTime)
            {
                Update?.Invoke(passedTime);
            }
        }

        public class GameStateEnding : IGameState
        {
            public static event Action<float> Update;

            public void Run(float passedTime)
            {
                Update?.Invoke(passedTime);
            }
        }


        public class GameStateWaitingForRematching : IGameState
        {
            public static event Action<float> Update;

            public void Run(float passedTime)
            {
                Update?.Invoke(passedTime);
            }
        }

        #endregion
    }
}