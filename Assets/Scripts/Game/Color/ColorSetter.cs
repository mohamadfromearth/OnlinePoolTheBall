using Fusion;
using UnityEngine;




namespace Game
{
    public class ColorSetter : NetworkBehaviour
    {
    
    
        public override void Spawned()
        {
            var colorController = FindObjectOfType<PlayerColorController>();
            var color = colorController.GetPlayerColor(Object.InputAuthority);
            var renderer = GetComponentInChildren<Renderer>();
            renderer.material.color = color;
        }
    }  
}

