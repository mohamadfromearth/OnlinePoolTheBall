using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Ui.Game
{
    public class PlayerDataItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerScoreText;
        [SerializeField] private TextMeshProUGUI playerRemainedBallText;

        [SerializeField] private Image background;


        public void SetPlayerNameScoreText(string text)
        {
            playerNameText.text = text;
        }

        public void SetPlayerRemainedBallText(string text)
        {
            playerRemainedBallText.text = text;
        }

        public void SetPlayerScoreText(string text)
        {
            playerScoreText.text = text;
        }


        public void SetBackgroundColor(Color color)
        {
           // background.color = color;
        }
    }
}