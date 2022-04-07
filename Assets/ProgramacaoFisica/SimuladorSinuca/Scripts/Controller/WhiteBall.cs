using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WhiteBall : Ball
{
    [SerializeField]
    private float minShootPower = 1;
    [SerializeField]
    private float maxShootPower = 10;
    [SerializeField]
    private float maxShootVectorSize = 0.15f;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private PowerGauge powerGauge;
    [SerializeField]
    private float lineSize = 50;

    private bool canShoot = true;

    private Vector3 startingMousePosition;
    private Vector3 currentMousePosition;

    private void FixedUpdate()
    {
        if(body.Velocity.magnitude <= minBallSpeed)
        {
            body.Velocity = Vector3.zero;
            canShoot = true;
        }
    }

    public void Shoot(Vector3 direction, float power)
    {
        if (canShoot == false) return;

        power = Mathf.Clamp(power, minShootPower, maxShootPower);
        canShoot = false;

        body.AddImpact(direction * power);

        SinucaGameController.singleton.UpdateHitsCounter();
        PlayAudio(hitSound);
    }

    private void Update()
    {
        if (canShoot == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            startingMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            powerGauge.StartGauge(body.Shape.Position);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition) - startingMousePosition;
            difference = Truncate(difference, maxShootVectorSize);

            Vector3 direction = new Vector3(-difference.x, 0, -difference.y);
            float power = CalculatePower(difference);

            powerGauge.UpdateGauge(body.Shape.Position + direction * (power/maxShootPower) * lineSize);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition) - startingMousePosition;
            difference = Truncate(difference, maxShootVectorSize);

            Debug.Log("difference = " + difference);

            Vector3 direction = new Vector3(-difference.x, 0, -difference.y);
            float power = CalculatePower(difference);

            Debug.Log("impact = " + (direction * power).ToString());

            Shoot(direction, power);
            powerGauge.HideRenderer();
        }
    }

    private float CalculatePower(Vector3 inputVector)
    {
        float power = Mathf.Lerp(minShootPower, maxShootPower, inputVector.magnitude / maxShootVectorSize);
        return power;
    }

    private Vector3 Truncate(Vector3 vector, float maxValue)
    {
        return Vector3.ClampMagnitude(vector, maxValue);
    }

    public override void FellOnPocket()
    {
        Debug.Log("LOST");
        SinucaGameController.singleton.WhiteBallPocketed();
        Destroy(gameObject);
    }
}
