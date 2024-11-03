using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
//using YG;


public class GridSpawner : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public GameObject cellPrefab;
    private LetterImageMapping letterImageMapping;
    public Text wordsListText;
    public GridTemplatesData[] templatesDataArray;
    public GameObject winPanel;
    private GameObject[,] gridCells;

    private void Start()
    {
        letterImageMapping = GetComponent<LetterImageMapping>();
    }

    public void InitializeGrid(int difficultyLevel)
    {
        if (templatesDataArray == null || templatesDataArray.Length == 0)
        {
            Debug.LogError("������ templatesDataArray ���� ��� �� ���������.");
            return;
        }

        GridTemplate selectedTemplate = GetRandomTemplateForLevel(difficultyLevel);
        currentDifficultyLevel = difficultyLevel;   

        if (selectedTemplate != null)
        {
            List<string> wordList = GetWordListForLevel(difficultyLevel);
            (char[,] grid, List<string> usedWords) = LetterGridGenerator.GenerateLetterGrid(selectedTemplate.gridSize, selectedTemplate, wordList);
            SpawnGrid(grid, usedWords);
        }
        else
        {
            Debug.LogError("������ ��� ���������� ������ ��������� �� ������.");
        }
    }


    private GridTemplate GetRandomTemplateForLevel(int difficultyLevel)
    {
        int expectedGridSize = difficultyLevel + 3;

        foreach (var templatesData in templatesDataArray)
        {
            List<GridTemplate> filteredTemplates = templatesData.templates.FindAll(template => template.gridSize == expectedGridSize);

            if (filteredTemplates.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, filteredTemplates.Count);
                Debug.Log($"������ ������: {filteredTemplates[randomIndex].name} ��� ����� {expectedGridSize}x{expectedGridSize}.");
                return filteredTemplates[randomIndex];
            }
        }

        Debug.LogWarning($"��� ���������� �������� ��� ������ ��������� {difficultyLevel}. �������� ������ �����: {expectedGridSize}x{expectedGridSize}.");
        return null;
    }


    private List<string> GetWordListForLevel(int level)
    {
        switch (level)
        {
            case 1: return WordLists.FourLetterWords;
            case 2: return WordLists.FiveLetterWords;
            case 3: return WordLists.SixLetterWords;
            case 4: return WordLists.SevenLetterWords;
            case 5: return WordLists.EightLetterWords;
            default: return WordLists.FourLetterWords;
        }
    }
    private List<string> wordList;// = new List<string>();
    private int displayWordCount;

    public void SpawnGrid(char[,] grid, List<string> usedWords)
    {
        ClearGrid();
        wordList = new List<string>(usedWords); // ��������� ������ ���� ��� ������� ����
        displayWordCount = Math.Min(grid.GetLength(0), wordList.Count); // ������������� ���������� ������������ ����

        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = cols;

        gridCells = new GameObject[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject cell = Instantiate(cellPrefab, gridLayoutGroup.transform);
                gridCells[i, j] = cell;

                char letter = char.ToLower(grid[i, j]);

                if (letter == ' ' || char.IsWhiteSpace(letter))
                {
                    Debug.LogWarning($"������ ������ �� ������� [{i}, {j}]");
                    continue;
                }

                Sprite letterImage = letterImageMapping.GetSpriteForLetter(letter);
                if (letterImage == null)
                {
                    Debug.LogError($"����������� ��� ����� '{letter}' �� �������.");
                    continue;
                }

                LetterCell cellController = cell.GetComponent<LetterCell>();
                if (cellController != null)
                {
                    cellController.SetLetterImage(letterImage);
                }
            }
        }

        DisplayUsedWords();
    }
    private int currentDifficultyLevel;

    // ����� ��� ������������� ������ ��������� ��� ������ �������
    public void SetDifficultyLevel(int difficultyLevel)
    {
        currentDifficultyLevel = difficultyLevel;
        InitializeGrid(currentDifficultyLevel);
    }

    // ����� ��� �������� �����
    public void RestartGrid()
    {
        ClearGrid(); // ������� ������� �����
        InitializeGrid(currentDifficultyLevel); // �������������� ����� � ������� ������� ���������
        CanvasButtons canvasButtons = FindObjectOfType<CanvasButtons>();
        canvasButtons.OnCloseButtonClick();
    }
    public void NextnGrid()
    {
        ClearGrid(); // ������� ������� �����
        InitializeGrid(currentDifficultyLevel); // �������������� ����� � ������� ������� ���������

        CanvasButtons canvasButtons = FindObjectOfType<CanvasButtons>();
        canvasButtons.OnCloseButtonClick();

        int lvl = PlayerPrefs.GetInt("Levels", 0);
        PlayerPrefs.SetInt("Levels", lvl+1);
        PlayerPrefs.Save();
        //YandexGame.savesData.levels = lvl;
        //YandexGame.SaveProgress();
        //YandexGame.NewLeaderboardScores("Levels", lvl);
    }
    private void DisplayUsedWords()
    {
        if (wordsListText != null)
        {
            List<string> displayWords = wordList.GetRange(0, displayWordCount);
            wordsListText.text = "����� ��� ������:\n" + string.Join(", ", displayWords);
        }
        else
        {
            Debug.LogError("Text UI ������� ��� ������ ���� �� ����������.");
        }
    }
    private void ClearGrid()
    {
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private List<LetterCell> selectedCells = new List<LetterCell>();
    private string currentWordAttempt = "";

    public void RegisterCellClick(LetterCell cell)
    {
        selectedCells.Add(cell);
        currentWordAttempt += cell.letterImage.sprite.name;

        if (!CheckWord(currentWordAttempt))
        {
            foreach (var selectedCell in selectedCells)
            {
                selectedCell.gameObject.SetActive(true);
            }
            selectedCells.Clear();
            currentWordAttempt = "";
        }
        else if (wordList.Contains(currentWordAttempt))
        {
            wordList.Remove(currentWordAttempt);
            displayWordCount--; // ��������� ���������� ������������ ����
            if (displayWordCount == 0)
            {
                Debug.Log("�� ��������!");
                winPanel.SetActive(true);
            }
            DisplayUsedWords();
            foreach (var selectedCell in selectedCells)
            {
                selectedCell.gameObject.SetActive(false);
            }
            selectedCells.Clear();
            currentWordAttempt = "";
        }
    }

    private bool CheckWord(string currentWord)
    {
        foreach (string word in wordList)
        {
            if (word.StartsWith(currentWord))
            {
                return true;
            }
        }
        return false;
    }
}