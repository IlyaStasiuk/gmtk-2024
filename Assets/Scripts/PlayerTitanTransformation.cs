using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTitanTransformation : MonoBehaviour
{
    [SerializeField] GameObject _humanBody;
    [SerializeField] GameObject _titanBody;

    [SerializeField] float _titanScale = 1f;
    [SerializeField] float _toTitanDuration = 1f;
    [SerializeField] AnimationCurve _toTitanScaleCurve;
    [SerializeField] GameObject _toTitanEffect;

    [SerializeField] float _toHumanDuration = 1f;
    [SerializeField] AnimationCurve _toHumanScaleCurve;
    [SerializeField] GameObject _toHumanEffect;

    public bool IsTitan => _titanBody.activeSelf;

    // Usage: OnTransformedToTitan.AddListener(OnTransformedToTitan); - where OnTransformedToTitan is method like: void OnTransformedToTitan();
    // Usage: OnTransformedToTitan.RemoveListener(OnTransformedToTitan); - where OnTransformedToTitan is method like: void OnTransformedToTitan();
    public UnityEvent OnTransformedToTitanStarted;
    public UnityEvent OnTransformedToTitanFinished;
    public UnityEvent OnTransformedToHumanStarted;
    public UnityEvent OnTransformedToHumanFinished;

    float _transformationBeginTime;
    bool _transformInProgress;

    public void TransformToTitan()
    {
        _transformationBeginTime = Time.time;
        _transformInProgress = true;
        StartTransformToTitan();
    }

    public void TransformToHuman()
    {
        _transformationBeginTime = Time.time;
        _transformInProgress = true;
        StartTransformToHuman();
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
                if (_transformInProgress)
                {
                    float scale = _toHumanScaleCurve.Evaluate(1f - progress) * _titanScale;
                    _titanBody.transform.localScale = new Vector3(scale, scale, scale);

                    if (progress >= 1f)
                    {
                        _transformInProgress = false;
                        FinishTransformToHuman();
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (IsTitan) TransformToHuman();
                else TransformToTitan();
            }
        }
    }

    void StartTransformToTitan()
    {
        OnTransformedToTitanStarted.Invoke();

        _humanBody.SetActive(false);
        _titanBody.SetActive(true);

        if (_toTitanEffect != null)
        {
            GameObject effect = Instantiate(_toTitanEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void ProcessTransformToTitan(float progress)
    {
        float scale = _toTitanScaleCurve.Evaluate(progress) * _titanScale;
        _titanBody.transform.localScale = new Vector3(scale, scale, scale);
    }

    void FinishTransformToTitan()
    {
        OnTransformedToTitanFinished.Invoke();
    }

    void StartTransformToHuman()
    {
        OnTransformedToHumanStarted.Invoke();

        if (_toHumanEffect != null)
        {
            GameObject effect = Instantiate(_toHumanEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void ProcessTransformToHuman(float progress)
    {
        float scale = _toHumanScaleCurve.Evaluate(1f - progress) * _titanScale;
        _titanBody.transform.localScale = new Vector3(scale, scale, scale);
    }

    void FinishTransformToHuman()
    {
        OnTransformedToHumanFinished.Invoke();

        _humanBody.SetActive(true);
        _titanBody.SetActive(false);
    }
}
