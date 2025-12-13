using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnClicked()
    {
        SceneManager.LoadScene("Core");
    }
}
