using System.Collections.Generic;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MeshRenderer mRenderer;

    private bool _isMovingForward = false;
    private List<Transform> _points;
    private int _currentPointIndex = 0;

    public int CurrentPointIndex { get => _currentPointIndex; set => _currentPointIndex = value; }

    // ����� ��� ��������� ������ ����� ��� ������
    public void SetPoints(List<Transform> points)
    {
        _points = points;
    }

    // ����� ��� ���������/���������� �������� ������
    public void MoveAnimation(bool move)
    {
        animator.SetBool("Move", move);
    }

    // ����� ���������� ���������
    public void SetColor(Material material)
    {
        mRenderer.material = material;
    }

    // ����� ���� �� �����
    public void SoloSprint(float speed, float offset)
    {
        // ���� ������ ������� ����� ������ ��� ����� ���������� �������� ������
        if (CurrentPointIndex >= _points.Count)
        {
            // ���������� ������ ������� ��������
            CurrentPointIndex = 0;
        }

        if (Vector3.Distance(transform.position, _points[CurrentPointIndex].position) > offset)
        {
            // ��������� ����������� � ��������� �����
            Vector3 direction = (_points[CurrentPointIndex].position - transform.position).normalized;
            // ������������ ������, ����� �� ������� � ������� ��������� �����
            transform.LookAt(_points[CurrentPointIndex]);
            // ������� ������ � ����������� ��������� ����� � ������ ��������
            transform.position += direction * speed * Time.deltaTime;
            MoveAnimation(true);
        }
        else
        {
            // ���� �������� ������� �����, ��������� � ���������
            CurrentPointIndex++;
        }
    }

    // ����� �������� �������. �������.
    public void SoloShuttle(float speed, float offset)
    {
        if (Vector3.Distance(transform.position, _points[CurrentPointIndex].position) > offset)
        {
            // ��������� ����������� � ��������� �����
            Vector3 direction = (_points[CurrentPointIndex].position - transform.position).normalized;
            // ������������ ������, ����� �� ������� � ������� ��������� �����
            transform.LookAt(_points[CurrentPointIndex]);
            // ������� ������ � ����������� ��������� ����� � ������ ��������
            transform.position += direction * speed * Time.deltaTime;
            MoveAnimation(true);
        }
        else
        {
            if (CurrentPointIndex == _points.Count - 1) // ���� ������� ������ ����� ���������� ������� ������
            {
                _isMovingForward = false; // ����� � �������� �������
            }
            else if (CurrentPointIndex == 0) // ���� ������� ������ ����� ������� ������� ������
            {
                _isMovingForward = true; // ������������� ��������
            }

           CurrentPointIndex += _isMovingForward ? 1 : -1;
        }
    }
}
