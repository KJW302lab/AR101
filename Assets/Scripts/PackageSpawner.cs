using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PackageSpawner : MonoBehaviour
{
    public Package Package { get; private set; }

    [SerializeField] SurfaceManager surfaceManager;
    [SerializeField] GameObject     packagePrefab;

    public static Vector3 GetRandomPointOnPlane(ARPlane plane)
    {
        // 1. ARPlane�� Mesh �����͸� ������
        Mesh mesh = plane.GetComponent<ARPlaneMeshVisualizer>().mesh;
        if (mesh == null) return plane.transform.position; // ����� ������ �⺻ ��ġ ��ȯ

        // 2. �ﰢ�� ������ ��������
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        // 3. ������ �ﰢ�� ����
        int triangleIndex = Random.Range(0, triangles.Length / 3) * 3;

        // 4. ���õ� �ﰢ���� ������ ��������
        Vector3 v0 = vertices[triangles[triangleIndex]];
        Vector3 v1 = vertices[triangles[triangleIndex + 1]];
        Vector3 v2 = vertices[triangles[triangleIndex + 2]];

        // 5. �ﰢ�� ������ ���� ��ġ ã�� (Barycentric ��ǥ ���)
        Vector3 randomPoint = GetRandomPointInTriangle(v0, v1, v2);

        // 6. ���� ��ǥ�� ���� ��ǥ�� ��ȯ
        return plane.transform.TransformPoint(randomPoint);
    }

    private static Vector3 GetRandomPointInTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float u = Random.value;
        float v = Random.value;

        if (u + v > 1)
        {
            u = 1 - u;
            v = 1 - v;
        }

        float w = 1 - (u + v);

        return (v0 * w) + (v1 * u) + (v2 * v);
    }

    private void SpawnPackage(ARPlane plane)
    {
        var randomPoint = GetRandomPointOnPlane(plane);

        Package = Instantiate(packagePrefab, randomPoint, Quaternion.identity).GetComponent<Package>();
    }

    private void Update()
    {
        var fixedPlane = surfaceManager.FixedPlane;

        if (fixedPlane == null)
            return;

        if (Package != null)
            return;

        SpawnPackage(fixedPlane);

        var packagePosition = Package.gameObject.transform.position;
        packagePosition.Set(packagePosition.x, fixedPlane.center.y, packagePosition.z);
    }
}
