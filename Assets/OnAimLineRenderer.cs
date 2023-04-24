using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class OnAimLineRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _initialVelocity = 5;
    [SerializeField] private float _angle = 45;
    [SerializeField] private float _step = 0.1f;

    private Camera _camera;
    private float _height;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - _firePoint.position;
            Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
            Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);

            _height = targetPos.y + targetPos.magnitude / 2f;
            _height = Mathf.Max(0.01f, _height);
            float angle, v0, time;
            CalculatePathWithHeight(targetPos, _height, out v0, out angle, out time);
            DrawPath(groundDirection.normalized, v0, angle, time, _step);

            if (ManosInputController.Instance.Space.IsPressed())
            {
                StopAllCoroutines();
                StartCoroutine(Coroutine_Movement(groundDirection.normalized, v0, angle, time));
            }
        }
    }

    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        _line.positionCount = (int)(time / step) + 2;
        int count = 0;
        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _line.SetPosition(count, _firePoint.position + direction * x + Vector3.up * y);
            count++;
        }
        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _line.SetPosition(count, _firePoint.position + direction * xFinal + Vector3.up * yFinal);
    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float a = Mathf.Sqrt(2 * g * h);
        float b = (-0.5f * g);
        float c = -yt;

        float tPlus = QuadraticEquartion(a, b, c, 1);
        float tMin = QuadraticEquartion(a, b, c, -1);

        if (tPlus > tMin)
            time = tPlus;
        else
            time = tMin;

        angle = Mathf.Atan(b * time / xt);

        v0 = b / Mathf.Sin(angle);
    }

    private float QuadraticEquartion(float a, float b, float c, float sign) 
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a); ;
    }

    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time)
    {
        float t = 0;
        while (t < time)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _firePoint.position + direction * x + Vector3.up * y;
            t += Time.deltaTime;
            yield return null;
        }
    }
}