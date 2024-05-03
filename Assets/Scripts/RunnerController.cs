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

    // Метод для установки списка точек для бегуна
    public void SetPoints(List<Transform> points)
    {
        _points = points;
    }

    // Метод для включения/отключения анимации прыжка
    public void MoveAnimation(bool move)
    {
        animator.SetBool("Move", move);
    }

    // Метод присвоения материала
    public void SetColor(Material material)
    {
        mRenderer.material = material;
    }

    // Метод бега по кругу
    public void SoloSprint(float speed, float offset)
    {
        // Если индекс текущей точки больше или равен последнему элементу списка
        if (CurrentPointIndex >= _points.Count)
        {
            // Возвращаем индекс первого элемента
            CurrentPointIndex = 0;
        }

        if (Vector3.Distance(transform.position, _points[CurrentPointIndex].position) > offset)
        {
            // Вычисляем направление к следующей точке
            Vector3 direction = (_points[CurrentPointIndex].position - transform.position).normalized;
            // Поворачиваем объект, чтобы он смотрел в сторону следующей точки
            transform.LookAt(_points[CurrentPointIndex]);
            // Двигаем объект в направлении следующей точки с учетом скорости
            transform.position += direction * speed * Time.deltaTime;
            MoveAnimation(true);
        }
        else
        {
            // Если достигли текущей точки, переходим к следующей
            CurrentPointIndex++;
        }
    }

    // Метод движения челнока. Патруль.
    public void SoloShuttle(float speed, float offset)
    {
        if (Vector3.Distance(transform.position, _points[CurrentPointIndex].position) > offset)
        {
            // Вычисляем направление к следующей точке
            Vector3 direction = (_points[CurrentPointIndex].position - transform.position).normalized;
            // Поворачиваем объект, чтобы он смотрел в сторону следующей точки
            transform.LookAt(_points[CurrentPointIndex]);
            // Двигаем объект в направлении следующей точки с учетом скорости
            transform.position += direction * speed * Time.deltaTime;
            MoveAnimation(true);
        }
        else
        {
            if (CurrentPointIndex == _points.Count - 1) // Если текущий индекс равен последнему индексу списка
            {
                _isMovingForward = false; // Валим в обратную сторону
            }
            else if (CurrentPointIndex == 0) // Если текущий индекс равен первому индексу списка
            {
                _isMovingForward = true; // Разворачиваем движение
            }

           CurrentPointIndex += _isMovingForward ? 1 : -1;
        }
    }
}
