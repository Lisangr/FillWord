using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridTemplate
{
    public string name;
    public List<Vector2Int> positions; // Позиции для каждого слова
    public List<Color> colors; // Цвета для визуального отображения последовательностей
    public int gridSize;

    public GridTemplate(string name, int gridSize)
    {
        this.name = name;
        this.gridSize = gridSize;
        positions = new List<Vector2Int>();
        colors = new List<Color>();
    }
}
