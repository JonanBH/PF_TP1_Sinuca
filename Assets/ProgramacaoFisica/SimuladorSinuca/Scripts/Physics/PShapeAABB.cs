using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShapeAABB : PShape
{
    public Vector3 min;
    public Vector3 max;
    public Vector3 size = Vector3.one;

    private void Awake()
    {
        shapeType = PShapeType.CUBE;
        Position = transform.position;
    }

    private void FixedUpdate()
    {
        min = (Position + center) - size * 0.5f;
        max = (Position + center) + size * 0.5f;
    }

    public float SqrdDistanceToPoint(Vector3 point)
    {
        Vector3 closestPoint = ClosestPoint(point);
        return Vector3.SqrMagnitude(point - closestPoint);
    }

    public Vector3 ClosestPoint(Vector3 point)
    {

        point.x = Mathf.Clamp(point.x, min.x, max.x);
        point.y = Mathf.Clamp(point.y, min.y, max.y);
        point.z = Mathf.Clamp(point.z, min.z, max.z);

        return point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        min = (center) - size * 0.5f;
        max = (center) + size * 0.5f;

        Gizmos.DrawLine(min + transform.position, min + transform.position + max.x * Vector3.right * 2);
        Gizmos.DrawLine(min + transform.position, min + transform.position + max.y * Vector3.up * 2);
        Gizmos.DrawLine(min + transform.position, min + transform.position + max.z * Vector3.forward * 2);

        Gizmos.DrawLine(max + transform.position, max + transform.position - min.x * Vector3.left * 2);
        Gizmos.DrawLine(max + transform.position, max + transform.position - min.y * Vector3.down * 2);
        Gizmos.DrawLine(max + transform.position, max + transform.position - min.z * Vector3.back * 2);
    }
}
