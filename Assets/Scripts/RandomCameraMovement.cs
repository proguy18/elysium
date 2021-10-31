using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class RandomCameraMovement : MonoBehaviour
{
    [SerializeField] private Transform minPoint;
    [SerializeField] private Transform maxPoint;
    [SerializeField] private float speed;
    private Transform camTransform;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float movementDuration;
    

    private void Awake()
    {
        camTransform = Camera.main.transform;
        camTransform.position = minPoint.position;
    }

    private void Start()
    {
        endPoint = GenerateRandomPoint();
        StartCameraMovement();
    }

    private void StartCameraMovement()
    {
        ChooseRandomPoints();
        CalculateMovementDuration();
        StartCoroutine(MoveCamera());
    }

    private void ChooseRandomPoints()
    {
        startPoint = endPoint;
        endPoint = GenerateRandomPoint();
    }

    private Vector3 GenerateRandomPoint()
    {
        var minPosition = minPoint.position;
        var maxPosition = maxPoint.position;
        float x = Random.Range(minPosition.x, maxPosition.x);
        float y = Random.Range(minPosition.y, maxPosition.y);
        float z = Random.Range(minPosition.z, maxPosition.z);

        return new Vector3(x, y, z);
    }

    private IEnumerator MoveCamera()
    {
        float t = 0.0f;
        while (t < movementDuration)
        {
            Debug.Log("t:" + t);
            camTransform.position = Vector3.Lerp(startPoint, endPoint, t/movementDuration);
            t += Time.deltaTime;
            yield return null;
        }
        StartCameraMovement();
    }

    private void CalculateMovementDuration()
    {
        movementDuration = Vector3.Distance(startPoint, endPoint) / speed;
    }
}
