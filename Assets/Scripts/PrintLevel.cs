using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PrintLevel : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;

    public void LoadLevelName(string levelName)
    {
        levelText.text = levelName[^2..];
    }
}
