using System;
using Data;
using Game.Core;
using TMPro;
using UnityEngine;


namespace Ui.Game
{
    public class UiManager : StateHandler
    {
        [SerializeField] private TextMeshProUGUI Title;
        


        private void OnEnable()
        {
            SubscribeStates();
        }

        private void OnDisable()
        {
            UnSubscribeStates();
        }


        protected override void Ending(float remainedTime)
        {
            if (!isEnding)
            {
                Title.enabled = false;
            }

            base.Ending(remainedTime);
        }


        protected override void WaitingForPlaying(float remainedTime)
        {
            if (!isWaitingForPlaying)
            {
                Title.enabled = true;
                Title.text = "Waiting For Playing";
            }

            base.WaitingForPlaying(remainedTime);
        }


        protected override void WaitingForPlayers(float remainedTime)
        {
            if (!isWaitingForPlayer)
            {
                Title.enabled = true;
                Title.text = "Waiting For Players";
            }

            base.WaitingForPlayers(remainedTime);
        }


        protected override void Playing(float remainedTime)
        {
            if (!isPlaying)
            {
                Title.enabled = false;
            }

            base.Playing(remainedTime);
        }
    }
}