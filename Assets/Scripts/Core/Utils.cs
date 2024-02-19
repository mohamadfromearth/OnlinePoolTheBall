

using UnityEngine;

namespace Core.Util
{
    public class Utils : MonoBehaviour
    {
        public DataStore DataStore => _dataStore;


        private DataStore _dataStore = new DataStore();
    }
}
