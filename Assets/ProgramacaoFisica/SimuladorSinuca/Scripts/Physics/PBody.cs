using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBody : MonoBehaviour
{
    public Vector3 Velocity, Force, Gravity;
    public float Mass;
    public PShape Shape;
    public float Bounciness = 0.5f;
    public float Drag = 0.5f;
    private Vector3 ImpactForce = Vector3.zero;
    public float bounciness = 1;
    public Action<PBody> OnCollision;

    public static Action<PBody> OnBodyDestroyed;

    public void PUpdate(float time)
    {
        AddForce(Velocity * -1 * Drag);
        Dynamic(time);
        Kinematic(time);
        transform.position = Shape.Position;
        Force = Vector3.zero;
        ImpactForce = Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        Force += force;
    }

    public void AddImpact(Vector3 impact)
    {
        ImpactForce += impact;
    }

    private void Dynamic(float time)
    {
        Vector3 acceleration = Force / Mass + Gravity;
        Velocity = Velocity + acceleration * time + ImpactForce;
    }

    private void Kinematic(float time)
    {
        Shape.Position = Shape.Position + Velocity * time;
    }

    private void OnDestroy()
    {
        OnBodyDestroyed?.Invoke(this);
    }

    public virtual void PCollisionEnter(PBody body)
    {
        OnCollision?.Invoke(body);
    }
}
