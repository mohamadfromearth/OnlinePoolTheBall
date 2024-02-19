using Game.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Ui.Game
{
    public class WaitingForGameStartTimer : StateHandler
    {
        // events


        [SerializeField] private float waitTimeInSeconds = 20f;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Image background;


        private void OnEnable()
        {
            SubscribeStates();
        }

        private void OnDisable()
        {
            UnSubscribeStates();
        }


        // subscribers


        protected override void WaitingForPlayers(float remainedTime)
        {
            if (isWaitingForPlayer == false)
            {
                background.enabled = false;
                timeText.enabled = false;
            }

            base.WaitingForPlayers(remainedTime);
        }


        protected override void WaitingForPlaying(float remainedTime)
        {
            if (isWaitingForPlaying == false)
            {
                background.enabled = true;
                timeText.enabled = true;
            }

            int passedTimeToInt = (int)remainedTime;
            timeText.text = passedTimeToInt.ToString();

            base.WaitingForPlaying(remainedTime);
        }


        protected override void Playing(float remainedTime)
        {
            if (isPlaying == false)
            {
                background.enabled = false;
                timeText.enabled = false;
            }

            base.Playing(remainedTime);
        }
    }
}