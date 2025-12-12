using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerGridMover2D : MonoBehaviour
{
    [Header("Move")]
    public float step = 1f;            // 한 번 이동 거리(칸)
    public float moveDuration = 0.12f; // 이동 시간(초)

    [Header("Blocking (Walls 레이어 등)")]
    public LayerMask blockMask;        // 벽/장애물 타일맵의 레이어 마스크
    public float skin = 0.02f;         // 모서리 끼임 방지 여유

    [Header("Box")]
    public LayerMask boxMask;          // 박스 레이어 마스크

    Rigidbody2D rb;
    Collider2D selfCol;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Animator 파라미터 Hash(Idle BT만 사용)
    static readonly int HashMoveX = Animator.StringToHash("MoveX");
    static readonly int HashMoveY = Animator.StringToHash("MoveY");

    Vector2 lastDir = Vector2.down;
    bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        selfCol = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.None;
        rb.freezeRotation = true;

        // 초기 바라봄 전달(IdleBT가 바로 반영하도록)
        if (animator != null)
        {
            animator.SetFloat(HashMoveX, lastDir.x);
            animator.SetFloat(HashMoveY, lastDir.y);
        }
    }

    void Update()
    {
        if (isMoving) return;

        Vector2 dir = ReadDiscreteInput();
        if (dir == Vector2.zero) return;

        Vector2 start = rb.position;
        Vector2 target = start + dir * step;

        // 1) 벽/장애물
        if (IsBlocked(start, dir, step)) return;

        // 2) 다음 칸의 박스 확인
        Vector2 mySize = (Vector2)selfCol.bounds.size - Vector2.one * skin;
        Collider2D boxColAtNext = Physics2D.OverlapBox(target, mySize, 0f, boxMask);

        // 입력 확정 → 마지막 바라봄 및 시각 동기화
        lastDir = dir;
        if (animator != null)
        {
            animator.SetFloat(HashMoveX, lastDir.x);
            animator.SetFloat(HashMoveY, lastDir.y);
        }
        if (dir.x != 0f) spriteRenderer.flipX = dir.x < 0f;

        if (boxColAtNext != null)
        {
            Rigidbody2D boxRb = boxColAtNext.attachedRigidbody;
            if (boxRb == null) return;

            Vector2 boxStart = boxRb.position;
            Vector2 boxEnd   = boxStart + dir * step;

            // 3) 박스 다음 칸도 비어 있어야 밀 수 있음
            Vector2 boxSize = (Vector2)boxColAtNext.bounds.size - Vector2.one * skin;
            bool boxBlocked = Physics2D.OverlapBox(boxEnd, boxSize, 0f, blockMask | boxMask) != null;
            if (boxBlocked) return;

            StartCoroutine(MovePlayerAndBox(start, target, boxRb, boxStart, boxEnd));
            return;
        }

        // 4) 일반 이동
        StartCoroutine(MoveStep(start, target));
    }

    Vector2 ReadDiscreteInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))    return Vector2.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))  return Vector2.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))  return Vector2.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return Vector2.right;
        return Vector2.zero;
    }

    bool IsBlocked(Vector2 origin, Vector2 dir, float distance)
    {
        Vector2 size = (Vector2)selfCol.bounds.size - Vector2.one * skin;
        Vector2 targetCenter = origin + dir * distance;
        return Physics2D.OverlapBox(targetCenter, size, 0f, blockMask) != null;
    }

    IEnumerator MoveStep(Vector2 start, Vector2 target)
    {
        isMoving = true;
        rb.linearVelocity = Vector2.zero;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float eased = Mathf.SmoothStep(0f, 1f, t);
            rb.MovePosition(Vector2.Lerp(start, target, eased));
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(SnapToGrid(target, step));
        rb.linearVelocity = Vector2.zero;
        isMoving = false;
        OnStepFinished();
    }

    IEnumerator MovePlayerAndBox(Vector2 pStart, Vector2 pTarget, Rigidbody2D boxRb, Vector2 bStart, Vector2 bTarget)
    {
        isMoving = true;
        rb.linearVelocity = Vector2.zero;
        boxRb.linearVelocity = Vector2.zero;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float eased = Mathf.SmoothStep(0f, 1f, t);

            rb.MovePosition(Vector2.Lerp(pStart, pTarget, eased));
            boxRb.MovePosition(Vector2.Lerp(bStart, bTarget, eased));

            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(SnapToGrid(pTarget, step));
        boxRb.MovePosition(SnapToGrid(bTarget, step));

        rb.linearVelocity = Vector2.zero;
        boxRb.linearVelocity = Vector2.zero;
        isMoving = false;
        OnStepFinished();
    }

    void OnStepFinished()
    {
        // IdleBT는 MoveX/MoveY만으로 방향 유지
        if (animator != null)
        {
            animator.SetFloat(HashMoveX, lastDir.x);
            animator.SetFloat(HashMoveY, lastDir.y);
        }
        GoalManager.Instance?.TryCheckWin();
    }

    static Vector2 SnapToGrid(Vector2 pos, float step)
    {
        float x = Mathf.Round(pos.x / step) * step;
        float y = Mathf.Round(pos.y / step) * step;
        return new Vector2(x, y);
    }
}