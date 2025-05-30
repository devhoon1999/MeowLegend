using UnityEngine;

public class ButterflyFlipWithMargin : MonoBehaviour
{
    public RectTransform canvasRectTransform;

    private RectTransform rectTransform;
    private bool isOutsideLastFrame = false;
    private bool flipped = false;

    [Tooltip("ĵ���� ��迡�� �󸶳� �������� ������ ����(px)")]
    public float margin = 20f;

    [Tooltip("���� �� �ٽ� ���� �����ϵ��� �������� ����� ���;� �ϴ� �Ÿ�(px)")]
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