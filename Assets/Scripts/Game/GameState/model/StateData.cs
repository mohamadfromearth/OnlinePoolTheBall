using UnityEngine.Serialization;


namespace Game
{
    [System.Serializable]
    public struct StateData
    {
        public int state;
        [FormerlySerializedAs("passedTime")] public float remainedTime;

        public StateData(int state, float remainedTime)
        {
            this.state = state;
            this.remainedTime = remainedTime;
        }
    }
}