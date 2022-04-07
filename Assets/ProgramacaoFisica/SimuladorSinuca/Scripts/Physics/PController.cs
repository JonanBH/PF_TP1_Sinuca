using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PController : MonoBehaviour
{
    public List<PBody> bodies;

    private void Start()
    {
        bodies = new List<PBody>(FindObjectsOfType<PBody>());
        PBody.OnBodyDestroyed += HandleBodyDestroyed;
    }

    private void FixedUpdate()
    {
        foreach(PBody body in bodies)
        {
            if (body.Shape.IsStatic) continue;

            body.PUpdate(Time.fixedDeltaTime);
            foreach(PBody body2 in bodies)
            {
                if (!body2.Equals(body))
                {
                    PCollision collision;
                    if (body.Shape.GetShapeType() == PShape.PShapeType.SPHERE &&
                        body2.Shape.GetShapeType() == PShape.PShapeType.SPHERE)
                    {
                        if (SphereSphereCollision(body, body2, out collision))
                        {
                            body.PCollisionEnter(body2);
                            body2.PCollisionEnter(body);
                            ResolveSphereSpherePenetration(body, body2, collision);
                            ResolveCollision(body, body2, collision);
                        }
                    }

                    if (body.Shape.GetShapeType() == PShape.PShapeType.SPHERE &&
                        body2.Shape.GetShapeType() == PShape.PShapeType.CUBE)
                    {
                        if (SphereAABBCollision(body, body2, out collision))
                        {
                            body.PCollisionEnter(body2);
                            body2.PCollisionEnter(body);
                            ResolveSphereAABBPenetration(body, body2, collision);
                            ResolveCollision(body, body2, collision);
                        }
                    }
                }
            }
        }
    }

    private void ResolveSphereAABBPenetration(PBody b1, PBody b2, PCollision c)
    {
        if (b2.Shape.IsStatic)
        {
            b1.Shape.Position = b1.Shape.Position + c.Penetration * c.Normal;
        }
        else
        {
            float mt = b1.Mass + b2.Mass;
            b1.Shape.Position = b1.Shape.Position + c.Penetration * (b2.Mass / mt) * c.Normal / 2.0f;
            b2.Shape.Position = b2.Shape.Position - c.Penetration * (b1.Mass / mt) * c.Normal / 2.0f;
        }
    }

    private bool SphereSphereCollision(PBody b1, PBody b2, out PCollision collision)
    {
        collision = null;
        Vector3 distance = b1.Shape.Position - b2.Shape.Position;

        PShapeSphere sphere1 = (PShapeSphere)b1.Shape;
        PShapeSphere sphere2 = (PShapeSphere)b2.Shape;

        float penetration = distance.magnitude - (sphere1.radius + sphere2.radius);

        if (penetration <= 0)
        {
            collision = new PCollision();
            collision.Penetration = Mathf.Abs(penetration);
            collision.Normal = (b1.Shape.Position - b2.Shape.Position).normalized;
            collision.Point = (b2.Shape.Position + collision.Normal * sphere2.radius);
            return true;
        }

        return false;
    }

    private void ResolveSphereSpherePenetration(PBody b1, PBody b2, PCollision c)
    {
        if (b2.Shape.IsStatic)
        {
            b1.Shape.Position = b1.Shape.Position - c.Penetration * c.Normal;
        }
        else
        {
            float mt = b1.Mass + b2.Mass;
            b1.Shape.Position = b1.Shape.Position + c.Penetration * (b2.Mass / mt) * c.Normal / 2.0f;
            b2.Shape.Position = b2.Shape.Position - c.Penetration * (b1.Mass / mt) * c.Normal / 2.0f;
        }
    }

    private bool SphereAABBCollision(PBody b1, PBody b2, out PCollision collision)
    {
        PShapeSphere sphere = (PShapeSphere)b1.Shape;
        PShapeAABB box = (PShapeAABB)b2.Shape;
        collision = null;
        float sqrdDistance = box.SqrdDistanceToPoint(sphere.Position);

        if (sqrdDistance < sphere.radius * sphere.radius){
            collision = new PCollision();
            Vector3 closestPoint = box.ClosestPoint(sphere.Position);
            collision.Normal = (sphere.Position - closestPoint).normalized;
            collision.Penetration = sphere.radius - Vector3.Distance(closestPoint, sphere.Position);
            collision.Point = closestPoint;

            return true;
        }

        return false;
    }

    private void ResolveCollision(PBody b1, PBody b2, PCollision collision)
    {
        if (b2.Shape.IsStatic)
        {
            Vector3 reflect = Vector3.Reflect(b1.Velocity, collision.Normal);
            b1.Velocity = reflect;
        }
        else
        {
            float mt = b1.Mass + b2.Mass;
            float vt = b1.Velocity.magnitude + b2.Velocity.magnitude;
            b1.Velocity += collision.Normal * (b2.Mass / mt) * vt;
            b2.Velocity += -collision.Normal * (b1.Mass / mt) * vt;
        }
    }

    private void HandleBodyDestroyed(PBody body)
    {
        bodies.Remove(body);
    }
}
