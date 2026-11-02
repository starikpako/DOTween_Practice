using DG.Tweening;
using UnityEngine;


public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private const int InfiniteLoops = -1;

    private void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
    }

    private void Start()
    {
        if (_renderer == null)
        {
            Debug.LogError($"{nameof(ColorChanger)}: на объекте {name} нет Renderer.", this);
            return;
        }

        AnimationConfig.ColorChangerConfig config = AnimationConfig.Get().color_changer;
        Material materialInstance = _renderer.material;

        materialInstance.DOColor(config.target_color.ToColor(), config.color_property, config.duration)
            .SetLoops(InfiniteLoops, LoopType.Yoyo)
            .SetEase(AnimationConfig.ParseEase(config.ease));
    }
}
