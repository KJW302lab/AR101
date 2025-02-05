using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PackageSpawner : MonoBehaviour
{
    public Package Package { get; private set; }

    [SerializeField] SurfaceManager surfaceManager;
    [SerializeField] GameObject     packagePrefab;

    public static Vector3 GetRandomPointOnPlane(ARPlane plane)
    {
        // 1. ARPlane의 Mesh 데이터를 가져옴
        Mesh mesh = plane.GetComponent<ARPlaneMeshVisualizer>().mesh;
        if (mesh == null) return plane.transform.position; // 평면이 없으면 기본 위치 반환

        // 2. 삼각형 데이터 가져오기
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        // 3. 무작위 삼각형 선택
        int triangleIndex = Random.Range(0, triangles.Length / 3) * 3;

        // 4. 선택된 삼각형의 꼭짓점 가져오기
        Vector3 v0 = vertices[triangles[triangleIndex]];
        Vector3 v1 = vertices[triangles[triangleIndex + 1]];
        Vector3 v2 = vertices[triangles[triangleIndex + 2]];

        // 5. 삼각형 내부의 랜덤 위치 찾기 (Barycentric 좌표 사용)
        Vector3 randomPoint = GetRandomPointInTriangle(v0, v1, v2);

        // 6. 로컬 좌표를 월드 좌표로 변환
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
