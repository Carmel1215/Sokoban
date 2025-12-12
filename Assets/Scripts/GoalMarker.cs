using UnityEngine;

public class GoalMarker : MonoBehaviour
{
    public int id = 0; // 특정 조건을 지닌 박스와 대응하기 위함

    void OnEnable()  { GoalManager.Instance?.RegisterGoal(this); }
    void OnDisable() { GoalManager.Instance?.UnregisterGoal(this); }
}