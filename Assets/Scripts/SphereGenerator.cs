using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public Material material;
    public Vector3 center;
    public float radius;
    public int segments = 10;
    public float focalLength;

    private void OnPostRender()
    {
        DrawSphere();
    }

    private void OnDrawGizmos()
    {
        DrawSphere();
    }

    private void DrawSphere()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        for (int i = 0; i < segments; i++)
        {
            float lat0 = Mathf.PI * (-0.5f + (float)i / segments);
            float lat1 = Mathf.PI * (-0.5f + (float)(i + 1) / segments);

            for (int j = 0; j < segments; j++)
            {
                float lon0 = 2 * Mathf.PI * (float)j / segments;
                float lon1 = 2 * Mathf.PI * (float)(j + 1) / segments;

                Vector3 p0 = new Vector3(center.x + radius * Mathf.Cos(lat0) * Mathf.Cos(lon0), center.y + radius * Mathf.Cos(lat0) * Mathf.Sin(lon0), center.z + radius * Mathf.Sin(lat0));
                Vector3 p1 = new Vector3(center.x + radius * Mathf.Cos(lat1) * Mathf.Cos(lon0), center.y + radius * Mathf.Cos(lat1) * Mathf.Sin(lon0), center.z + radius * Mathf.Sin(lat1));
                Vector3 p2 = new Vector3(center.x + radius * Mathf.Cos(lat1) * Mathf.Cos(lon1), center.y + radius * Mathf.Cos(lat1) * Mathf.Sin(lon1), center.z + radius * Mathf.Sin(lat1));
                Vector3 p3 = new Vector3(center.x + radius * Mathf.Cos(lat0) * Mathf.Cos(lon1), center.y + radius * Mathf.Cos(lat0) * Mathf.Sin(lon1), center.z + radius * Mathf.Sin(lat0));

                DrawLine(p0, p1);
                DrawLine(p1, p2);
                DrawLine(p2, p3);
                DrawLine(p3, p0);
            }
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