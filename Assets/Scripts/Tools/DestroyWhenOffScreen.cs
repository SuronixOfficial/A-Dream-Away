using UnityEngine;

public class DestroyWhenOffScreen : MonoBehaviour
{
    private Renderer _objectRenderer;
    private Camera _camera;

    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        _camera = Camera.main;
    }

    void Update()
    {
        if (!IsVisible())
            Destroy(gameObject);
    }

    bool IsVisible()
    {
        if (_objectRenderer == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        return GeometryUtility.TestPlanesAABB(planes, _objectRenderer.bounds);
    }
}
