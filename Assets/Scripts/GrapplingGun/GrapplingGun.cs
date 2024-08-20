using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// https://www.youtube.com/watch?v=dnNCVcVS6uw
public class GrapplingGun : MonoBehaviour
{
    [SerializeField] Transform _gunPivot;
    [SerializeField] GrapplingHook _grapplingHook;
    [SerializeField] GrapplingSpring _grapplingSping;
    [SerializeField] private bool _leftButton = true;
    [SerializeField] private bool _allowHookByClick = false;
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 80)][SerializeField] private float rotationSpeed = 4;

    Vector2? _target = null;

    KeyCode Button => _leftButton ? KeyCode.Mouse0 : KeyCode.Mouse1;

    public GrapplingHook GrapplingHook => _grapplingHook;

    void Update()
    {
        if (_grapplingHook.State == GrapplingHook.HookState.Grappled) _grapplingSping.Grapple();
        else _grapplingSping.EndGrapple();

        if (SceneRestarter.instance.IsInDeathZone)
        {
            _grapplingHook.Retract();
            return;
        }

        if (PlayerTitanTransformation.instance.IsTitan)
        {
            _grapplingHook.Retract();
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_grapplingHook.State == GrapplingHook.HookState.Idle) RotateGun(mousePosition, true);
        else RotateGun(_grapplingHook.transform.position, true);

        if (!Menu.MenuScreen.Instance || !Menu.MenuScreen.Instance.IsPaused)
        {
            if (_allowHookByClick)
            {
                if (Input.GetKeyDown(Button))
                {
                    _target = mousePosition;
                }

                if (Input.GetKeyDown(Button) || Input.GetKeyUp(Button))
                {
                    if (_grapplingHook.State == GrapplingHook.HookState.Grappled) _grapplingHook.Retract();
                }

            }
            else
            {
                if (Input.GetKeyDown(Button))
                {
                    if (_grapplingHook.State == GrapplingHook.HookState.Idle) _target = mousePosition;
                }

                if (!Input.GetKey(Button))
                {
                    if (_grapplingHook.State == GrapplingHook.HookState.Grappled) _grapplingHook.Retract();
                }
            }
        }

        if (_target.HasValue && _grapplingHook.State == GrapplingHook.HookState.Idle)
        {
            Vector2 launchVector = _target.Value - (Vector2)_gunPivot.transform.position;
            _grapplingHook.Launch(launchVector.normalized);
            _target = null;
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - _gunPivot.transform.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            Quaternion startRotation = _gunPivot.transform.rotation;
            _gunPivot.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            _gunPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}