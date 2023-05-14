using UnityEngine;
using UnityEngine.AI;

public class BotInterface : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform target = null;

    [SerializeField] public float distance = 20f;
    [SerializeField] public float distanceForAttake = 20f;
    [SerializeField] public float distanceForFastAttake = 3f;

    [SerializeField] private RaycastHit raycastHit;

    [SerializeField] public Transform[] pointsForPotruling;
    [SerializeField] private int destPoinForPotrulingt = 0;


    [SerializeField] public Transform[] pointsForHide;

    public float rotationSpeed = 0.2f;

    private float spawnRate = 2f;
    float nextSpawn = 1.5f;

    public bool IsRunAway;

    public float HP;
    private Animation _anim;

    private void Start()
    {
        _anim = GetComponent<Animation>();
        if (agent == null)
            if (!TryGetComponent(out agent))
                print(name + " needs a navmesh agent!");

        GotoNextPotrulingPoint();
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public bool IsViewTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Player")
                return true;
        }
        return false;
    }

    public void EnemyWalk(Vector3 pos)
    {
        agent.Resume();
        agent.SetDestination(pos);
    }

    public void RotateToTarget()
    {
        _anim.Play("Idle Aiming");
        var targetRotation = Quaternion.LookRotation(new Vector3(target.position.x, target.position.y + 1.5f, target.position.z) - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
    }

    public bool IsPointsForPotrulingExist()
    {
        if (pointsForPotruling.Length == 0)
            return false;
        return true;
    }
    public void GotoNextPotrulingPoint()
    {
        if (IsPointsForPotrulingExist())
        {
            _anim.Play("Rifle Walk");
            EnemyWalk(pointsForPotruling[destPoinForPotrulingt].position);
            destPoinForPotrulingt = (destPoinForPotrulingt + 1) % pointsForPotruling.Length;
        }
    }

    public bool IsPointsForHideExist()
    {
        if (pointsForPotruling.Length == 0)
            return false;
        return true;
    }
    //public void GotoNextHidePoint()
    //{
    //    if (IsPointsForPotrulingExist())
    //    {
    //        EnemyWalk(pointsForPotruling[destPoinForPotrulingt].position);
    //        destPoinForPotrulingt = (destPoinForPotrulingt + 1) % pointsForPotruling.Length;
    //    }
    //}

    public void EnemyRunToNextHidePoint()
    {
        IsRunAway = true;
        //run away from player
        //Vector3 pos = new Vector3(Random.Range(-transform.position.x -10f, transform.position.x + 10f), 0, Random.Range(-transform.position.z + 10f, transform.position.z + 10f));
        //EnemyWalk(pos);
        if (IsPointsForHideExist())
        {
            int k = 0;
            float MinDistanceBetweenEnemyAndPoint = Vector3.Distance(transform.position, pointsForHide[0].position);
            for (int i = 1; i < pointsForHide.Length; i++)
            {
                float distanceBetweenEnemyAndPoint = Vector3.Distance(transform.position, pointsForHide[i].position);
                if (distanceBetweenEnemyAndPoint < MinDistanceBetweenEnemyAndPoint && CalculateDistanceBetweenPlayerAndPoints(i) > distanceForAttake)
                {
                    MinDistanceBetweenEnemyAndPoint = distanceBetweenEnemyAndPoint;
                    k = i;
                }
            }
            _anim.Play("Sprint Forward");
            EnemyWalk(pointsForHide[k].position);
        }
    }

    public float CalculateDistanceBetweenPlayerAndPoints(int i)
    {
        return (Vector3.Distance(transform.position, pointsForHide[i].position));
    }


    public void ResetIsRunAway()
    {
        IsRunAway = false;
    }

    public void EnemyAttack()
    {
        agent.Stop(); 
        // method when Enemy ready give damage to Player with reload attake
        if (Time.time > nextSpawn)
        {
            _anim.Play("Firing Rifle");
            nextSpawn = Time.time + spawnRate;
        }
    }

    public void EnemyDie()
    {
        Destroy(gameObject, 60);
        _anim.Play("Death From The Front");
        agent.enabled = false;
        GetComponent<BotController>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
    public void EnemyTakeDamage(float damage)
    {
        HP-= damage;
        EnemyRunToNextHidePoint();
        if(HP<=0)
            EnemyDie();
    }
}
