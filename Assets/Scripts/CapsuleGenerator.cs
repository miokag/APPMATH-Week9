using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleGenerator : MonoBehaviour
{
    public Material material; 
    public Vector3 center;   
    public float radius = 1f; 
    public float height = 2f; 
    public int segments = 20;
    public float focalLength = 5f; 

    private void OnPostRender()
    {
        DrawCapsule();
    }

    private void OnDrawGizmos()
    {
        DrawCapsule();
    }

    private void DrawCapsule()
    {
        if (material == null)
        {
            Debug.LogError("Material is missing!");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        DrawCylinder(center, radius, height, segments);

        DrawHemisphere(center + Vector3.up * (height / 2), radius, segments, true);

        DrawHemisphere(center - Vector3.up * (height / 2), radius, segments, false);

        GL.End();
        GL.PopMatrix();
    }

    private void DrawCylinder(Vector3 center, float radius, float height, int segments)
    {
        Vector3 bottomCenter = center - Vector3.up * (height / 2);
        Vector3 topCenter = center + Vector3.up * (height / 2);

        for (int i = 0; i < segments; i++)
        {
            float angle1 = 2 * Mathf.PI * i / segments;
            float angle2 = 2 * Mathf.PI * (i + 1) / segments;

            // Bottom circle
            Vector3 bottomPoint1 = bottomCenter + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 bottomPoint2 = bottomCenter + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;
            DrawLine(bottomPoint1, bottomPoint2);

            // Top circle
            Vector3 topPoint1 = topCenter + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 topPoint2 = topCenter + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;
            DrawLine(topPoint1, topPoint2);

            // Vertical lines connecting top and bottom circles
            DrawLine(bottomPoint1, topPoint1);
        }
    }

    private void DrawHemisphere(Vector3 center, float radius, int segments, bool isTop)
    {
        for (int i = 0; i < segments / 2; i++)
        {
            float lat1 = Mathf.PI * ((float)i / segments);
            float lat2 = Mathf.PI * ((float)(i + 1) / segments);

            for (int j = 0; j < segments; j++)
            {
                float lon1 = 2 * Mathf.PI * ((float)j / segments);
                float lon2 = 2 * Mathf.PI * ((float)(j + 1) / segments);

                Vector3 p1 = GetSpherePoint(center, radius, lat1, lon1, isTop);
                Vector3 p2 = GetSpherePoint(center, radius, lat2, lon1, isTop);
                Vector3 p3 = GetSpherePoint(center, radius, lat2, lon2, isTop);
                Vector3 p4 = GetSpherePoint(center, radius, lat1, lon2, isTop);

                DrawLine(p1, p2);
                DrawLine(p2, p3);
                DrawLine(p3, p4);
                DrawLine(p4, p1);
            }
        }
    }

    private Vector3 GetSpherePoint(Vector3 center, float radius, float lat, float lon, bool isTop)
    {
        float y = Mathf.Sin(lat) * radius;
        float x = Mathf.Cos(lat) * Mathf.Cos(lon) * radius;
        float z = Mathf.Cos(lat) * Mathf.Sin(lon) * radius;

        if (isTop)
            return center + new Vector3(x, y, z);
        else
            return center + new Vector3(x, -y, z);
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