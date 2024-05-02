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
            // Вычисляем направление к следующей точке
            Vector3 direction = (points[_currentPoint].position - runners[0].transform.position).normalized;
            // Поворачиваем объект, чтобы он смотрел в сторону следующей точки
            runners[0].transform.LookAt(points[_currentPoint]);
            // Двигаем объект в направлении следующей точки с учетом скорости
            runners[0].transform.position += direction * speed * Time.deltaTime;
            runners[0].MoveAnimation(true);
        }
        else
        {
            // Если достигли текущей точки, переходим к следующей
            _currentPoint++;
        }
    }

    private void SoloShuttle()
    {
        if (Vector3.Distance(runners[0].transform.position, points[_currentPoint].position) > offset)
        {
            // Вычисляем направление к следующей точке
            Vector3 direction = (points[_currentPoint].position - runners[0].transform.position).normalized;
            // Поворачиваем объект, чтобы он смотрел в сторону следующей точки
            runners[0].transform.LookAt(points[_currentPoint]);
            // Двигаем объект в направлении следующей точки с учетом скорости
            runners[0].transform.position += direction * speed * Time.deltaTime;
            runners[0].MoveAnimation(true);
        }
        else
        {
            // Если достигли последней точки, меняем направление движения на обратное
            if (_currentPoint == points.Count - 1)
            {
                _isMovingForward = false;
            }
            // Если достигли первой точки, меняем направление движения на прямое
            else if (_currentPoint == 0)
            {
                _isMovingForward = true;
            }

            // Увеличиваем или уменьшаем текущий индекс в зависимости от направления движения
            _currentPoint += _isMovingForward ? 1 : -1;
        }
    }

    private void MedleyRelay()
    {
        // Проверяем, достиг ли текущий бегун точки следующего бегуна
        if (Vector3.Distance(runners[currentRunnerIndex].transform.position, points[(currentRunnerIndex + 1) % points.Count].position) > offset)
        {
            // Если нет, двигаем текущего бегуна к этой точке
            MoveRunner(currentRunnerIndex, (currentRunnerIndex + 1) % points.Count);
        }
        else
        {
            // Если текущий бегун достиг точки следующего бегуна
            // Переключаем анимацию текущего бегуна на остановку
            runners[currentRunnerIndex].MoveAnimation(false);

            // Переходим к следующему бегуну в списке
            currentRunnerIndex = (currentRunnerIndex + 1) % points.Count;

            // Запускаем движение текущего бегуна к следующей точке
            MoveRunner(currentRunnerIndex, (currentRunnerIndex + 1) % points.Count);
        }
    }

    private void MoveRunner(int current, int next)
    {
        // Вычисляем направление к следующей точке
        Vector3 direction = (points[next].position - runners[current].transform.position).normalized;
        // Поворачиваем объект, чтобы он смотрел в сторону следующей точки
        runners[current].transform.LookAt(points[next]);
        // Двигаем объект в направлении следующей точки с учетом скорости
        runners[current].transform.position += direction * speed * Time.deltaTime;
        runners[current].MoveAnimation(true);
    }
}


