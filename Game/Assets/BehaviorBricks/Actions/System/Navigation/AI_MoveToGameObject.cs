using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using Pathfinding;

namespace BBUnity.Actions
{
    [Action("Navigation/AI_MoveToGameObject")]
    [Help("Moves the game object to a given position by using a NavMeshAgent")]
    public class AI_MoveToGameObject : GOAction
    {
        [InParam("target")]
        [Help("Target position where the game object will be moved")]
        public GameObject target;

        private Transform targetTransform;
        private IAstarAI ai;
        // Use this for initialization

        public override void OnStart()
        {
            if (target != null)
            {
                targetTransform = target.transform;
                ai = gameObject.GetComponent<IAstarAI>();
                ai.destination = target.transform.position;
                ai.SearchPath();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (target == null)
                return TaskStatus.FAILED;
            if (!ai.pathPending && (!ai.hasPath || ai.reachedEndOfPath))
                return TaskStatus.COMPLETED;
            else if (ai.destination != targetTransform.position)
            {
                ai.destination = targetTransform.position;
                ai.SearchPath();
            }
            return TaskStatus.RUNNING;
        }

        public override void OnAbort()
        {
#if UNITY_5_6_OR_NEWER
            if (ai != null)
                ai.isStopped = true;
#else
            if (ai != null)
                ai.Stop();
#endif
        }
    }
}

