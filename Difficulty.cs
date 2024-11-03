using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public GridSpawner gridSpawner;
    public GameObject gameObject;
    public GameObject gameObjectToActivated;
    public GameObject gameObjectPauseButtons;
    public void GenerateGrid(int level)
    {
        if (gridSpawner == null)
        {
            Debug.LogError("GridSpawner не назначен.");
            return;
        }

        gridSpawner.InitializeGrid(level);  // ѕередаем уровень сложности как параметр
        gameObjectToActivated.SetActive(true);
        gameObjectPauseButtons.SetActive(true);
        gameObject.SetActive(false);
    }
}










/*using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public GridSpawner gridSpawner;
    public GridTemplatesData templatesData; // ScriptableObject с шаблонами

    public void GenerateGrid(int difficultyLevel)
    {
        // ѕроверка на наличие шаблонов дл€ данного уровн€ сложности
        if (templatesData == null || difficultyLevel < 1 || difficultyLevel > templatesData.templates.Count)
        {
            Debug.LogError("Ќедопустимый уровень сложности или отсутствуют шаблоны в templatesData.");
            return;
        }

        // ѕолучение шаблона на основе уровн€ сложности
        GridTemplate selectedTemplate = templatesData.templates[difficultyLevel - 1];
        int gridSize = selectedTemplate.gridSize;

        // ѕолучаем соответствующий список слов дл€ уровн€ сложности
        List<string> wordList = GetWordListForLevel(difficultyLevel);

        // √енераци€ буквенной сетки с использованием шаблона
        (char[,] grid, List<string> usedWords) = LetterGridGenerator.GenerateLetterGrid(gridSize, selectedTemplate, wordList);

        // ѕередача сетки и списка слов в GridSpawner дл€ визуализации
        if (gridSpawner != null)
        {
            gridSpawner.SpawnGrid(grid, usedWords);
        }
        else
        {
            Debug.LogError("GridSpawner не задан.");
        }

        gameObject.SetActive(false);
    }

    private List<string> GetWordListForLevel(int level)
    {
        switch (level)
        {
            case 1: return WordLists.FourLetterWords;
            case 2: return WordLists.FiveLetterWords;
            case 3: return WordLists.SixLetterWords;
            case 4: return WordLists.SevenLetterWords;
            default: return WordLists.FourLetterWords;
        }
    }
}
*/