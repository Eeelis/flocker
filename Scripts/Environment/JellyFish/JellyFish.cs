using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
    [SerializeField] private float stretchFactor = 0.1f;
    [SerializeField] private float moveSpeed = 2f;              
    [SerializeField] private float pulseDuration = 2f;            
    [SerializeField] private AnimationCurve pulseCurve;      

    private Vector3 originalScale;
    private float timeSinceLastPulse = 0f;      

    private void OnEnable()
    {
        originalScale = transform.localScale;
    }

    void Start()
    {
        if (pulseCurve == null)
        {
            pulseCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
        }
    }

    void Update()
    {
        float time = Time.time;

        timeSinceLastPulse += Time.deltaTime;

        float curveTime = (timeSinceLastPulse % pulseDuration) / pulseDuration;
        float speedModifier = pulseCurve.Evaluate(curveTime);

        Vector2 movementDirection = transform.up;

        Vector2 movement = movementDirection * moveSpeed * speedModifier * Time.deltaTime;
        transform.Translate(movement, Space.World);

        float stretchAmount = 1 + speedModifier * stretchFactor;
        float squishAmount = 1 / stretchAmount;
        transform.localScale = new Vector3(originalScale.x * stretchAmount, originalScale.y * squishAmount, originalScale.z);
    }
}
