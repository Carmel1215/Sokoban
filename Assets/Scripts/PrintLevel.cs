using UnityEngine;
using TMPro;

public class PrintLevel : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;

    public void LoadLevelName(string levelName)
    {
        string num = levelName[^2..];
        if (num == "00")
        {
            levelText.text = "Tutorial";
        }
        else
        {
            levelText.text = num;
        }
    }
}
