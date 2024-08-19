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


    public static PlayerTitanTransformation instance;
    public bool IsTitan => _isTitan;

    float _transformationBeginTime;
    bool _transformInProgress;
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
            if (IsTitan)
            {
                float progress = Mathf.Clamp01(elapsedTime / _toTitanDuration);
                ProcessTransformToTitan(progress);

                if (progress >= 1f)
                {
                    _transformInProgress = false;
                    FinishTransformToTitan();
                }
            }
            else
            {
                float progress = Mathf.Clamp01(elapsedTime / _toHumanDuration);
                ProcessTransformToHuman(progress);

                if (progress >= 1f)
                {
                    _transformInProgress = false;
                    FinishTransformToHuman();
                }
            }
        }
        else
        {
            if (IsTitan)
            {
                if (elapsedTime > _maxTitanDuration)
                {
                    TransformToHuman();
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Debug.Log("Speed: " + _rigidbody.velocity.magnitude);
                    if (_rigidbody.velocity.magnitude > _toTitanSpeedThreshold)
                    {
                        TransformToTitan();
                    }
                }
            }
        }
    }

    public void TransformToTitan()
    {
        _isTitan = true;
        _transformationBeginTime = Time.time;
        _transformInProgress = true;
        StartTransformToTitan();
    }

    public void TransformToHuman()
    {
        _isTitan = false;
        _transformationBeginTime = Time.time;
        _transformInProgress = true;
        StartTransformToHuman();
    }

    public void OnTitanHit(GameObject titan)
    {
        if (IsTitan)
        {
            TransformToHuman();
        }
    }

    void StartTransformToTitan()
    {
        if (_toTitanEffect != null)
        {
            GameObject effect = Instantiate(_toTitanEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void ProcessTransformToTitan(float progress)
    {
        if (_titanScale != 1f)
        {
            float scale = _toTitanScaleCurve.Evaluate(progress) * _titanScale;
            _transformsToScale.ForEach(t => t.localScale = new Vector3(scale, scale, scale));
        }
    }

    void FinishTransformToTitan()
    {
    }

    void StartTransformToHuman()
    {
        if (_toHumanEffect != null)
        {
            GameObject effect = Instantiate(_toHumanEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void ProcessTransformToHuman(float progress)
    {
        if (_titanScale != 1f)
        {
            float scale = _toHumanScaleCurve.Evaluate(1f - progress) * _titanScale;
            _transformsToScale.ForEach(t => t.localScale = new Vector3(scale, scale, scale));
        }
    }

    void FinishTransformToHuman()
    {
    }
}
