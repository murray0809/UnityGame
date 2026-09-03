using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel model;
    private EnemyView view;

    [SerializeField]
    private EnemyData enemyData;

    private WaypointController waypointController;

    private int currentWaypointIndex = 0;

    private void Awake()
    {
        view = GetComponent<EnemyView>();

        waypointController = FindFirstObjectByType<WaypointController>();
    }

    private void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyData is not assigned!");
            return;
        }

        model = new EnemyModel(
            enemyData.maxHp,
            enemyData.moveSpeed,
            enemyData.reward,
            enemyData.goalDamage
        );
    }

    private void Update()
    {
        MoveToWaypoint();
    }

    public void Initialize(EnemyData data)
    {
        enemyData = data;
    }

    private void MoveToWaypoint()
    {
        Transform targetWaypoint =
            waypointController.GetWaypoint(currentWaypointIndex);

        if (targetWaypoint == null)
        {
            return;
        }

        Vector3 direction =
            targetWaypoint.position - transform.position;

        transform.position +=
            direction.normalized *
            model.Speed *
            Time.deltaTime;

        if (Vector3.Distance(
                transform.position,
                targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypointController.WaypointCount)
            {
                ReachGoal();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        model.TakeDamage(damage);

        Debug.Log("Enemy HP: " + model.HP);

        if (model.IsDead())
        {
            Die();
        }
    }

    private void Die()
    {
        EnemySpawner enemySpawner =
            FindFirstObjectByType<EnemySpawner>();

        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyDefeated();
        }

        GameController gameController =
            FindFirstObjectByType<GameController>();

        if (gameController != null)
        {
            gameController.AddMoney(model.Reward);
        }

        Debug.Log("Enemy Defeated!");

        Destroy(gameObject);
    }

    private void ReachGoal()
    {
        GameController gameController =
            FindFirstObjectByType<GameController>();

        if (gameController != null)
        {
            gameController.DamageLife(model.GoalDamage);
        }

        EnemySpawner enemySpawner =
            FindFirstObjectByType<EnemySpawner>();

        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyDefeated();
        }

        Debug.Log("Goal!");

        Destroy(gameObject);
    }
}