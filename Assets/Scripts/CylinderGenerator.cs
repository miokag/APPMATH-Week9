using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGenerator : MonoBehaviour
{
    public Material material;
    public Vector3 center;
    public float radius;
    public float height;
    public int segments = 10;
    public float focalLength;
    public Vector3 rotation = Vector3.zero;

    private void OnPostRender()
    {
        DrawCylinder();
    }

    private void OnDrawGizmos()
    {
        DrawCylinder();
    }

    private void DrawCylinder()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        Vector3[] bottomCircle = new Vector3[segments];
        Vector3[] topCircle = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            bottomCircle[i] = new Vector3(center.x + radius * Mathf.Cos(angle), center.y + radius * Mathf.Sin(angle), center.z);
            topCircle[i] = new Vector3(center.x + radius * Mathf.Cos(angle), center.y + radius * Mathf.Sin(angle), center.z + height);
        }

        Quaternion rotationQuat = Quaternion.Euler(rotation);
        for (int i = 0; i < segments; i++)
        {
            bottomCircle[i] = RotatePointAroundPivot(bottomCircle[i], center, rotationQuat);
            topCircle[i] = RotatePointAroundPivot(topCircle[i], center, rotationQuat);
        }

        for (int i = 0; i < segments; i++)
        {
            DrawLine(bottomCircle[i], bottomCircle[(i + 1) % segments]);
            DrawLine(topCircle[i], topCircle[(i + 1) % segments]);
            DrawLine(bottomCircle[i], topCircle[i]);
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

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        direction = rotation * direction;
        return pivot + direction;
    }
}