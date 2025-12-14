using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void OnClicked()
    {
        GameManager.Instance.Reset();
        SceneManager.LoadScene("Scenes/TitleScreen");
    }
}
