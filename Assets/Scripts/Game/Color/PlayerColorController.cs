using System.Collections.Generic;
using UnityEngine;

public class PlayerColorController : MonoBehaviour
{
    private Dictionary<int, Color> colorDictionary = new();
    [SerializeField] private List<Color> _colorList;
    private Stack<Color> _colors = new ();

    private void Start()
    {
        foreach (var color in _colorList)
        {
            _colors.Push(color);
        }
    }


    private int counter = 0;


    public Color GetPlayerColor(int playerId)
    {
        if (colorDictionary.TryGetValue(playerId, out var color))
        {
            return color;
        }

        if (_colors.TryPop(out Color stackColor))
        {
            colorDictionary.Add(playerId, stackColor);
            return stackColor;
        }

        return Color.white;
    }
}