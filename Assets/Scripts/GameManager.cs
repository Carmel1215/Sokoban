using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score;
    public int stageCount;
    public int time;

    public void AddScore(int stageScore)
    {
        score += stageScore;
        stageCount++;
    }

    public void SetTime(int _time)
    {
        time = _time;
    }

    public void Reset()
    {
        score = 0;
        stageCount = 0;
        time = 0;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
