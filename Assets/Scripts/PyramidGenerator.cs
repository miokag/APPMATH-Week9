using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public Material material; // Material for drawing lines
    public Vector3 baseCenter = new Vector3(0, 0, 10); // Center of the pyramid's base
    public float baseWidth = 2f; // Width of the base
    public float height = 3f; // Height of the pyramid
    public float focalLength = 5f; // Perspective projection strength
    public Vector3 rotation = Vector3.zero; // Rotation angles (in degrees) for the pyramid

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

        // Calculate the base corners in 3D space
        Vector3[] baseCorners = new Vector3[]
        {
            new Vector3(baseCenter.x - baseWidth / 2, baseCenter.y - baseWidth / 2, baseCenter.z),
            new Vector3(baseCenter.x + baseWidth / 2, baseCenter.y - baseWidth / 2, baseCenter.z),
            new Vector3(baseCenter.x + baseWidth / 2, baseCenter.y + baseWidth / 2, baseCenter.z),
            new Vector3(baseCenter.x - baseWidth / 2, baseCenter.y + baseWidth / 2, baseCenter.z)
        };

        // Calculate the apex of the pyramid in 3D space
        Vector3 apex = new Vector3(baseCenter.x, baseCenter.y, baseCenter.z + height);

        // Apply rotation to the base corners and apex
        Quaternion rotationQuat = Quaternion.Euler(rotation);
        for (int i = 0; i < baseCorners.Length; i++)
        {
            baseCorners[i] = RotatePointAroundPivot(baseCorners[i], baseCenter, rotationQuat);
        }
        apex = RotatePointAroundPivot(apex, baseCenter, rotationQuat);

        // Draw the base square with perspective
        for (int i = 0; i < baseCorners.Length; i++)
        {
            Vector3 corner1 = baseCorners[i];
            Vector3 corner2 = baseCorners[(i + 1) % baseCorners.Length];

            // Apply perspective projection to the base corners
            Vector2 projectedCorner1 = ProjectTo2D(corner1);
            Vector2 projectedCorner2 = ProjectTo2D(corner2);

            // Draw the base edges
            GL.Vertex3(projectedCorner1.x, projectedCorner1.y, 0);
            GL.Vertex3(projectedCorner2.x, projectedCorner2.y, 0);
        }

        // Draw the edges from the base to the apex with perspective
        for (int i = 0; i < baseCorners.Length; i++)
        {
            Vector3 baseCorner = baseCorners[i];

            // Apply perspective projection to the base corner and apex
            Vector2 projectedBaseCorner = ProjectTo2D(baseCorner);
            Vector2 projectedApex = ProjectTo2D(apex);

            // Draw the pyramid edges
            GL.Vertex3(projectedBaseCorner.x, projectedBaseCorner.y, 0);
            GL.Vertex3(projectedApex.x, projectedApex.y, 0);
        }

        GL.End();
        GL.PopMatrix();
    }

    private Vector2 ProjectTo2D(Vector3 point)
    {
        // Apply perspective projection
        float scale = focalLength / (point.z + focalLength);
        return new Vector2(point.x * scale, point.y * scale);
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        // Rotate a point around a pivot using a quaternion
        Vector3 direction = point - pivot; // Get the direction from the pivot to the point
        direction = rotation * direction; // Apply the rotation
        return pivot + direction; // Return the new position
    }
}