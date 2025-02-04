using System;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject    carPrefab;
    [SerializeField] CameraReticle reticle;
    [SerializeField] SurfaceManager surfaceManager;

    private event Action TouchEvent;


    private ChasingCar _car;

    private void Awake()
    {
        TouchEvent += SpawnCar;
    }

    private void Update()
    {
        DetectTouch();
    }

    private void SpawnCar()
    {
        if (reticle.CurrentPlane == null)
            return;

        var obj = Instantiate(carPrefab, reticle.transform.position, Quaternion.identity);

        _car = obj.GetComponent<ChasingCar>();

        _car.Initialize(reticle);

        surfaceManager.FixPlane(reticle.CurrentPlane);

        TouchEvent -= SpawnCar;
    }

    private void DetectTouch()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began) return;

        TouchEvent?.Invoke();
    }
}
