using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private void Update()
    {
        transform.eulerAngles += Vector3.one * Time.deltaTime * 5f;
    }
}
