using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject arrowHead;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int curveSegments = 32;      // 曲线分段数(点数)
    [SerializeField] private float bowAmount = 0.25f;      // 弯曲程度(相对连线长度的比例)
    private Vector3 startPosition;

    private void Update()
    {
        Vector3 endPosition = MouseUtil.GetMousePositionInWorldSpace();
        DrawCurve(startPosition, endPosition);
        // 箭头朝向曲线末端切线(沿弯曲方向)
        arrowHead.transform.position = endPosition;
        arrowHead.transform.right = GetTangent(startPosition, endPosition, 1f);
    }

    public void SetupArrow(Vector3 startPosition)
    {
        this.startPosition = startPosition;
        lineRenderer.useWorldSpace = true; // 用世界坐标画曲线
        lineRenderer.positionCount = curveSegments;
        DrawCurve(startPosition, MouseUtil.GetMousePositionInWorldSpace());
    }

    // 控制点：连线中点向上偏移 bowAmount*长度 -> 向上弧线(更像抛射)
    private Vector3 GetControlPoint(Vector3 start, Vector3 end)
    {
        Vector3 mid = (start + end) / 2f;
        Vector3 dir = end - start;
        return mid + UnityEngine.Vector3.up * (dir.magnitude * bowAmount);
    }

    // 二次贝塞尔: B(t) = (1-t)^2 P0 + 2(1-t)t P1 + t^2 P2
    private Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }

    private Vector3 GetTangent(Vector3 start, Vector3 end, float t)
    {
        Vector3 control = GetControlPoint(start, end);
        Vector3 tangent = 2f * (1f - t) * (control - start) + 2f * t * (end - control);
        return tangent.normalized;
    }

    private void DrawCurve(Vector3 start, Vector3 end)
    {
        // 在箭头头前面留出间隙, 避免头盖住线尾
        float gap = 0.35f;
        Vector3 endTangent = GetTangent(start, end, 1f);
        Vector3 lineEnd = end - endTangent * gap;
        Vector3 control = GetControlPoint(start, lineEnd);
        for (int i = 0; i < curveSegments; i++)
        {
            float t = i / (float)(curveSegments - 1);
            lineRenderer.SetPosition(i, QuadraticBezier(start, control, lineEnd, t));
        }
    }
}
