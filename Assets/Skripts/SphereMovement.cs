using DG.Tweening;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    private const int InfiniteLoops = -1;

    private void Start()
    {
        AnimationConfig.SphereMovementConfig config = AnimationConfig.Get().sphere_movement;

        transform.DOMove(config.target_position.ToVector3(), config.duration)
            .SetLoops(InfiniteLoops, LoopType.Yoyo)
            .SetEase(AnimationConfig.ParseEase(config.ease));
    }
}
