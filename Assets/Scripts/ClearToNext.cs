using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class ClearToNext : MonoBehaviour
{
    public string prefix = "Level_";
    public int digits = 2;

    Coroutine bindCo;
    bool subscribed;

    void OnEnable()
    {
        bindCo = StartCoroutine(BindWhenReady());
    }

    void OnDisable()
    {
        if (bindCo != null) { StopCoroutine(bindCo); bindCo = null; }
        if (subscribed && GoalManager.Instance != null)
        {
            GoalManager.Instance.OnCleared -= HandleCleared;
            subscribed = false;
        }
    }

    IEnumerator BindWhenReady()
    {
        // GoalManager가 생성될 때까지 대기
        while (GoalManager.Instance == null) yield return null;

        GoalManager.Instance.OnCleared += HandleCleared;
        subscribed = true;
    }

    void HandleCleared()
    {
        var cur = SceneManager.GetActiveScene().name;
        var m = Regex.Match(cur, @"^(.*?)(\d+)$");
        if (!m.Success) { Debug.LogWarning("이름 규칙이 맞지 않아요."); return; }

        int n = int.Parse(m.Groups[2].Value) + 1;
        string next = prefix + n.ToString(new string('0', digits));
        FindFirstObjectByType<SceneBridge>()?.LoadLevel(next);
    }
}