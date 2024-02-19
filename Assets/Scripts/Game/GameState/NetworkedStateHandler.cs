using Fusion;


namespace Game
{
    public abstract class NetworkedStateHandler : NetworkBehaviour
    {
        protected bool isWaitingForPlayer = false;
        protected bool isWaitingForPlaying = false;
        protected bool isPlaying = false;
        protected bool isEnding = false;
        protected bool isWaitingForRematching = false;


        protected void SubscribeStates()
        {
            GameStateController.GameStateWaitingForPlayers.Update += WaitingForPlayers;
            GameStateController.GameStateWaitingForPlaying.Update += WaitingForPlaying;
            GameStateController.GameStatePlaying.Update += Playing;
            GameStateController.GameStateEnding.Update += Ending;
            GameStateController.GameStateWaitingForRematching.Update += WaitingForRematching;
        }


        protected void UnSubscribeStates()
        {
            GameStateController.GameStateWaitingForPlayers.Update -= WaitingForPlayers;
            GameStateController.GameStateWaitingForPlaying.Update -= WaitingForPlaying;
            GameStateController.GameStatePlaying.Update -= Playing;
            GameStateController.GameStateEnding.Update -= Ending;
            GameStateController.GameStateWaitingForRematching.Update -= WaitingForRematching;
        }


        protected virtual void WaitingForPlayers(float remainedTime)
        {
            isWaitingForPlaying = false;
            isPlaying = false;
            isEnding = false;
            isWaitingForRematching = false;

            if (isWaitingForPlayer == false)
            {
                isWaitingForPlayer = true;
            }
        }

        protected virtual void WaitingForPlaying(float remainedTime)
        {
            isWaitingForPlayer = false;
            isPlaying = false;
            isEnding = false;
            isWaitingForRematching = false;
            if (isWaitingForPlaying == false)
            {
                isWaitingForPlaying = true;
            }
        }


        protected virtual void Playing(float remainedTime)
        {
            isWaitingForPlayer = false;
            isWaitingForPlaying = false;
            isEnding = false;
            isWaitingForRematching = false;


            if (isPlaying == false)
            {
                isPlaying = true;
            }
        }

        protected virtual void Ending(float remainedTime)
        {
            isWaitingForPlayer = false;
            isWaitingForPlaying = false;
            isPlaying = false;
            isWaitingForRematching = false;

            if (isEnding == false)
            {
                isEnding = true;
            }
        }

        protected virtual void WaitingForRematching(float remainedTime)
        {
            isWaitingForPlayer = false;
            isWaitingForPlaying = false;
            isPlaying = false;
            isEnding = false;

            if (isWaitingForRematching == false)
            {
                isWaitingForRematching = true;
            }
        }
    }
}