using UnityEngine;
using DG.Tweening;

public class FloatingSprite : MonoBehaviour
{
    [SerializeField] float distance = 0.1f;
    [SerializeField] float duration = 1.5f;

    void Start()
    {
        transform.DOMoveY(transform.position.y + distance, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}