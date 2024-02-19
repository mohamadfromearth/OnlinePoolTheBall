using System;
using System.Collections.Generic;

namespace Core.Util
{
    public class DataStore
    {
        private Dictionary<string, Object> _dataDictionary = new Dictionary<string, object>();


        public void AddData(string key, Object data)
        {
            if (data != null)
            {
                _dataDictionary[key] = data;
            }
        }

        public T GetData<T>(string key)
        {
            if (_dataDictionary.TryGetValue(key, out var data))
            {
                return (T)data;
            }

            return default;
        }
    }
}

