using UnityEngine;
using TMPro;
using DG.Tweening;

public class PrintScore : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text stageText;
    [SerializeField] TMP_Text scoreText;

    [Header("Anim")]
    [SerializeField] float moveOffset = 20f;

    void Start()
    {
        PlaySequence();
    }

    void PlaySequence()
    {
        // 원래 위치 저장
        Vector3 timePos  = timeText.transform.localPosition;
        Vector3 stagePos = stageText.transform.localPosition;
        Vector3 scorePos = scoreText.transform.localPosition;

        // 초기화
        timeText.alpha  = 0;
        stageText.alpha = 0;
        scoreText.alpha = 0;

        timeText.transform.localPosition  = timePos  - Vector3.up * moveOffset;
        stageText.transform.localPosition = stagePos - Vector3.up * moveOffset;
        scoreText.transform.localPosition = scorePos - Vector3.up * moveOffset;

        // 텍스트 세팅
        timeText.text  = $"{GameManager.Instance.time}초";
        stageText.text = $"{GameManager.Instance.stageCount}개";
        scoreText.text = "0점";

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(0.4f);

        // 시간
        seq.Append(timeText.DOFade(1f, 0.6f));
        seq.Join(
            timeText.transform.DOLocalMoveY(timePos.y, 0.6f)
                .SetEase(Ease.OutCubic)
        );
        seq.AppendInterval(0.4f);

        // 스테이지
        seq.Append(stageText.DOFade(1f, 0.6f));
        seq.Join(
            stageText.transform.DOLocalMoveY(stagePos.y, 0.6f)
                .SetEase(Ease.OutCubic)
        );
        seq.AppendInterval(0.4f);

        // 점수
        seq.Append(scoreText.DOFade(1f, 0.4f));
        seq.Join(
            scoreText.transform.DOLocalMoveY(scorePos.y, 0.4f)
                .SetEase(Ease.OutCubic)
        );
        seq.Append(
            DOTween.To(
                () => 0,
                x => scoreText.text = $"{x}점",
                GameManager.Instance.score + 4000,
                3.0f
            ).SetEase(Ease.OutExpo)
        );
    }
}