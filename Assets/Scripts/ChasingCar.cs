using UnityEngine;

public class ChasingCar : MonoBehaviour
{
    private CameraReticle _reticle;

    private float         _speed = 0.3f;

    public void Initialize(CameraReticle reticle)
    {
        _reticle = reticle;
    }

    private void Update()
    {
        var trackingPosition = _reticle.transform.position;

        if (Vector3.Distance(trackingPosition, transform.position) < 0.1)
            return;

        Vector3 direction = (trackingPosition - transform.position);

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        transform.position = Vector3.MoveTowards(transform.position, trackingPosition, Time.deltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Package package = other.GetComponent<Package>();

        if (package != null)
            Destroy(package.gameObject);
    }
}
