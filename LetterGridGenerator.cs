using System;
using System.Collections.Generic;
using UnityEngine;

public static class LetterGridGenerator
{
    private static readonly System.Random random = new System.Random();

    public static (char[,], List<string>) GenerateLetterGrid(int gridSize, GridTemplate template, List<string> wordList)
    {
        int wordCount = Math.Min(template.positions.Count, wordList.Count);
        List<string> words = GetRandomWords(wordList, wordCount);

        char[,] grid = new char[gridSize, gridSize];
        List<string> usedWords = new List<string>();

        int positionIndex = 0;

        for (int i = 0; i < words.Count; i++)
        {
            string word = words[i];
            usedWords.Add(word);

            for (int j = 0; j < word.Length; j++)
            {
                // ���������, �� ������� �� �� ������� �������
                if (positionIndex >= template.positions.Count)
                {
                    Debug.LogWarning($"������ ������������ ��� ���������� ���� ���� ����� '{word}'");
                    break;
                }

                Vector2Int position = template.positions[positionIndex];

                // �������� ������ �����
                if (position.x < 0 || position.y < 0 || position.x >= gridSize || position.y >= gridSize)
                {
                    Debug.LogError($"���������� {position} ������� �� ������� ����� ������� {gridSize}x{gridSize}.");
                    positionIndex++;
                    continue;
                }

                // ��������� ������ ����� ��������
                grid[position.x, position.y] = word[j];
                positionIndex++;
            }
        }

        // ��������� ���������� ������ ������ ���������� �������
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i, j] == '\0')
                {
                    grid[i, j] = (char)random.Next('�', '�' + 1); // ���������� ��������� ������������� ������
                }
            }
        }

        return (grid, usedWords);
    }


    private static List<string> GetRandomWords(List<string> wordList, int count)
    {
        List<string> randomWords = new List<string>();
        HashSet<int> usedIndices = new HashSet<int>();

        while (randomWords.Count < count)
        {
            int index = random.Next(wordList.Count);
            if (!usedIndices.Contains(index))
            {
                randomWords.Add(wordList[index]);
                usedIndices.Add(index);
            }
        }

        return randomWords;
    }
}
