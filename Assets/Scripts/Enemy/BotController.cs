using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BotController : BotInterface
{
    private Animation _anim;

    private void Start()
    {
        _anim = GetComponent<Animation>();
    }
    private void Update()
    {
        float dis = Vector3.Distance(target.position, transform.position);
        //update for new project
        if (target == null)
            return;
        if (dis <= distance && dis > distanceForAttake)
        {
            EnemyWalk(target.position);
            _anim.Play("Sprint Forward");
        }
        else if (dis > distance)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                _anim.Play("Rifle Walk");
                GotoNextPoint();
            }
                
        }
        else if (dis <= distanceForFastAttake)
        {
            RotateToTarget();
            EnemyAttack();
            _anim.Play("Firing Rifle");
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
