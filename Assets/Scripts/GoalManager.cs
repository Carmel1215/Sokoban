using System;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance { get; private set; }

    public enum MatchMode { AnyBoxFillsAnyGoal, MatchById }
    public MatchMode matchMode = MatchMode.AnyBoxFillsAnyGoal;
    public float step = 1f;
    public float epsilon = 0.0001f;

    readonly List<GoalMarker> goals = new();
    readonly List<BoxMarker>  boxes = new();

    public event Action OnCleared;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // --- 등록/해제: Marker에서 OnEnable/OnDisable로 호출 ---
    public void RegisterGoal(GoalMarker g)
    {
        if (g != null && !goals.Contains(g)) goals.Add(g);
    }
    public void UnregisterGoal(GoalMarker g)
    {
        if (g != null) goals.Remove(g);
    }
    public void RegisterBox(BoxMarker b)
    {
        if (b != null && !boxes.Contains(b)) boxes.Add(b);
    }
    public void UnregisterBox(BoxMarker b)
    {
        if (b != null) boxes.Remove(b);
    }

    // --- 외부 호출 ---
    public void TryCheckWin()
    {
        if (CheckWin()) OnCleared?.Invoke();
    }

    public int RemainingGoals()
    {
        int covered = CountCoveredGoals();
        int total   = goals.Count;
        return Mathf.Max(0, total - covered);
    }

    // --- 핵심 로직 ---
    public bool CheckWin()
    {
        CleanupNulls();
        if (goals.Count == 0) return false;

        return matchMode == MatchMode.MatchById
            ? AllGoalsCoveredById()
            : AllGoalsCoveredAny();
    }

    bool AllGoalsCoveredAny() => CountCoveredGoals() == goals.Count;

    int CountCoveredGoals()
    {
        int covered = 0;
        for (int i = 0; i < goals.Count; i++)
        {
            var g = goals[i];
            if (g == null) continue;

            Vector2 gSnap = SnapToGrid(g.transform.position, step);
            bool hasBox = false;

            for (int j = 0; j < boxes.Count; j++)
            {
                var b = boxes[j];
                if (b == null) continue;

                Vector2 bSnap = SnapToGrid(b.transform.position, step);
                if (Approximately(bSnap, gSnap, epsilon))
                {
                    hasBox = true;
                    break;
                }
            }
            if (hasBox) covered++;
        }
        return covered;
    }

    bool AllGoalsCoveredById()
    {
        for (int i = 0; i < goals.Count; i++)
        {
            var g = goals[i];
            if (g == null) return false;

            Vector2 gSnap = SnapToGrid(g.transform.position, step);
            bool matched = false;

            for (int j = 0; j < boxes.Count; j++)
            {
                var b = boxes[j];
                if (b == null || b.id != g.id) continue;

                Vector2 bSnap = SnapToGrid(b.transform.position, step);
                if (Approximately(bSnap, gSnap, epsilon))
                {
                    matched = true;
                    break;
                }
            }
            if (!matched) return false;
        }
        return true;
    }

    void CleanupNulls()
    {
        for (int i = goals.Count - 1; i >= 0; i--) if (goals[i] == null) goals.RemoveAt(i);
        for (int i = boxes.Count - 1; i >= 0; i--) if (boxes[i] == null) boxes.RemoveAt(i);
    }

    // --- Utils ---
    public static Vector2 SnapToGrid(Vector2 pos, float s)
    {
        float x = Mathf.Round(pos.x / s) * s;
        float y = Mathf.Round(pos.y / s) * s;
        return new Vector2(x, y);
    }
    static bool Approximately(Vector2 a, Vector2 b, float eps)
        => Mathf.Abs(a.x - b.x) <= eps && Mathf.Abs(a.y - b.y) <= eps;
}