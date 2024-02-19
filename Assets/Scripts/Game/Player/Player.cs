using Core.Consts;
using Core.Util;
using Data;
using Fusion;
using Game.Network;
using UnityEngine;


namespace Game.Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef ballPrefab;
        


        private Utils _utils;

        private void Awake()
        {
            _utils = FindObjectOfType<Utils>();
        }

        private void Start()
        {
        }

        private void OnEnable()
        {
            MyNetworkRunnerCallbacks.OnNewSceneLoaded += HandleOnNewSceneLoaded;
            PlayerTurnController.TurnChanged += OnTurnChanged;
        }

        private void OnDisable()
        {
            MyNetworkRunnerCallbacks.OnNewSceneLoaded -= HandleOnNewSceneLoaded;
            PlayerTurnController.TurnChanged -= OnTurnChanged;
        }


        public override void Spawned()
        {
            if (Object.HasStateAuthority) return;
            if (Object.HasInputAuthority)
            {
                var name = _utils.DataStore.GetData<PlayerData>(Consts.DATA_STORE_KEY_PLAYER_DATA).name;
                Rpc_SendPlayerData(name, Object.InputAuthority);
            }
        }


        private void HandleOnNewSceneLoaded()
        {
            if (Object.HasInputAuthority)
            {
                var name = _utils.DataStore.GetData<PlayerData>(Consts.DATA_STORE_KEY_PLAYER_DATA).name;
                Rpc_SendPlayerData(name, Object.InputAuthority);
                Debug.Log("OnNew scenec loaded");
            }
        }


        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void Rpc_SendPlayerData(string name, PlayerRef playerRef)
        {
            Debug.Log("Getting Rpc");
            var playerController = FindObjectOfType<PlayerDataController>();
            playerController.AddPlayerData(new PlayerData(name, 0, playerRef.PlayerId, 9));
        }


        // subscribers
        private void OnTurnChanged(int playerId)
        {
            if (playerId == Object.InputAuthority.PlayerId)
            {
                Runner.Spawn(ballPrefab, null, null, Object.InputAuthority);
            }
        }
    }
}