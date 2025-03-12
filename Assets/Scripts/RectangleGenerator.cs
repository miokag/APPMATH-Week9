using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleGenerator : MonoBehaviour
{
    public Material material;
    public Vector3 center;
    public float width;
    public float height;
    public float depth;
    public float focalLength;

    private void OnPostRender()
    {
        DrawRectangle();
    }

    private void OnDrawGizmos()
    {
        DrawRectangle();
    }

    private void DrawRectangle()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        Vector3[] frontCorners = new Vector3[]
        {
            new Vector3(center.x - width / 2, center.y - height / 2, center.z - depth / 2),
            new Vector3(center.x + width / 2, center.y - height / 2, center.z - depth / 2),
            new Vector3(center.x + width / 2, center.y + height / 2, center.z - depth / 2),
            new Vector3(center.x - width / 2, center.y + height / 2, center.z - depth / 2)
        };

        Vector3[] backCorners = new Vector3[]
        {
            new Vector3(center.x - width / 2, center.y - height / 2, center.z + depth / 2),
            new Vector3(center.x + width / 2, center.y - height / 2, center.z + depth / 2),
            new Vector3(center.x + width / 2, center.y + height / 2, center.z + depth / 2),
            new Vector3(center.x - width / 2, center.y + height / 2, center.z + depth / 2)
        };

        for (int i = 0; i < 4; i++)
        {
            DrawLine(frontCorners[i], frontCorners[(i + 1) % 4]);
            DrawLine(backCorners[i], backCorners[(i + 1) % 4]);
            DrawLine(frontCorners[i], backCorners[i]);
        }

        GL.End();
        GL.PopMatrix();
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        float startScale = focalLength / (start.z + focalLength);
        float endScale = focalLength / (end.z + focalLength);

        Vector3 scaledStart = start * startScale;
        Vector3 scaledEnd = end * endScale;

        GL.Vertex3(scaledStart.x, scaledStart.y, 0);
        GL.Vertex3(scaledEnd.x, scaledEnd.y, 0);
    }
}