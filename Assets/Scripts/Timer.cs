using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] int seconds = 10;

    [Header("UI")]
    [SerializeField] TMP_Text timerText;

    [Header("Scene")]
    [SerializeField] string finishSceneName = "FinishScene";

    Coroutine routine;

    void Start()
    {
        UpdateText(seconds);   // 시작하자마자 표시
    }

    void OnEnable()
    {
        routine = StartCoroutine(Countdown());
    }

    void OnDisable()
    {
        if (routine != null) StopCoroutine(routine);
    }

    IEnumerator Countdown()
    {
        int timeLeft = seconds;

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            GameManager.Instance.time++; // 걸린 시간 세기
            UpdateText(timeLeft);
        }

        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene(finishSceneName);
    }

    void UpdateText(int time)
    {
        int min = time / 60;
        int sec = time % 60;
        timerText.text = $"{min:00}:{sec:00}";
    }
}