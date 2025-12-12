using UnityEngine;

public class BoxMarker : MonoBehaviour
{
    public int id = 0; // 특정 조건을 지닌 목표지점과 대응하기 위함

    void OnEnable()  { GoalManager.Instance?.RegisterBox(this); }
    void OnDisable() { GoalManager.Instance?.UnregisterBox(this); }
}