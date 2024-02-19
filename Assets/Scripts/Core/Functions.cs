
using UnityEngine;


namespace Core.Util
{
    public class Functions
    {
        public static float SumerizeFloat(float value)
        {
            return Mathf.RoundToInt(value * 100) / 100f;
        }
    }
}



