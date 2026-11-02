using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Последовательно меняет текст: замена, добавление и scramble-эффект.
/// </summary>
public class TextManipulator : MonoBehaviour
{
    [SerializeField] private Text _text;

    private const int InfiniteLoops = -1;
    private Sequence _sequence;

    // Подставляет Text, если ссылка не задана в инспекторе.
    private void Awake()
    {
        if (_text == null)
        {
            _text = GetComponent<Text>();
        }
    }

    // Запускает цикличную последовательность текстовых эффектов DOTween.
    private void Start()
    {
        if (_text == null)
        {
            Debug.LogError($"{nameof(TextManipulator)}: на объекте {name} нет UI.Text.", this);
            return;
        }

        AnimationConfig.TextManipulatorConfig config = AnimationConfig.Get().text_manipulator;
        ScrambleMode scrambleMode = AnimationConfig.ParseScrambleMode(config.scramble_mode);
        string appendedText = config.replaced_text + config.appended_suffix;

        _text.DOKill();
        _text.text = config.base_text;

        _sequence = DOTween.Sequence().SetLink(gameObject).SetUpdate(true);
        _sequence.Append(_text.DOText(config.replaced_text, config.step_duration));
        _sequence.Append(_text.DOText(appendedText, config.step_duration));
        _sequence.Append(
            _text.DOText(config.scrambled_text, config.step_duration, richTextEnabled: true, scrambleMode));
        _sequence.AppendInterval(config.step_duration);
        _sequence.Append(_text.DOText(config.base_text, config.step_duration));
        _sequence.SetLoops(InfiniteLoops, LoopType.Restart);
    }

    // Останавливает твины текста при уничтожении объекта.
    private void OnDestroy()
    {
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }

        if (_text != null)
        {
            _text.DOKill();
        }
    }
}
