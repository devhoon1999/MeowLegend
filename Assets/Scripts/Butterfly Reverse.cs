using UnityEngine;

public class ButterflyFlipWithMargin : MonoBehaviour
{
    public RectTransform canvasRectTransform;

    private RectTransform rectTransform;
    private bool isOutsideLastFrame = false;
    private bool flipped = false;

    [Tooltip("캔버스 경계에서 얼마나 안쪽으로 여유를 둘지(px)")]
    public float margin = 20f;

    [Tooltip("반전 후 다시 반전 가능하도록 안쪽으로 충분히 들어와야 하는 거리(px)")]
    public float insideBuffer = 10f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (canvasRectTransform == null)
        {
            Debug.LogError("Canvas RectTransform is not assigned!");
            return;
        }

        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        Vector3[] objCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objCorners);

        float leftBoundary = canvasCorners[0].x + margin;
        float rightBoundary = canvasCorners[2].x - margin;

        isOutsideLastFrame =
            objCorners[0].x < leftBoundary ||
            objCorners[2].x > rightBoundary;
    }

    void Update()
    {
        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        Vector3[] objCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objCorners);

        float leftBoundary = canvasCorners[0].x + margin;
        float rightBoundary = canvasCorners[2].x - margin;

        bool isOutside =
            objCorners[0].x < leftBoundary ||
            objCorners[2].x > rightBoundary;

        if (isOutside && !isOutsideLastFrame && !flipped)
        {
            FlipImage();
            flipped = true;
        }

        bool isWellInside =
            objCorners[0].x > leftBoundary + insideBuffer &&
            objCorners[2].x < rightBoundary - insideBuffer;

        if (isWellInside)
        {
            flipped = false;
        }

        isOutsideLastFrame = isOutside;
    }

    void FlipImage()
    {
        Vector3 scale = rectTransform.localScale;
        scale.x *= -1;
        rectTransform.localScale = scale;
    }
}