using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Idle,
    SoloSprin,
    SoloShuttle,
    MedleyRelay
}

public class GameController : MonoBehaviour
{
    [SerializeField] private MovementType moveType;
    [SerializeField] private RunnerController runner;
    [SerializeField] private List<Transform> points;
    [SerializeField] private List<Material> materials;
    [SerializeField] private float speed, offset;

    private List<RunnerController> runners;
    private int _currentPoint = 0;
    private int currentRunnerIndex = 0;
    private bool _isMovingForward;

    private void Start()
    {
        runners = new List<RunnerController>();
    }

    public void ChangeMovementType(int type)
    {
        moveType = (MovementType)type;

        _currentPoint = 0;

        for (int i = 0; i < runners.Count; i++) Destroy(runners[i].gameObject);
        runners.Clear();

        if (type > 2) SpawnRunner(4);
        else SpawnRunner(1);
    }

    private void Update()
    {
        if (moveType == MovementType.SoloSprin) SoloSprin();
        else if (moveType == MovementType.SoloShuttle) SoloShuttle();
        else if (moveType == MovementType.MedleyRelay) MedleyRelay();
        else return;
    }

    private Material SetRandomColor()
    {
        int random = Random.Range(0, materials.Count);
        return materials[random];
    }

    private void SpawnRunner(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(runner.gameObject, points[i].position, Quaternion.identity);
            go.transform.SetParent(transform);
            runners.Add(go.GetComponent<RunnerController>());
            runners[i].SetColor(SetRandomColor());
            runners[i].MoveAnimation(false);
        }
    }

    private void SoloSprin()
    {
        if (_currentPoint >= points.Count)
        {
            _currentPoint = 0;
        }

        if (Vector3.Distance(runners[0].transform.position, points[_currentPoint].position) > offset)
        {
            // ��������� ����������� � ��������� �����
            Vector3 direction = (points[_currentPoint].position - runners[0].transform.position).normalized;
            // ������������ ������, ����� �� ������� � ������� ��������� �����
            runners[0].transform.LookAt(points[_currentPoint]);
            // ������� ������ � ����������� ��������� ����� � ������ ��������
            runners[0].transform.position += direction * speed * Time.deltaTime;
            runners[0].MoveAnimation(true);
        }
        else
        {
            // ���� �������� ������� �����, ��������� � ���������
            _currentPoint++;
        }
    }

    private void SoloShuttle()
    {
        if (Vector3.Distance(runners[0].transform.position, points[_currentPoint].position) > offset)
        {
            // ��������� ����������� � ��������� �����
            Vector3 direction = (points[_currentPoint].position - runners[0].transform.position).normalized;
            // ������������ ������, ����� �� ������� � ������� ��������� �����
            runners[0].transform.LookAt(points[_currentPoint]);
            // ������� ������ � ����������� ��������� ����� � ������ ��������
            runners[0].transform.position += direction * speed * Time.deltaTime;
            runners[0].MoveAnimation(true);
        }
        else
        {
            // ���� �������� ��������� �����, ������ ����������� �������� �� ��������
            if (_currentPoint == points.Count - 1)
            {
                _isMovingForward = false;
            }
            // ���� �������� ������ �����, ������ ����������� �������� �� ������
            else if (_currentPoint == 0)
            {
                _isMovingForward = true;
            }

            // ����������� ��� ��������� ������� ������ � ����������� �� ����������� ��������
            _currentPoint += _isMovingForward ? 1 : -1;
        }
    }

    private void MedleyRelay()
    {
        // ���������, ������ �� ������� ����� ����� ���������� ������
        if (Vector3.Distance(runners[currentRunnerIndex].transform.position, points[(currentRunnerIndex + 1) % points.Count].position) > offset)
        {
            // ���� ���, ������� �������� ������ � ���� �����
            MoveRunner(currentRunnerIndex, (currentRunnerIndex + 1) % points.Count);
        }
        else
        {
            // ���� ������� ����� ������ ����� ���������� ������
            // ����������� �������� �������� ������ �� ���������
            runners[currentRunnerIndex].MoveAnimation(false);

            // ��������� � ���������� ������ � ������
            currentRunnerIndex = (currentRunnerIndex + 1) % points.Count;

            // ��������� �������� �������� ������ � ��������� �����
            MoveRunner(currentRunnerIndex, (currentRunnerIndex + 1) % points.Count);
        }
    }

    private void MoveRunner(int current, int next)
    {
        // ��������� ����������� � ��������� �����
        Vector3 direction = (points[next].position - runners[current].transform.position).normalized;
        // ������������ ������, ����� �� ������� � ������� ��������� �����
        runners[current].transform.LookAt(points[next]);
        // ������� ������ � ����������� ��������� ����� � ������ ��������
        runners[current].transform.position += direction * speed * Time.deltaTime;
        runners[current].MoveAnimation(true);
    }
}


