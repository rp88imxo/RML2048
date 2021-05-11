using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RML/Color Palette For Tiles")]
public class ColorTilePalette : ScriptableObject
{
    [SerializeField]
    Color[] colorPalette;

    public Color GetColor(int number)
    {
       int index = Mathf.RoundToInt(Mathf.Log(number, 2)) - 1;

       return colorPalette[index];
    }




}
