using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private Vector3[] segmentPoses;
    [SerializeField] private Transform targetDir;
    [SerializeField] private float targetDist;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float trailSpeed;
    [SerializeField] private float wiggleSpeed;
    [SerializeField] private float wiggleMagnitude;
    [SerializeField] private Transform wiggleDir;

    private Vector3[] segmentV;
    private LineRenderer lineRenderer;

    private void OnEnable()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        for (int i = 0; i < length; i++)
        {
            segmentPoses[i] = transform.parent.position;
        }

        lineRenderer.startColor = lineRenderer.endColor = transform.parent.GetComponent<SpriteRenderer>().color;
        lineRenderer.SetPositions(segmentPoses);
        lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        for (int i = 0; i < length; i++)
        {
            segmentPoses[i] = transform.parent.position;
        }
        
        lineRenderer.SetPositions(segmentPoses);
        lineRenderer.enabled = false;
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        segmentPoses[0] = targetDir.position;

        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i-1] - targetDir.up * targetDist, ref segmentV[i], smoothSpeed + i / trailSpeed);
        }

        lineRenderer.SetPositions(segmentPoses);
    }
}
