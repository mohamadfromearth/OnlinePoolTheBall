using Fusion;
using TMPro;
using UnityEngine;
using Core.Consts;
using Core.Util;
using Data;
using WebSocketSharp;


namespace Ui
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _playerNameInput;

        private Utils _utils;
        private NetworkRunner _runner;


        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _runner = FindObjectOfType<NetworkRunner>();
            _utils = FindObjectOfType<Utils>();
            if (_runner == null)
            {
                Debug.Log("Runner is null");
            }
        }


        public void OnStartGameClick()
        {
            if (IsValidName(_playerNameInput.text))
            {
                _utils.DataStore.AddData(Consts.DATA_STORE_KEY_PLAYER_DATA,
                    new PlayerData(_playerNameInput.text, 0, -1, 4));
                StartGame(GameMode.AutoHostOrClient);
            }
            else
            {
            }
        }

        private bool IsValidName(string name)
        {
            if (_playerNameInput.text.IsNullOrEmpty())
            {
                Debug.Log("Player name is Empty");
                return false;
            }

            return true;
        }


        private async void StartGame(GameMode gameMode)
        {
            _runner.ProvideInput = true;


            var startGameArgs = new StartGameArgs()
            {
                GameMode = gameMode,
            };
            Debug.Log("Game starting");


            var startGame = await _runner.StartGame(startGameArgs);

            if (startGame.Ok)
            {
                Debug.Log("Game started");
            }
            else
            {
                Debug.Log("Game is not started" + startGame.ErrorMessage);
            }


            _runner.SetActiveScene(Consts.GAME_SCENE_NAME);
        }
    }
}