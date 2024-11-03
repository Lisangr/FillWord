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
                // Проверяем, не выходит ли за пределы шаблона
                if (positionIndex >= template.positions.Count)
                {
                    Debug.LogWarning($"Шаблон недостаточен для размещения всех букв слова '{word}'");
                    break;
                }

                Vector2Int position = template.positions[positionIndex];

                // Проверка границ сетки
                if (position.x < 0 || position.y < 0 || position.x >= gridSize || position.y >= gridSize)
                {
                    Debug.LogError($"Координата {position} выходит за пределы сетки размера {gridSize}x{gridSize}.");
                    positionIndex++;
                    continue;
                }

                // Заполняем ячейку сетки символом
                grid[position.x, position.y] = word[j];
                positionIndex++;
            }
        }

        // Заполняем оставшиеся пустые ячейки случайными буквами
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i, j] == '\0')
                {
                    grid[i, j] = (char)random.Next('а', 'я' + 1); // Заполнение случайной кириллической буквой
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
