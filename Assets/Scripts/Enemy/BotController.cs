using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BotController : BotInterface
{
    private void Update()
    {
        float dis = Vector3.Distance(target.position, transform.position);

        //update for new project
        if (target == null)
            return;
        if (dis <= distance)
        {
            RotateToTarget();
            EnemyAttack();
        }
        else if (dis > distance)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPotrulingPoint();
        } 
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
