using UnityEngine;

public class SceneSFX : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.clear);
    }
}
