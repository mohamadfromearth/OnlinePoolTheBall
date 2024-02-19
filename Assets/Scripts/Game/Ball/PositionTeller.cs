using System;
using Core;
using Fusion;
using UnityEngine;


namespace Game.Ball
{
    public class PositionTeller : NetworkBehaviour
    {
        private IdGenerator _idGenerator;

        public static event Action<int, string, Vector3> PositionValue;


        private void Start()
        {
            _idGenerator = GetComponent<IdGenerator>();
        }


        private void FixedUpdate()
        {
            PositionValue?.Invoke(Object.InputAuthority.PlayerId, _idGenerator.id, transform.position);
        }
    }
}


