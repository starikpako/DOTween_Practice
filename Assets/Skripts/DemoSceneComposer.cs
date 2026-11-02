using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Страховка: если на сцене не хватает объектов задания — создаёт их при Play.
/// </summary>
public class DemoSceneComposer : MonoBehaviour
{
    private const string SphereName = "Sphere_Move";
    private const string CubeRotateName = "Cube_Rotate";
    private const string CapsuleName = "Capsule_Scale";
    private const string ComplexCubeName = "Cube_Complex";
    private const string ColorObjectName = "Cube_Color";
    private const string CanvasName = "TextCanvas";
    private const string TextObjectName = "AnimatedText";

    // Проверяет объекты задания и дополняет недостающие.
    private void Awake()
    {
        AnimationConfig.SceneLayoutConfig layout = AnimationConfig.Get().scene_layout;

        EnsurePrimitive(SphereName, PrimitiveType.Sphere, layout.sphere_position.ToVector3(), typeof(SphereMovement));
        EnsurePrimitive(CubeRotateName, PrimitiveType.Cube, layout.cube_position.ToVector3(), typeof(CubeRotation));
        EnsurePrimitive(CapsuleName, PrimitiveType.Capsule, layout.capsule_position.ToVector3(), typeof(CapsuleScaling));
        EnsurePrimitive(ComplexCubeName, PrimitiveType.Cube, layout.complex_cube_position.ToVector3(), typeof(ComplexTransformer));
        EnsurePrimitive(ColorObjectName, PrimitiveType.Cube, layout.color_object_position.ToVector3(), typeof(ColorChanger));
        EnsureTextObject();
    }

    // Создаёт примитив с компонентом только если объекта ещё нет.
    private static void EnsurePrimitive(string objectName, PrimitiveType primitiveType, Vector3 position, System.Type componentType)
    {
        GameObject target = GameObject.Find(objectName);
        if (target == null)
        {
            target = GameObject.CreatePrimitive(primitiveType);
            target.name = objectName;
            target.transform.position = position;
        }

        if (target.GetComponent(componentType) == null)
        {
            target.AddComponent(componentType);
        }
    }

    // Создаёт Canvas и UI.Text с TextManipulator, если их нет.
    private static void EnsureTextObject()
    {
        GameObject canvasObject = GameObject.Find(CanvasName);
        if (canvasObject == null)
        {
            canvasObject = new GameObject(CanvasName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
        }

        Transform textTransform = canvasObject.transform.Find(TextObjectName);
        GameObject textObject = textTransform != null
            ? textTransform.gameObject
            : new GameObject(TextObjectName, typeof(RectTransform), typeof(Text));

        if (textTransform == null)
        {
            textObject.transform.SetParent(canvasObject.transform, false);
        }

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = new Vector2(0f, -40f);
        rectTransform.sizeDelta = new Vector2(900f, 120f);

        Text text = textObject.GetComponent<Text>();
        if (text.font == null)
        {
            text.font = ResolveUiFont();
        }

        text.fontSize = 48;
        text.alignment = TextAnchor.UpperCenter;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        if (textObject.GetComponent<TextManipulator>() == null)
        {
            textObject.AddComponent<TextManipulator>();
        }
    }

    // Подбирает встроенный UI-шрифт.
    private static Font ResolveUiFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return font != null ? font : Resources.GetBuiltinResource<Font>("Arial.ttf");
    }
}
