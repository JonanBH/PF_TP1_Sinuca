using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : PBody
{
    [SerializeField]
    private AudioSource audioSource;

    public override void PCollisionEnter(PBody body)
    {
        Ball ball = body.GetComponent<Ball>();

        if (ball)
        {
            ball.FellOnPocket();
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
    }
}
