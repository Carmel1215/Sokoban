using UnityEngine;

public class RetryButton : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneBridge bridge = FindFirstObjectByType<SceneBridge>();

        if (bridge != null)
            bridge.ReloadLevel();
        else
            Debug.LogError("No bridge found");
    }
}
