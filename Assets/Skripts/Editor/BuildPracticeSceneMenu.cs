using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Меню редактора: собирает 6 объектов задания прямо в открытой сцене.
/// </summary>
public static class BuildPracticeSceneMenu
{
    private const string MenuPath = "DOTween Practice/Build Scene Objects";

    // Собирает объекты задания в активной сцене и сохраняет её.
    [MenuItem(MenuPath)]
    private static void BuildSceneObjects()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || string.IsNullOrEmpty(scene.path))
        {
            EditorUtility.DisplayDialog(
                "DOTween Practice",
                "Откройте сохранённую сцену Assets/Scenes/SampleScene.unity и повторите.",
                "OK");
            return;
        }

        AnimationConfig.SceneLayoutConfig layout = AnimationConfig.Get().scene_layout;

        EnsurePrimitive("Sphere_Move", PrimitiveType.Sphere, layout.sphere_position.ToVector3(), typeof(SphereMovement));
        EnsurePrimitive("Cube_Rotate", PrimitiveType.Cube, layout.cube_position.ToVector3(), typeof(CubeRotation));
        EnsurePrimitive("Capsule_Scale", PrimitiveType.Capsule, layout.capsule_position.ToVector3(), typeof(CapsuleScaling));
        EnsurePrimitive("Cube_Complex", PrimitiveType.Cube, layout.complex_cube_position.ToVector3(), typeof(ComplexTransformer));
        EnsurePrimitive("Cube_Color", PrimitiveType.Cube, layout.color_object_position.ToVector3(), typeof(ColorChanger));
        EnsureTextUi();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "DOTween Practice",
            "Объекты задания созданы и сцена сохранена.\nПроверьте Hierarchy.",
            "OK");
    }

    // Создаёт или обновляет примитив со скриптом анимации.
    private static void EnsurePrimitive(string objectName, PrimitiveType primitiveType, Vector3 position, System.Type componentType)
    {
        GameObject target = GameObject.Find(objectName);
        if (target == null)
        {
            target = GameObject.CreatePrimitive(primitiveType);
            target.name = objectName;
            Undo.RegisterCreatedObjectUndo(target, "Create " + objectName);
        }

        Undo.RecordObject(target.transform, "Move " + objectName);
        target.transform.position = position;

        if (target.GetComponent(componentType) == null)
        {
            Undo.AddComponent(target, componentType);
        }
    }

    // Создаёт Canvas и UI.Text со скриптом TextManipulator.
    private static void EnsureTextUi()
    {
        GameObject canvasObject = GameObject.Find("TextCanvas");
        if (canvasObject == null)
        {
            canvasObject = new GameObject("TextCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Undo.RegisterCreatedObjectUndo(canvasObject, "Create TextCanvas");
        }

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        Transform textTransform = canvasObject.transform.Find("AnimatedText");
        GameObject textObject;
        if (textTransform == null)
        {
            textObject = new GameObject("AnimatedText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            Undo.RegisterCreatedObjectUndo(textObject, "Create AnimatedText");
            textObject.transform.SetParent(canvasObject.transform, false);
        }
        else
        {
            textObject = textTransform.gameObject;
        }

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = new Vector2(0f, -40f);
        rectTransform.sizeDelta = new Vector2(900f, 120f);

        Text text = textObject.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
        {
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        text.fontSize = 48;
        text.alignment = TextAnchor.UpperCenter;
        text.color = Color.white;
        text.text = AnimationConfig.Get().text_manipulator.base_text;

        if (textObject.GetComponent<TextManipulator>() == null)
        {
            Undo.AddComponent<TextManipulator>(textObject);
        }
    }
}
