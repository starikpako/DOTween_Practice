using DG.Tweening;
using UnityEngine;

public class ComplexTransformer : MonoBehaviour
{
    private const int InfiniteLoops = -1;

    private void Start()
    {
        AnimationConfig.ComplexTransformerConfig config = AnimationConfig.Get().complex_transformer;
        Ease ease = AnimationConfig.ParseEase(config.ease);

        Sequence transformSequence = DOTween.Sequence();
        transformSequence.Join(transform.DOMove(config.target_position.ToVector3(), config.duration));
        transformSequence.Join(transform.DORotate(config.rotation_angle.ToVector3(), config.duration));
        transformSequence.Join(transform.DOScale(config.target_scale.ToVector3(), config.duration));
        transformSequence.SetLoops(InfiniteLoops, LoopType.Yoyo).SetEase(ease);
    }
}
