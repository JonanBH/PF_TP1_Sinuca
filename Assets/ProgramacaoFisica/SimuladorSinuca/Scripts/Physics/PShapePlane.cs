using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShapePlane : PShape
{
    public Vector3 Normal;
    // Start is called before the first frame update
    void Awake()
    {
        shapeType = PShapeType.PLANE;
        Position = transform.position;
    }

    public float ClosestDistance(Vector3 point)
    {
        Vector3 d = point - transform.position;
        return Vector3.Dot(d, transform.up);
    }
}
