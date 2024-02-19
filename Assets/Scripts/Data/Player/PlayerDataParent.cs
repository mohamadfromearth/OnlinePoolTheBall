using System.Collections.Generic;


namespace Data
{
    [System.Serializable]
    public class PlayerDataParent
    {
        public List<PlayerData> playerDataList;

        public PlayerDataParent(List<PlayerData> playerDataList)
        {
            this.playerDataList = playerDataList;
        }
    }
}