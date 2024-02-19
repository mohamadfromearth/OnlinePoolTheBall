using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game.Ball
{
    public class Mover : NetworkBehaviour
    {
        private NetworkRigidbody rg;


        private float force = 100f;
        private bool isPullingAllowed = true;
        private bool lastFrameIsPressed = false;

        private bool isTouching = false;

        private void Start()
        {
            rg = GetComponent<NetworkRigidbody>();
        }


        public override void FixedUpdateNetwork()
        {
            if (Object.HasInputAuthority == false)
            {
                Debug.Log("You dont have a auth sir ..!");
                return;
            }


            if (Touchscreen.current.primaryTouch.press.isPressed && isPullingAllowed)
            {
                lastFrameIsPressed = true;
                Vector3 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

                Ray ray = Camera.main.ScreenPointToRay(touchPos);

                // Perform the raycast
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 worldPosition = hit.point;

                    RPC_Move(worldPosition.x, 0f, worldPosition.z);
                }
            }
            else if (lastFrameIsPressed)
            {
                lastFrameIsPressed = false;
                isPullingAllowed = false;
            }
        }


        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_Move(float x, float y, float z)
        {
            Debug.Log("Pulling");

            Vector3 direction = (new Vector3(x, y, z) - transform.position).normalized;
            rg.Rigidbody.AddForce(direction  * force);

        }
    }
}