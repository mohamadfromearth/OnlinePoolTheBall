namespace Data
{
    public struct PlayerScore
    {
        public int playerId;
        public float score;


        public PlayerScore(int playerId, float score)
        {
            this.playerId = playerId;
            this.score = score;
        }
    }
}