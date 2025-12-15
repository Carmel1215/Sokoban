using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Core");
    }

    public void OnRetryButtonClicked()
    {
        SceneBridge bridge = FindFirstObjectByType<SceneBridge>();

        if (bridge != null)
            bridge.ReloadLevel();
        else
            Debug.LogError("No bridge found");
    }

    public void OnGiveUpButtonClicked()
    {
        SceneManager.LoadScene("FinishScreen");
    }

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.Reset();
        SceneManager.LoadScene("TitleScreen");
    }
}
