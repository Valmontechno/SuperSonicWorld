using Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] Vector2 followOffset;

    [SerializeField] CameraOffsetBehaviour cameraOffsetBehaviour;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraOffsetBehaviour.offset = followOffset;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraOffsetBehaviour.offset = Vector2.zero;
        }
    }
}
