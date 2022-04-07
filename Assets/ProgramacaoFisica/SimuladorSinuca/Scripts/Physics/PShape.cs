using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShape : MonoBehaviour
{
    public enum PShapeType
    {
        SPHERE,
        CUBE,
        PLANE
    }
    public bool IsStatic;
    public Vector3 Position;
    protected  PShapeType shapeType;
    public Vector3 center;

    private void Awake()
    {
        Position = transform.position; 
    }

    public PShapeType GetShapeType()
    {
        return shapeType;
    }
}
