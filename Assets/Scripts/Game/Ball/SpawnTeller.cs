using System;
using Core;
using Fusion;

namespace Game.Ball
{
    public class SpawnTeller : NetworkBehaviour
    {
        public static event Action<int, string> BallSpawned;


        public override void Spawned()
        {
            BallSpawned?.Invoke(Object.InputAuthority.PlayerId, GetComponent<IdGenerator>().id);
        }
    }  
}

