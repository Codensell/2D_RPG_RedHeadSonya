using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private ParallaxLayer[] backgroundLayers;
    
    private Camera _mainCamera;
    private float _currentCameraPositionX;
    private float _lastCameraPositionX;
    private float _distanceToMove;
    
    private float _cameraHalfWidth;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraHalfWidth = _mainCamera.orthographicSize *  _mainCamera.aspect;
        InitializeLayers();
    }

    private void FixedUpdate()
    {
        _currentCameraPositionX = _mainCamera.transform.position.x;
        _distanceToMove = _currentCameraPositionX - _lastCameraPositionX;
        _lastCameraPositionX = _currentCameraPositionX;

        float cameraLeftEdge = _currentCameraPositionX - _cameraHalfWidth;
        float cameraRightEdge = _currentCameraPositionX + _cameraHalfWidth;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.MoveBackground(_distanceToMove);
            layer.LoopBackGround(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void InitializeLayers()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}
