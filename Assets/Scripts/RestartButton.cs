using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void OnClicked()
    {
        SceneManager.LoadScene("Scenes/TitleScreen");
    }
}
