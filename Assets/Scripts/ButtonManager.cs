using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Tutorial");
    }

    public void OnRetryButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        SceneBridge bridge = FindFirstObjectByType<SceneBridge>();

        if (bridge != null)
            bridge.ReloadLevel();
        else
            Debug.LogError("No bridge found");
    }

    public void OnGiveUpButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("FinishScreen");
    }

    public void OnRestartButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        GameManager.Instance.Reset();
        SceneManager.LoadScene("TitleScreen");
    }
}
