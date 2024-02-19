using Game.Core;
using Game.Player;



namespace Game.Ball
{
    public class RemainedBall : StateHandler
    {
        private int remainedBall = 0;
        private PlayerDataController _playerDataController;


        public int GetRemainedBall()
        {
            return remainedBall;
        }

        private void OnEnable()
        {
            SubscribeStates();
            SpawnTeller.BallSpawned += OnBallSpawned;
        }

        private void OnDisable()
        {
            UnSubscribeStates();
            SpawnTeller.BallSpawned += OnBallSpawned;
        }


        private void Start()
        {
            _playerDataController = FindObjectOfType<PlayerDataController>();
        }


        protected override void Playing(float remainedTime)
        {
            if (isPlaying == false)
            {
                foreach (var playerData in _playerDataController.GetPlayerDataList())
                {
                    remainedBall += playerData.remainedBall;
                }
            }

            base.Playing(remainedTime);
        }


        protected override void WaitingForPlayers(float remainedTime)
        {
            remainedBall = 0;
            base.WaitingForPlayers(remainedTime);
        }


        protected override void WaitingForPlaying(float remainedTime)
        {
            remainedBall = 0;
            base.WaitingForPlaying(remainedTime);
        }


        private void OnBallSpawned(int playerId, string ballId)
        {
            remainedBall--;
        }
    }
}