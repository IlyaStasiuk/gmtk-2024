using System;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    enum Type
    {
        None,
        Straight,
        SinWave,
        Curve,
    }

    [Header("General refrences")]
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _start;
    [SerializeField] Transform _end;

    [Header("General parameters")]
    [SerializeField] int _pointCount = 20;
    [SerializeField][Range(0f, 0.9f)] float _ropeDrag = 0f;
    [SerializeField] float _ropeOffsetDrag = 1;
    [SerializeField] float _straighteningSpeed = 4;

    [SerializeField] AnimationCurve _curve;
    [SerializeField] float _curveWaveSize = 20;
    [SerializeField] float _curveWaveLenght = 20;

    [SerializeField] AnimationCurve _sinWaveSmoothCurve;
    [SerializeField] float _sinWaveSize = 20;
    [SerializeField] float _sinFrequency = 1;
    [SerializeField] float _sinSmoothStart = 1;
    [SerializeField] float _sinSmoothEnd = 1;

    // [HideInInspector] public bool disableDrag = false;

    Type _ropeType;

    Vector2[] _prevPositions;
    float[] _currentOffsets;
    float[] _targetOffsets;

    float _toStraightStartTime = float.NaN;

    public void SetSinWave() => SetRopeType(Type.SinWave);
    public void SetCurve() => SetRopeType(Type.Curve);
    public void SetStraight() => SetRopeType(Type.Straight);
    public void Clear()
    {
        enabled = false;
    }

    public void Straighten()
    {
        if (!enabled) return;
        if (Straightening) return;

        _toStraightStartTime = Time.time;
    }

    float StraightenProgress => !Straightening ? 0f : FloatUtils.MapRange(Time.time, _toStraightStartTime, _toStraightStartTime + 1f / _straighteningSpeed, 0f, 1f);
    bool Straightening => !float.IsNaN(_toStraightStartTime);

    void FinishStraightening()
    {
        _toStraightStartTime = float.NaN;
        SetStraight();
    }

    void SetRopeType(Type type)
    {
        enabled = true;
        _ropeType = type;
        _toStraightStartTime = float.NaN;
    }


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _lineRenderer.positionCount = _pointCount;
        _prevPositions = new Vector2[_pointCount];
        _currentOffsets = new float[_pointCount];
        _targetOffsets = new float[_pointCount];
    }

    private void OnEnable()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = _pointCount;
        ResetLine();
    }

    private void OnDisable()
    {
        _lineRenderer.enabled = false;
        _ropeType = Type.None;
    }

    void Update()
    {
        // Debug.Log("Update " + _ropeType);

        if (_ropeType == Type.None) return;

        if (StraightenProgress == 1f) FinishStraightening();

        switch (_ropeType)
        {
            case Type.Straight:
                EvaluateStraight();
                break;
            case Type.SinWave:
                EvaluateSin();
                break;
            case Type.Curve:
                EvaluateCurve();
                break;
        }

        DrawCurve();
    }

    static float SmoothStep(float x, float p = 1f)
    {
        x = Mathf.Clamp01(x);
        float v = 4f * x * (1f - x);
        if (p != 1f) v = Mathf.Pow(v, p);
        return v;
    }

    float DistancePointToLine(Vector2 A, Vector2 B, Vector2 P)
    {
        // Calculate the vector from A to B and from A to P
        Vector2 AB = B - A;
        Vector2 AP = P - A;

        // Get the perpendicular distance using the cross product formula in 2D
        float distance = Mathf.Abs(AB.x * AP.y - AB.y * AP.x) / AB.magnitude;

        return distance;
    }

    void DrawCurve()
    {
        Vector2 vector = _end.position - _start.position;
        Vector2 normal = Vector2.Perpendicular(vector).normalized;

        Vector2 endMovement = (Vector2)_end.position - _prevPositions[_pointCount - 1];

        bool sizeIncreases = Vector2.Dot(endMovement, vector) > 0;

        bool disableRopeDrag = Straightening || endMovement.magnitude > 0.1f;
        bool disableRopeOffsetDrag = sizeIncreases;

        // Debug.DrawRay(_start.position, _end.position - _start.position, disableRopeOffsetDrag ? Color.green : Color.red, 1f);

        for (int i = 0; i < _pointCount; i++)
        {
            float drawingProgress = (float)i / ((float)_pointCount - 1f);
            Vector2 targetPosition = Vector2.Lerp(_start.position, _end.position, drawingProgress);

            // Vector2 fromStartToCurrentPosition = _prevPositions[i] - (Vector2)_start.position;
            // Vector2 currentPosition = (Vector2)_start.position + fromStartToCurrentPosition * scale;
            Vector2 currentPosition = _prevPositions[i];

            // float distanceToTarget = DistancePointToLine(_start.position, _end.position, currentPosition);
            // Vector2 currentPositionCorrected = targetPosition + normal * distanceToTarget;
            // currentPosition = currentPositionCorrected;//Vector2.Lerp(currentPosition, currentPositionCorrected, 0.5f);

            Vector2 newPosition;
            if (disableRopeDrag) newPosition = targetPosition;
            else
            {
                float progress = (float)i / (_pointCount - 1);
                float prevPositionInfluence = SmoothStep(progress, 0.4f);
                newPosition = Vector2.Lerp(targetPosition, currentPosition, prevPositionInfluence * _ropeDrag);
            }

            float currentOffset = _currentOffsets[i];
            float targetOffset = _targetOffsets[i];
            float offset = disableRopeOffsetDrag ? targetOffset : Mathf.Lerp(currentOffset, targetOffset, 1f / _ropeOffsetDrag);

            // Vector2 tangent = (basePosition - prevBasePosition).normalized;
            // Vector2 normal = Vector2.Perpendicular(tangent).normalized;
            Vector2 offsetVector = normal * offset;
            Vector2 newPositionWithOffset = newPosition + offsetVector;

            // Debug.DrawRay(newPosition, offsetVector, Color.white, 0f);

            _currentOffsets[i] = offset;
            _prevPositions[i] = newPosition;
            _lineRenderer.SetPosition(i, newPositionWithOffset);
        }
    }

    void ResetLine()
    {
        for (int i = 0; i < _pointCount; i++)
        {
            _lineRenderer.SetPosition(i, _lineRenderer.transform.position);
            _prevPositions[i] = _lineRenderer.transform.position;
            _currentOffsets[i] = 0;
            _targetOffsets[i] = 0;
        }
    }

    // void SetTargetPosition(int i, Vector2 targetPosition) => _targetPositions[i] = targetPosition;
    void SetTargetOffset(int i, float offset) => _targetOffsets[i] = offset;

    void EvaluateCurve()
    {
        Vector2 vector = _end.position - _start.position;
        float distance = vector.magnitude;

        float waveSize = _curveWaveSize;
        if (Straightening)
        {
            waveSize *= 1f - StraightenProgress;
        }

        float waveLenght = Mathf.Max(_curveWaveLenght, distance);
        float waveOffset = waveLenght - distance;

        for (int i = 0; i < _pointCount; i++)
        {
            float drawingProgress = (float)i / ((float)_pointCount - 1f);
            float currentWaveDistance = waveOffset + drawingProgress * distance;
            float waveProgress = currentWaveDistance / waveLenght;

            float offset = _curve.Evaluate(waveProgress) * waveSize;
            float smothing = SmoothStep(drawingProgress);
            float smoothOffset = offset * smothing;

            // float offset = _curve.Evaluate(waveProgress) * waveSize;
            // Vector2 offset = Vector2.Perpendicular(vector).normalized * offsetValue;
            // Vector2 currentPosition = Vector2.Lerp(_start.position, _end.position, drawingProgress) + offset;

            SetTargetOffset(i, smoothOffset);
        }
    }


    void EvaluateSin()
    {
        Vector2 vector = _end.position - _start.position;
        float distance = vector.magnitude;
        float frequency = _sinFrequency;
        float waveSize = _sinWaveSize;

        if (Straightening)
        {
            float progress = StraightenProgress;
            frequency *= FloatUtils.MapRange(progress, 0f, 1f, 1f, 0.001f);
            waveSize *= FloatUtils.MapRange(progress, 0.5f, 1f, 1f, 0f);
        }

        // Debug.Log("EvaluateSin " + frequency);

        // Vector2 currentPosition = Vector2.zero;
        for (int i = 0; i < _pointCount; i++)
        {
            float drawingProgress = (float)i / ((float)_pointCount - 1f);
            float currentDistance = drawingProgress * distance;

            float offset = Mathf.Sin(currentDistance * frequency) * waveSize;
            float smothingStart = FloatUtils.MapRange01(currentDistance, 0f, _sinSmoothStart);
            float smothingEnd = FloatUtils.MapRange01(currentDistance, distance, distance - _sinSmoothEnd);
            float smothing = smothingStart * smothingEnd;
            // float smothing = _sinWaveSmoothCurve.Evaluate(drawingProgress);
            float smoothOffset = offset * smothing;
            // Vector2 offset = Mathf.Sin(currentDistance * Frequency) * WaveSize * Vector2.Perpendicular(vector).normalized;
            // Vector2 currentPosition = Vector2.Lerp(_start.position, _end.position, drawingProgress) + offset;


            SetTargetOffset(i, smoothOffset);
        }
    }

    void EvaluateStraight()
    {
        // Debug.Log("EvaluateStraight ");

        for (int i = 0; i < _pointCount; i++)
        {
            // float drawingProgress = (float)i / ((float)_pointCount - 1f);
            // Vector2 currentPosition = Vector2.Lerp(_start.position, _end.position, drawingProgress);

            SetTargetOffset(i, 0f);
        }
    }
}