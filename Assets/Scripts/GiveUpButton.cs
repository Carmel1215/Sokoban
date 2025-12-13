using UnityEngine;
using UnityEngine.SceneManagement;

public class GiveUpButton : MonoBehaviour
{
    public void OnClicked()
    {
        SceneManager.LoadScene("FinishScreen");
    }
}
