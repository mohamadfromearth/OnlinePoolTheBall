namespace Data
{
    public struct RankItemData
    {
        public int rank;
        public string playerName;
        public float score;
        public int playerId;

        public RankItemData(int rank, string playerName, float score, int playerId)
        {
            this.rank = rank;
            this.playerName = playerName;
            this.score = score;
            this.playerId = playerId;
        }
    }
}