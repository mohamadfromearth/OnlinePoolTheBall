using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Ui.Game
{
    public class RankItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Image readyForRematchImage;


        public void setRankText(string text)
        {
            rankText.text = text;
        }

        public void setPlayerNameText(string text)
        {
            playerNameText.text = text;
        }


        public void setScoreText(string text)
        {
            scoreText.text = text;
        }


        public void SetRankItemRematchState(bool isActive)
        {
            readyForRematchImage.enabled = isActive;
        }
    }
}