using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Idle,
    SoloSprint,
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
    private int currentRunnerIndex = 0;

    private void Start()
    {
        runners = new List<RunnerController>();
    }

    public void ChangeMovementType(int type)
    {
        moveType = (MovementType)type;

        for (int i = 0; i < runners.Count; i++) Destroy(runners[i].gameObject);

        runners.Clear();

        if (type > 2) SpawnRunner(4);
        else SpawnRunner(1);
    }

    private void Update()
    {
        if (moveType == MovementType.SoloSprint) runners[0].SoloSprint(speed, offset);
        else if (moveType == MovementType.SoloShuttle) runners[0].SoloShuttle(speed, offset);
        else if (moveType == MovementType.MedleyRelay) MedleyRelay();
        else return;
    }

    private Material SetRandomColor(List<Material> currentMaterials)
    {
        int random = Random.Range(0, currentMaterials.Count);
        Material material = currentMaterials[random];
        currentMaterials.Remove(material);
        return material;
    }

    private void SpawnRunner(int count)
    {
        List<Material> currentMaterials = new List<Material>();

        for (int i = 0; i < materials.Count; i++)
        {
            currentMaterials.Add(materials[i]);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(runner.gameObject, points[i].position, Quaternion.identity);
            go.transform.SetParent(transform);
            RunnerController runnerController = go.GetComponent<RunnerController>();
            runnerController.SetPoints(points); // ѕередаем список точек бегуну
            runnerController.CurrentPointIndex = i;
            runners.Add(runnerController);
            runners[i].SetColor(SetRandomColor(currentMaterials));
            runners[i].MoveAnimation(false);
        }
    }

    private void MedleyRelay()
    {
        // ѕровер€ем, достиг ли текущий бегун следующей точки из своего списка
        if (Vector3.Distance(runners[currentRunnerIndex].transform.position, 
            points[runners[currentRunnerIndex].CurrentPointIndex % points.Count].position) > offset)
        {
            // ≈сли нет, двигаем текущего бегуна к этой точке
            runners[currentRunnerIndex].SoloSprint(speed,offset);
        }
        else
        {
            // ≈сли текущий бегун достиг точки точки из своего списка
            // ѕереключаем анимацию текущего бегуна на остановку
            runners[currentRunnerIndex].MoveAnimation(false);

            // ѕереходим к следующему бегуну в списке
            currentRunnerIndex = (currentRunnerIndex + 1) % runners.Count;

            // «апускаем движение текущего бегуна к следующей точке
            runners[currentRunnerIndex].SoloSprint(speed, offset);
        }
    }
}


