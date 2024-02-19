using Core;


namespace Game.Ball
{
    public class Despawner : NetworkedStateHandler
    {
        private void OnEnable()
        {
            SubscribeStates();
        }

        private void OnDisable()
        {
            UnSubscribeStates();
        }


        protected override void WaitingForRematching(float remainedTime)
        {
            if (isWaitingForRematching == false && HasStateAuthority)
            {
                Runner.Despawn(Object);
            }

            base.WaitingForRematching(remainedTime);
        }
    }
}