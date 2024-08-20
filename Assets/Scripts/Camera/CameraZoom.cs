using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Rigidbody2D _target;
    [SerializeField] float _maxZoomOut = 40f;
    [SerializeField] float _minZoomOutTargetSpeed = 25f;
    [SerializeField] float _maxZoomOutTargetSpeed = 50f;
    [SerializeField] AnimationCurve _zoomOutSpeedCurve;
    [SerializeField] float _zoomOutSpeedMultiplier = 1f;

    float _initialZoom;

    float CurrentZoom { get => _camera.orthographicSize; set => _camera.orthographicSize = value; }
    float MaxZoom => _maxZoomOut;

    void Awake()
    {
        _initialZoom = CurrentZoom;
    }

    void Update()
    {
        float speed = _target.velocity.magnitude;
        float targetZoom = FloatUtils.MapRange(speed, _minZoomOutTargetSpeed, _maxZoomOutTargetSpeed, _initialZoom, MaxZoom);

        float zoomProgress = FloatUtils.MapRange01(speed, _minZoomOutTargetSpeed, _maxZoomOutTargetSpeed);
        float zoomSpeed = _zoomOutSpeedCurve.Evaluate(zoomProgress);

        // float zoomDelta = targetZoom - CurrentZoom;
        // bool zoomingOut = zoomDelta < 0f;

        float newZoom = Mathf.Lerp(CurrentZoom, targetZoom, zoomSpeed * _zoomOutSpeedMultiplier * Time.deltaTime);

        CurrentZoom = Mathf.Clamp(newZoom, _initialZoom, MaxZoom);
    }
}
