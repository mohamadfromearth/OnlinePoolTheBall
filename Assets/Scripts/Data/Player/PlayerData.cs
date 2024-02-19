using System;


namespace Data
{
  
    
    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public float score;
        public int remainedBall;

        public PlayerData(string name, float score, int id, int remainedBall)
        {
            this.name = name;
            this.score = score;
            this.id = id;
            this.remainedBall = remainedBall;
        }
    }
    
    
}
