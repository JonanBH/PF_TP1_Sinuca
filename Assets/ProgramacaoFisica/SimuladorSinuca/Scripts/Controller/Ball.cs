using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    protected float minBallSpeed = 0.1f;
    [SerializeField]
    protected PBody body;

    [SerializeField]
    private AudioClip ballVsBallSound;
    [SerializeField]
    private AudioClip ballVsWoodSound;
    [SerializeField]
    private AudioClip holeSound;
    [SerializeField]
    protected AudioSource audioSource;

    private void Start()
    {
        body.OnCollision += HandleCollision;
    }

    private void FixedUpdate()
    {
        if (body.Velocity.magnitude <= minBallSpeed)
        {
            body.Velocity = Vector3.zero;
        }
    }

    public static Action<Ball> OnBallPocketed;
    public virtual void FellOnPocket() {
        OnBallPocketed?.Invoke(this);
        if(holeSound != null)
        {
            AudioSource.PlayClipAtPoint(holeSound, Camera.main.transform.position);
        }

        Destroy(gameObject);
    }

    protected void HandleCollision(PBody target)
    {
        AudioClip targetAudio = null;
        switch (target.Shape.GetShapeType())
        {
            case PShape.PShapeType.SPHERE:
                Ball b = target.GetComponent<Ball>();

                if (b != null)
                    targetAudio = ballVsBallSound;
                break;

            case PShape.PShapeType.CUBE:
                targetAudio = ballVsWoodSound;
                break;
        }

        if (targetAudio != null)
        {
            PlayAudio(targetAudio);
        }

    }

    protected void PlayAudio(AudioClip targetAudio)
    {
        audioSource.clip = targetAudio;
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

}
