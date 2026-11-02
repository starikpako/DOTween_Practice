using DG.Tweening;
using UnityEngine;

public class CapsuleScaling : MonoBehaviour
{
    private const int InfiniteLoops = -1;

    private void Start()
    {
        AnimationConfig.CapsuleScalingConfig config = AnimationConfig.Get().capsule_scaling;

        transform.DOScale(config.target_scale.ToVector3(), config.duration)
            .SetLoops(InfiniteLoops, LoopType.Yoyo)
            .SetEase(AnimationConfig.ParseEase(config.ease));
    }
}
