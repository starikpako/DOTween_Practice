using DG.Tweening;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    private const int InfiniteLoops = -1;

    private void Start()
    {
        AnimationConfig.CubeRotationConfig config = AnimationConfig.Get().cube_rotation;

        transform.DORotate(config.rotation_angle.ToVector3(), config.duration, RotateMode.FastBeyond360)
            .SetLoops(InfiniteLoops, LoopType.Incremental)
            .SetEase(AnimationConfig.ParseEase(config.ease));
    }
}
