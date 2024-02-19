using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Fusion;
using Game.Network;
using UnityEngine;


namespace Game
{
    public class RematchHandler : NetworkedStateHandler
    {
        public static Action<List<RematchData>> rematchDataListChange;
        public static Action Rematching;


        private RematchDataList rematchDataList = new RematchDataList(new List<RematchData>());


        [Networked(OnChanged = nameof(OnPlayerIdChange))]
        private NetworkString<_512> rematachDataListJson { get; set; }


        private void OnEnable()
        {
            MyNetworkRunnerCallbacks.OnPlayerLeftEvent += PlayerLeft;
            SubscribeStates();
        }


        private void OnDisable()
        {
            MyNetworkRunnerCallbacks.OnPlayerLeftEvent -= PlayerLeft;
            UnSubscribeStates();
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void Rpc_rematch(int playerId)
        {
            AddId(playerId);
            if (rematchDataList.rematchDatas.Count == 2)
            {
                Rematching?.Invoke();
            }
        }

        private void AddId(int playerId)
        {
            var isExist = false;
            foreach (var rematchData in rematchDataList.rematchDatas)
            {
                if (rematchData.playerId == playerId)
                {
                    isExist = true;
                }
            }

            if (isExist == false)
            {
                rematchDataList.rematchDatas.Add(new RematchData(playerId, true));
            }

            rematachDataListJson = JsonUtility.ToJson(rematchDataList);
        }


        // subscribers


        protected override void Playing(float remainedTime)
        {
            if (isPlaying == false)
            {
                rematchDataList.rematchDatas = new List<RematchData>();
            }

            base.Playing(remainedTime);
        }


        private void PlayerLeft(PlayerRef player)
        {
            var itemToRemove =
                rematchDataList.rematchDatas.Single(rematchData => rematchData.playerId == player.PlayerId);
            rematchDataList.rematchDatas.Remove(itemToRemove);
            rematachDataListJson = JsonUtility.ToJson(rematchDataList);
        }


        // onChange


        public static void OnPlayerIdChange(Changed<RematchHandler> changed)
        {
            var rematchHandler = changed.Behaviour;
            var playerIdList = JsonUtility.FromJson<RematchDataList>(rematchHandler.rematachDataListJson.ToString());
            Debug.Log("PlayerId " + playerIdList.rematchDatas);
            rematchDataListChange?.Invoke(playerIdList.rematchDatas);
        }
    }
}