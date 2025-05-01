using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffsetBehaviour : MonoBehaviour
{
    public Vector2 offset;
    [SerializeField] float speed;

    CinemachineCameraOffset cameraOffset;

    void Start()
    {
        cameraOffset = GetComponent<CinemachineCameraOffset>();
    }

    void Update()
    {
        cameraOffset.m_Offset = Vector2.MoveTowards(cameraOffset.m_Offset, offset, speed * Time.deltaTime);
    }
}
