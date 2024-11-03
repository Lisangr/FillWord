#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class GridTemplateEditor : EditorWindow
{
    private Color currentColor = Color.red;
    private GridTemplate currentTemplate;
    private int gridSize = 4;
    private bool isDrawingMode = false;
    private GridTemplatesData templatesData; // Текущий ScriptableObject

    [MenuItem("Tools/Grid Template Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridTemplateEditor>("Grid Template Editor");
    }

    private void OnEnable()
    {
        LoadOrCreateTemplateData();
        InitializeNewTemplate();
    }

    private void OnGUI()
    {
        GUILayout.Label("Редактор шаблона сетки", EditorStyles.boldLabel);

        gridSize = EditorGUILayout.IntField("Размер сетки:", gridSize);
        gridSize = Mathf.Clamp(gridSize, 4, 10);

        // Обновляем ScriptableObject и шаблон при изменении размера сетки
        if (currentTemplate == null || currentTemplate.gridSize != gridSize)
        {
            LoadOrCreateTemplateData();
            InitializeNewTemplate();
        }

        currentColor = EditorGUILayout.ColorField("Текущий цвет:", currentColor);

        if (GUILayout.Button(isDrawingMode ? "Завершить выбор" : "Выбрать ячейки"))
        {
            isDrawingMode = !isDrawingMode;
        }

        DisplayGrid();

        GUILayout.Space(10);

        if (GUILayout.Button("Сохранить шаблон"))
        {
            if (templatesData != null)
            {
                templatesData.templates.Add(currentTemplate);
                EditorUtility.SetDirty(templatesData);
                AssetDatabase.SaveAssets();
                Debug.Log($"Шаблон для {gridSize}x{gridSize} сохранен в {templatesData.name}.");
                InitializeNewTemplate(); // Создаем новый шаблон сразу после сохранения
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Новый шаблон"))
        {
            InitializeNewTemplate();
        }
    }

    private void DisplayGrid()
    {
        GUILayout.BeginVertical();
        for (int y = 0; y < gridSize; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Color originalColor = GUI.backgroundColor;

                int colorIndex = currentTemplate.positions.IndexOf(pos);
                if (colorIndex >= 0)
                {
                    GUI.backgroundColor = currentTemplate.colors[colorIndex];
                }

                if (GUILayout.Button("", GUILayout.Width(30), GUILayout.Height(30)) && isDrawingMode)
                {
                    if (colorIndex >= 0)
                    {
                        currentTemplate.positions.RemoveAt(colorIndex);
                        currentTemplate.colors.RemoveAt(colorIndex);
                    }
                    else
                    {
                        currentTemplate.positions.Add(pos);
                        currentTemplate.colors.Add(currentColor);
                    }
                }

                GUI.backgroundColor = originalColor;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void LoadOrCreateTemplateData()
    {
        string assetPath = $"Assets/GridTemplatesData_{gridSize}x{gridSize}.asset";
        templatesData = AssetDatabase.LoadAssetAtPath<GridTemplatesData>(assetPath);

        if (templatesData == null)
        {
            templatesData = ScriptableObject.CreateInstance<GridTemplatesData>();
            AssetDatabase.CreateAsset(templatesData, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"Создан новый ScriptableObject для сетки {gridSize}x{gridSize}.");
        }
    }

    private void InitializeNewTemplate()
    {
        int templateNumber = (templatesData != null) ? templatesData.templates.Count + 1 : 1;
        string templateName = $"Шаблон {templateNumber}";
        currentTemplate = new GridTemplate(templateName, gridSize);
        Debug.Log($"Создан новый шаблон: {templateName} для сетки {gridSize}x{gridSize}.");
    }
}
#endif