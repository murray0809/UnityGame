using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private ProjectileModel model;

    private EnemyController target;

    private bool isMagic;

    [SerializeField]
    private float magicAttackRange = 1.5f;

    [SerializeField]
    private GameObject magicExplosionPrefab;

    public void Initialize(
        EnemyController target,
        int damage,
        bool isMagic)
    {
        this.target = target;
        this.isMagic = isMagic;

        model = new ProjectileModel(
            8f,
            damage
        );
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction =
            target.transform.position - transform.position;

        transform.position +=
            direction.normalized *
            model.MoveSpeed *
            Time.deltaTime;

        if (Vector3.Distance(
                transform.position,
                target.transform.position) < 0.1f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }


        if (isMagic)
        {

            if (magicExplosionPrefab != null)
            {
                GameObject effect =
                    Instantiate(
                        magicExplosionPrefab,
                        target.transform.position,
                        Quaternion.identity
                    );

                MagicExplosionEffect explosion =
                    effect.GetComponent<MagicExplosionEffect>();

                if (explosion != null)
                {
                    explosion.Initialize(magicAttackRange);
                }
            }

            EnemyController[] enemies =
                FindObjectsByType<EnemyController>(
                    FindObjectsSortMode.None
                );

            foreach (EnemyController enemy in enemies)
            {
                if (enemy == null)
                {
                    continue;
                }

                float distance =
                    Vector3.Distance(
                        target.transform.position,
                        enemy.transform.position
                    );

                if (distance <= magicAttackRange)
                {
                    enemy.TakeDamage(model.Damage);
                }
            }
        }
        else
        {
            target.TakeDamage(model.Damage);
        }

        Destroy(gameObject);
    }
}