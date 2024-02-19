using System;
using UnityEngine;

namespace Core
{
    public class IdGenerator : MonoBehaviour
    {
        public string id;


        private void Awake()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}