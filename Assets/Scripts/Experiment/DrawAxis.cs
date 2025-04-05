using UnityEngine;

public class DrawAxis : MonoBehaviour
{
    float axisLength = 0.5f;  // Tăng độ dài trục để dễ nhìn hơn
    float lineWidth = 0.02f;

    private void Start()
    {
        DrawLine(Vector3.zero, Vector3.right * axisLength, Color.red, lineWidth);   // X - Đỏ
        DrawLine(Vector3.zero, Vector3.up * axisLength, Color.green, lineWidth);    // Y - Xanh lá
        DrawLine(Vector3.zero, Vector3.forward * axisLength, Color.blue, lineWidth);// Z - Xanh dương
    }
    void DrawLine(Vector3 start, Vector3 end, Color color, float width)
    {
        GameObject line = new GameObject("Line");
        LineRenderer lr = line.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

}
