#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class GridTemplateEditor : EditorWindow
{
    private Color currentColor = Color.red;
    private GridTemplate currentTemplate;
    private int gridSize = 4;
    private bool isDrawingMode = false;
    private GridTemplatesData templatesData; // ������� ScriptableObject

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
        GUILayout.Label("�������� ������� �����", EditorStyles.boldLabel);

        gridSize = EditorGUILayout.IntField("������ �����:", gridSize);
        gridSize = Mathf.Clamp(gridSize, 4, 10);

        // ��������� ScriptableObject � ������ ��� ��������� ������� �����
        if (currentTemplate == null || currentTemplate.gridSize != gridSize)
        {
            LoadOrCreateTemplateData();
            InitializeNewTemplate();
        }

        currentColor = EditorGUILayout.ColorField("������� ����:", currentColor);

        if (GUILayout.Button(isDrawingMode ? "��������� �����" : "������� ������"))
        {
            isDrawingMode = !isDrawingMode;
        }

        DisplayGrid();

        GUILayout.Space(10);

        if (GUILayout.Button("��������� ������"))
        {
            if (templatesData != null)
            {
                templatesData.templates.Add(currentTemplate);
                EditorUtility.SetDirty(templatesData);
                AssetDatabase.SaveAssets();
                Debug.Log($"������ ��� {gridSize}x{gridSize} �������� � {templatesData.name}.");
                InitializeNewTemplate(); // ������� ����� ������ ����� ����� ����������
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("����� ������"))
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
            Debug.Log($"������ ����� ScriptableObject ��� ����� {gridSize}x{gridSize}.");
        }
    }

    private void InitializeNewTemplate()
    {
        int templateNumber = (templatesData != null) ? templatesData.templates.Count + 1 : 1;
        string templateName = $"������ {templateNumber}";
        currentTemplate = new GridTemplate(templateName, gridSize);
        Debug.Log($"������ ����� ������: {templateName} ��� ����� {gridSize}x{gridSize}.");
    }
}
#endif