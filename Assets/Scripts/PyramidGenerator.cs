using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public Material material;
    public Vector3 baseCenter;
    public float baseWidth;
    public float height;
    public float focalLength;

    private void OnPostRender()
    {
        DrawPyramid();
    }

    private void OnDrawGizmos()
    {
        DrawPyramid();
    }

    private void DrawPyramid()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        Vector3[] baseCorners = new Vector3[]
        {
            new Vector3(baseCenter.x - baseWidth / 2, baseCenter.y - baseWidth / 2, baseCenter.z),
            new Vector3(baseCenter.x + baseWidth / 2, baseCenter.y - baseWidth / 2, baseCenter.z),
            new Vector3(baseCenter.x + baseWidth / 2, baseCenter.y + baseWidth / 2, baseCenter.z),
            new Vector3(baseCenter.x - baseWidth / 2, baseCenter.y + baseWidth / 2, baseCenter.z)
        };

        Vector3 apex = new Vector3(baseCenter.x, baseCenter.y, baseCenter.z + height);

        for (int i = 0; i < baseCorners.Length; i++)
        {
            DrawLine(baseCorners[i], baseCorners[(i + 1) % baseCorners.Length]);
            DrawLine(baseCorners[i], apex);
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