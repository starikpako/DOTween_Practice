using System;
using UnityEngine;

/// <summary>
/// Загружает параметры анимаций из Resources/config.json.
/// </summary>
public static class AnimationConfig
{
    private const string ConfigResourcePath = "config";
    private static RootConfig _cachedConfig;

    // Сбрасывает кэш при входе в Play Mode (важно без Domain Reload).
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCache()
    {
        _cachedConfig = null;
    }

    // Возвращает загруженный конфиг (с кэшированием).
    public static RootConfig Get()
    {
        if (_cachedConfig != null)
        {
            return _cachedConfig;
        }

        TextAsset configAsset = Resources.Load<TextAsset>(ConfigResourcePath);
        if (configAsset == null)
        {
            throw new InvalidOperationException(
                $"Не найден файл настроек Resources/{ConfigResourcePath}.json");
        }

        _cachedConfig = JsonUtility.FromJson<RootConfig>(configAsset.text);
        if (_cachedConfig == null)
        {
            throw new InvalidOperationException("Не удалось разобрать config.json");
        }

        return _cachedConfig;
    }

    // Преобразует имя Ease из конфига в enum DOTween.
    public static DG.Tweening.Ease ParseEase(string easeName)
    {
        if (Enum.TryParse(easeName, ignoreCase: true, out DG.Tweening.Ease ease))
        {
            return ease;
        }

        return DG.Tweening.Ease.Linear;
    }

    // Преобразует имя ScrambleMode из конфига.
    public static DG.Tweening.ScrambleMode ParseScrambleMode(string modeName)
    {
        if (Enum.TryParse(modeName, ignoreCase: true, out DG.Tweening.ScrambleMode mode))
        {
            return mode;
        }

        return DG.Tweening.ScrambleMode.All;
    }

    [Serializable]
    public class RootConfig
    {
        public SphereMovementConfig sphere_movement;
        public CubeRotationConfig cube_rotation;
        public CapsuleScalingConfig capsule_scaling;
        public ComplexTransformerConfig complex_transformer;
        public ColorChangerConfig color_changer;
        public TextManipulatorConfig text_manipulator;
        public SceneLayoutConfig scene_layout;
    }

    [Serializable]
    public class Vec3
    {
        public float x;
        public float y;
        public float z;

        // Конвертирует DTO в Vector3.
        public Vector3 ToVector3() => new Vector3(x, y, z);
    }

    [Serializable]
    public class ColorRgba
    {
        public float r;
        public float g;
        public float b;
        public float a;

        // Конвертирует DTO в Color.
        public Color ToColor() => new Color(r, g, b, a);
    }

    [Serializable]
    public class SphereMovementConfig
    {
        public Vec3 target_position;
        public float duration;
        public string ease;
    }

    [Serializable]
    public class CubeRotationConfig
    {
        public Vec3 rotation_angle;
        public float duration;
        public string ease;
    }

    [Serializable]
    public class CapsuleScalingConfig
    {
        public Vec3 target_scale;
        public float duration;
        public string ease;
    }

    [Serializable]
    public class ComplexTransformerConfig
    {
        public Vec3 target_position;
        public Vec3 rotation_angle;
        public Vec3 target_scale;
        public float duration;
        public string ease;
    }

    [Serializable]
    public class ColorChangerConfig
    {
        public ColorRgba target_color;
        public float duration;
        public string ease;
        public string color_property;
    }

    [Serializable]
    public class TextManipulatorConfig
    {
        public float step_duration;
        public string base_text;
        public string replaced_text;
        public string appended_suffix;
        public string scrambled_text;
        public string scramble_mode;
    }

    [Serializable]
    public class SceneLayoutConfig
    {
        public Vec3 sphere_position;
        public Vec3 cube_position;
        public Vec3 capsule_position;
        public Vec3 complex_cube_position;
        public Vec3 color_object_position;
    }
}
