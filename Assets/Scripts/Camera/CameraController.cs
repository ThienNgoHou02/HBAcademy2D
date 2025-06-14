using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance {  get; private set; }

    [Header("Target(transform)")]
    public Transform target;
    [Header("Camera Offset")]
    public float X;
    public float Y;
    public float Z;

    [SerializeField]
    private float xMin;
    [SerializeField]
    private float xMax;    
    [SerializeField]
    private float yMin;
    [SerializeField]
    private float yMax;

    private Vector3 cameraOffset;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cameraOffset = new Vector3 (X, Y, Z);
        transform.position = target.position;
    }
    private void LateUpdate()
    {
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Vector3 targetPos = target.position + cameraOffset;

        float minX = xMin + cameraWidth / 2f;
        float maxX = xMax - cameraWidth / 2f;
        float minY = yMin + cameraHeight / 2f;
        float maxY = yMax - cameraHeight / 2f;

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, -10.0f);
        transform.position = Vector3.Lerp(transform.position, clampedPos, Time.deltaTime * 2f);
    }
    public void Shake(float shakePower)
    {
        transform.position += new Vector3(Random.Range(-shakePower, shakePower), Random.Range(-shakePower, shakePower), 0);
    }
}
