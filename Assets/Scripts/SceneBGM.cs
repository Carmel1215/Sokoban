using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    [SerializeField] AudioClip bgm;
    void Start()
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.PlayBGM(bgm);
    }
}
