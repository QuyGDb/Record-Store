using UnityEngine;

public class LocalAxis : MonoBehaviour
{
    private LineRenderer lineX, lineY, lineZ;

    void Awake()
    {
        lineX = CreateLine(Color.red);
        lineY = CreateLine(Color.green);
        lineZ = CreateLine(Color.blue);
    }

    private void OnEnable()
    {
        lineX.enabled = true;
        lineY.enabled = true;
        lineZ.enabled = true;
    }
    private void OnDisable()
    {
        lineX.enabled = false;
        lineY.enabled = false;
        lineZ.enabled = false;
    }


    void Update()
    {
        DrawAxis();
    }

    LineRenderer CreateLine(Color color)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.parent = transform; // Gắn vào object chính
        LineRenderer line = lineObj.AddComponent<LineRenderer>();

        // Thiết lập LineRenderer
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        line.positionCount = 2;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = color;
        line.endColor = color;

        return line;
    }

    void DrawAxis()
    {
        float axisLength = 0.3f; // Độ dài trục

        // Vẽ trục X (màu đỏ)
        lineX.SetPosition(0, transform.position);
        lineX.SetPosition(1, transform.position + transform.right * axisLength);

        // Vẽ trục Y (màu xanh lá)
        lineY.SetPosition(0, transform.position);
        lineY.SetPosition(1, transform.position + transform.up * axisLength);

        // Vẽ trục Z (màu xanh dương)
        lineZ.SetPosition(0, transform.position);
        lineZ.SetPosition(1, transform.position + transform.forward * axisLength);
    }
}
