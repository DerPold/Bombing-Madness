using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using Pathfinding;

namespace BBUnity.Actions
{
    [Action("Navigation/AI_MoveUp")]
    [Help("Moves the game object to a given upper position by using a Astar")]

    public class AI_MoveUp : GOAction
    {
        [InParam("target")]
        [Help("Target position where the game object will be moved")]
        public Vector3 target;

        private IAstarAI ai;
        // Use this for initialization
        public override void OnStart()
        {
            ai = gameObject.GetComponent<IAstarAI>();
            ai.destination = target;
            ai.SearchPath();
        }

        // Update is called once per frame

        public override TaskStatus OnUpdate()
        {
            if (!ai.pathPending && (!ai.hasPath || ai.reachedEndOfPath))
                return TaskStatus.COMPLETED;

            return TaskStatus.RUNNING;
        }
    }
}
