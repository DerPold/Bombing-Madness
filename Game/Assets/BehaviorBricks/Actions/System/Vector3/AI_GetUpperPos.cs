using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using Pathfinding;

namespace BBUnity.Actions
{

    [Action("Vector3/AI_GetUpperPos")]
    [Help("Gets a upper position from a given area")]
    public class AI_GetUpperPos : GOAction
    {
        [InParam("Look Radius")]
        [Help("The Radius to pick a random point")]
        public float lookRadius;

        private Vector3 randomPoint;
        private IAstarAI ai;

        public override void OnStart()
        {
            ai = gameObject.GetComponent<IAstarAI>();          
        }

        public override TaskStatus OnUpdate()
        {
            if (!ai.pathPending && (!ai.hasPath || ai.reachedEndOfPath))
            {
                ai = gameObject.GetComponent<IAstarAI>();
                ai.destination = randomPoint;
                ai.SearchPath();
            }
            return TaskStatus.COMPLETED;
        }
        Vector3 PickRandomPoint()
        {
            var point = Random.insideUnitSphere * lookRadius;

            point.y = 0;
            point += ai.position;
            return point;
        }
    }
}