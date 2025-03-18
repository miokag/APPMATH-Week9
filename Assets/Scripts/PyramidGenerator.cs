using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public Material material; 
    public Vector3 baseCenter = new Vector3(0, 0, 10); 
    public float baseWidth = 2f; 
    public float height = 3f; 
    public float focalLength = 5f; 
    public Vector3 rotation = Vector3.zero; 

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

        Quaternion rotationQuat = Quaternion.Euler(rotation);
        for (int i = 0; i < baseCorners.Length; i++)
        {
            baseCorners[i] = RotatePointAroundPivot(baseCorners[i], baseCenter, rotationQuat);
        }
        apex = RotatePointAroundPivot(apex, baseCenter, rotationQuat);

        for (int i = 0; i < baseCorners.Length; i++)
        {
            Vector3 corner1 = baseCorners[i];
            Vector3 corner2 = baseCorners[(i + 1) % baseCorners.Length];

            Vector2 projectedCorner1 = ProjectTo2D(corner1);
            Vector2 projectedCorner2 = ProjectTo2D(corner2);

            GL.Vertex3(projectedCorner1.x, projectedCorner1.y, 0);
            GL.Vertex3(projectedCorner2.x, projectedCorner2.y, 0);
        }

        for (int i = 0; i < baseCorners.Length; i++)
        {
            Vector3 baseCorner = baseCorners[i];

            Vector2 projectedBaseCorner = ProjectTo2D(baseCorner);
            Vector2 projectedApex = ProjectTo2D(apex);

            GL.Vertex3(projectedBaseCorner.x, projectedBaseCorner.y, 0);
            GL.Vertex3(projectedApex.x, projectedApex.y, 0);
        }

        GL.End();
        GL.PopMatrix();
    }

    private Vector2 ProjectTo2D(Vector3 point)
    {
        float scale = focalLength / (point.z + focalLength);
        return new Vector2(point.x * scale, point.y * scale);
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot; 
        direction = rotation * direction; 
        return pivot + direction; 
    }
}