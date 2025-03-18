using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleGenerator : MonoBehaviour
{
    public Material material; 
    public Vector3 center = new Vector3(0, 0, 10); 
    public float width = 2f; 
    public float height = 2f; 
    public float depth = 2f; 
    public float focalLength = 5f; 
    public Vector3 rotation = Vector3.zero; 

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

        Quaternion rotationQuat = Quaternion.Euler(rotation);
        for (int i = 0; i < 4; i++)
        {
            frontCorners[i] = RotatePointAroundPivot(frontCorners[i], center, rotationQuat);
            backCorners[i] = RotatePointAroundPivot(backCorners[i], center, rotationQuat);
        }

        for (int i = 0; i < 4; i++)
        {
            DrawLine(frontCorners[i], frontCorners[(i + 1) % 4]);
        }

        for (int i = 0; i < 4; i++)
        {
            DrawLine(backCorners[i], backCorners[(i + 1) % 4]);
        }

        for (int i = 0; i < 4; i++)
        {
            DrawLine(frontCorners[i], backCorners[i]);
        }

        GL.End();
        GL.PopMatrix();
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        float startScale = focalLength / (start.z + focalLength);
        float endScale = focalLength / (end.z + focalLength);

        Vector3 scaledStart = new Vector3(start.x * startScale, start.y * startScale, 0);
        Vector3 scaledEnd = new Vector3(end.x * endScale, end.y * endScale, 0);

        GL.Vertex3(scaledStart.x, scaledStart.y, 0);
        GL.Vertex3(scaledEnd.x, scaledEnd.y, 0);
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot; 
        direction = rotation * direction; 
        return pivot + direction;
    }
}