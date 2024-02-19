using System;
using System.Collections.Generic;


namespace Data
{
    
 
    [Serializable]
    public struct RematchData
    {
        public int playerId;
        public bool isReady;


        public RematchData(int playerId, bool isReady)
        {
            this.playerId = playerId;
            this.isReady = isReady;
        }
    
    }   
    
    
    [Serializable]
    public struct RematchDataList
    {
        public List<RematchData> rematchDatas;

        public RematchDataList(List<RematchData> rematchDatas)
        {
            this.rematchDatas = rematchDatas;
        }
    }
    
    
    
}



