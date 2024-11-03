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
            Debug.LogError("GridSpawner �� ��������.");
            return;
        }

        gridSpawner.InitializeGrid(level);  // �������� ������� ��������� ��� ��������
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
    public GridTemplatesData templatesData; // ScriptableObject � ���������

    public void GenerateGrid(int difficultyLevel)
    {
        // �������� �� ������� �������� ��� ������� ������ ���������
        if (templatesData == null || difficultyLevel < 1 || difficultyLevel > templatesData.templates.Count)
        {
            Debug.LogError("������������ ������� ��������� ��� ����������� ������� � templatesData.");
            return;
        }

        // ��������� ������� �� ������ ������ ���������
        GridTemplate selectedTemplate = templatesData.templates[difficultyLevel - 1];
        int gridSize = selectedTemplate.gridSize;

        // �������� ��������������� ������ ���� ��� ������ ���������
        List<string> wordList = GetWordListForLevel(difficultyLevel);

        // ��������� ��������� ����� � �������������� �������
        (char[,] grid, List<string> usedWords) = LetterGridGenerator.GenerateLetterGrid(gridSize, selectedTemplate, wordList);

        // �������� ����� � ������ ���� � GridSpawner ��� ������������
        if (gridSpawner != null)
        {
            gridSpawner.SpawnGrid(grid, usedWords);
        }
        else
        {
            Debug.LogError("GridSpawner �� �����.");
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