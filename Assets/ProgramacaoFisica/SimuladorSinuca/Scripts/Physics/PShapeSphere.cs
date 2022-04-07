using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShapeSphere : PShape
{
    public float radius;

    private void Awake()
    {
        shapeType = PShapeType.SPHERE;
    }

    private void Start()
    {
        Position = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        if (radius <= 0) return; 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
