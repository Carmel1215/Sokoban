using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBridge : MonoBehaviour
{
    [SerializeField] string firstLevel = "Level_00";  // 처음 시작 레벨
    [SerializeField] string finishScene = "FinishScreen";
    [SerializeField] PrintLevel printLevel;
    public static SceneBridge Instance { get; private set; }

    string currentLevel; // 지금 플레이 중인 레벨 이름

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // 처음 시작할 때는 firstLevel로 세팅
        currentLevel = firstLevel;
    }

    void Start()
    {
        printLevel.LoadLevelName(currentLevel);
        StartCoroutine(LoadAdditive(currentLevel));
    }

    // 지금 레벨 다시하기
    public void ReloadLevel()
    {
        StartCoroutine(Switch(currentLevel));
    }

    // 다음 레벨로 이동
    public void LoadLevel(string name)
    {
        // 다음 씬이 있을 때
        if (Application.CanStreamedLevelBeLoaded(name))
        {
            GameManager.Instance.AddScore(1000); // TODO: 점수 세는 시스템 정비 필요
            currentLevel = name;
            StartCoroutine(Switch(name));
            printLevel.LoadLevelName(currentLevel);
        }
        // 다음 씬이 없을 때
        else
        {
            GameManager.Instance.AddScore(1000);
            currentLevel = finishScene;
            SoundManager.Instance.StopBGM();
            SceneManager.LoadScene(currentLevel);
        }
    }

    IEnumerator LoadAdditive(string name)
    {
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        Time.timeScale = 1f;
        ApplyLevelCamera();
    }

    IEnumerator Switch(string next)
    {
        var active = SceneManager.GetActiveScene();
        if (active.name != "Core")
            yield return SceneManager.UnloadSceneAsync(active);

        yield return LoadAdditive(next);
    }

    void ApplyLevelCamera()
    {
        var cam = Camera.main;
        if (cam == null) return;

        var settings = FindFirstObjectByType<LevelCameraSettings>();
        if (settings == null) return;

        cam.transform.position = settings.position;
        if (cam.orthographic) cam.orthographicSize = settings.orthographicSize;
    }
}