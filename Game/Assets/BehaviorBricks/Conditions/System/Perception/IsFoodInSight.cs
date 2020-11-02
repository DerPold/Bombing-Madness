using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
    [Condition("Perception/IsFoodInSight")]
    [Help("Finds the closest game object Food with a given tag")]
    public class IsFoodInSight : GOCondition
    {
        [InParam("tag")]
        [Help("Tag of the target game object")]
        public string tag;
       
        private GameObject foundGameObject;

        [InParam("closeDistance")]
        [Help("The maximun distance to consider that the target is close")]
        public float closeDistance;

        private float elapsedTime;

        public override bool Check()
        {
            float dist = float.MaxValue;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
            {
                float newdist = (go.transform.position + gameObject.transform.position).sqrMagnitude;
                if (newdist < dist)
                {
                    dist = newdist;
                    foundGameObject = go;
                    Debug.Log(foundGameObject);
                }
            }
            return (gameObject.transform.position - foundGameObject.transform.position).sqrMagnitude < closeDistance * closeDistance;
        }
    }
}