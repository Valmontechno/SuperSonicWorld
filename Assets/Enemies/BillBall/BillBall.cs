using Cinemachine.Utility;
using UnityEngine;

public class BillBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float destroyCameraDistance;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.right.normalized;

        Vector3 cameraDistance = Camera.main.transform.position - transform.position;
        cameraDistance = cameraDistance.Abs();
        if (cameraDistance.x >= destroyCameraDistance || cameraDistance.y >= destroyCameraDistance)
        {
            Destroy(gameObject);
        }
    }
}
