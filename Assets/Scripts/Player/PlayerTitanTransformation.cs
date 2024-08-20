using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTitanTransformation : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] List<Transform> _transformsToScale;

    [SerializeField] float _toTitanDuration = 1f;
    [SerializeField] float _titanScale = 3f;
    [SerializeField] AnimationCurve _toTitanScaleCurve;
    [SerializeField] GameObject _toTitanEffect;

    [SerializeField] float _toHumanDuration = 1f;
    [SerializeField] AnimationCurve _toHumanScaleCurve;
    [SerializeField] GameObject _toHumanEffect;

    [SerializeField] float _toTitanSpeedThreshold = 50f;
    [SerializeField] float _maxTitanDuration = 5f;
    [SerializeField] float _transformDelay = 1f;


    public static PlayerTitanTransformation instance;
    public bool IsTitan => _isTitan;
    public bool TransformInProgressIsTitan => _transformInProgressIsTitan;

    float _transformationBeginTime;
    float _transformationEndTime;
    bool _transformInProgress;
    bool _transformInProgressIsTitan;
    bool _isTitan;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        float elapsedTime = Time.time - _transformationBeginTime;

        if (_transformInProgress)
        {
            if (_transformInProgressIsTitan)
            {
                float progress = Mathf.Clamp01(elapsedTime / _toTitanDuration);
                ProcessTransformToTitan(progress);
                if (progress >= 1f) FinishTransformToTitan();
            }
            else
            {
                float progress = Mathf.Clamp01(elapsedTime / _toHumanDuration);
                ProcessTransformToHuman(progress);
                if (progress >= 1f) FinishTransformToHuman();
            }
        }
        else
        {
            if (IsTitan)
            {
                bool keyUp = !Input.GetKey(KeyCode.LeftShift);
                bool timeUp = elapsedTime > _maxTitanDuration;
                bool speedLow = _rigidbody.velocity.magnitude < _toTitanSpeedThreshold;
                if (keyUp || timeUp || speedLow) TransformToHuman();
            }
            else
            {
                // Debug.Log("Speed: " + _rigidbody.velocity.magnitude);
                bool keyDown = Input.GetKey(KeyCode.LeftShift);
                bool canTransform = Time.time > _transformationEndTime + _transformDelay;
                bool hasEnoughtSpeed = _rigidbody.velocity.magnitude > _toTitanSpeedThreshold;
                if (keyDown && canTransform && hasEnoughtSpeed) TransformToTitan();
            }
        }
    }

    public void OnTitanHit(GameObject titan)
    {
        // if (IsTitan)
        // {
        //     TransformToHuman();
        // }
    }

    void TransformToTitan()
    {
        Debug.Log("TransformToTitan");

        _isTitan = true;
        _transformInProgressIsTitan = true;
        _transformationBeginTime = Time.time;
        _transformInProgress = true;

        if (_toTitanEffect != null)
        {
            GameObject effect = Instantiate(_toTitanEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void ProcessTransformToTitan(float progress)
    {
        // Debug.Log("ProcessTransformToTitan");

        if (_titanScale != 1f)
        {
            float scale = FloatUtils.MapRange(_toTitanScaleCurve.Evaluate(progress), 0f, 1f, 1f, _titanScale);
            _transformsToScale.ForEach(t => t.localScale = new Vector3(scale, scale, scale));
        }
    }

    void FinishTransformToTitan()
    {
        Debug.Log("FinishTransformToTitan");

        _transformationEndTime = Time.time;
        _transformInProgress = false;
    }

    void TransformToHuman()
    {
        Debug.Log("TransformToHuman");

        _transformInProgressIsTitan = false;
        _transformationBeginTime = Time.time;
        _transformInProgress = true;

        if (_toHumanEffect != null)
        {
            GameObject effect = Instantiate(_toHumanEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void ProcessTransformToHuman(float progress)
    {
        // Debug.Log("ProcessTransformToTitan");

        if (_titanScale != 1f)
        {
            float scale = FloatUtils.MapRange(_toHumanScaleCurve.Evaluate(1f - progress), 0f, 1f, 1f, _titanScale);
            _transformsToScale.ForEach(t => t.localScale = new Vector3(scale, scale, scale));
        }
    }

    void FinishTransformToHuman()
    {
        Debug.Log("FinishTransformToHuman");

        _isTitan = false;
        _transformationEndTime = Time.time;
        _transformInProgress = false;
    }
}
