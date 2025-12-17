using UnityEngine;

public class SceneSFX : MonoBehaviour
{
    [SerializeField] AudioClip sfx;
    void Start()
    {
        SoundManager.Instance.PlaySFX(sfx);
    }
}
