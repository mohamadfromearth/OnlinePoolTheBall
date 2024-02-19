using System;
using Fusion;
using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game.Ball
{
    public class StopTeller : NetworkBehaviour
    {
        // events
        public static event Action<int> Stop;

        public float stopThreshold = 0.01f;


        private Rigidbody rg;


        private bool isStopping = false;

        private bool isStopped = false;

        private bool lastFrameIsPressed = false;


        public override void Spawned()
        {
            rg = GetComponent<Rigidbody>();
        }


        private void Start()
        {
            rg = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            if (rg.velocity.magnitude < stopThreshold && isStopping && isStopped == false)
            {
                isStopped = true;
                Stop?.Invoke(Object.InputAuthority);
            }

            if (HasInputAuthority == false) return;
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                lastFrameIsPressed = true;
            }
            else if (lastFrameIsPressed)
            {
                RPC_toggleIsStopping();
            }
        }


        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_STOP()
        {
            Debug.Log("RPC_STOP");
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_toggleIsStopping()
        {
            isStopping = true;
        }
    }
}