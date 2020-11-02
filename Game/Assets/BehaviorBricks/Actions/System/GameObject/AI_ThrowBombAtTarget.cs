using System;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/AI_ThrowBombAtTarget")]
    [Help("Instantiates a bomb")]
    public class AI_ThrowBombAtTarget : GOAction
    {
        [InParam("Cooldown",DefaultValue = 100)]
        [Help("Time to wait for throwing the next Bomb")]
        public int Cooldown;

        [InParam("Contact Bomb")]
        [Help("m24 Contact")]
        public GameObject bomb;

        [InParam("Big Bomb")]
        [Help("m24 Double Explosion")]
        public GameObject bomb2;

        [InParam("Throwforce")]
        [Help("Force of thrown Bomb")]
        public float throwForce;

        [InParam("Target")]
        [Help("target to throw at")]
        public GameObject target;

        [InParam("Throw Radius Player")]
        [Help("Radius of thrown Bomb against Player")]
        public float bombThrowRadiusPlayer;

        [InParam("Throw Radius Tree")]
        [Help("Radius of thrown Bomb against Tree")]
        public float bombThrowRadiusTree;

        [InParam("Throw Radius Magic Cube")]
        [Help("Radius of thrown Bomb against Magic Cube")]
        public float bombThrowRadiusMagicCube;

        private int elapsed = 0;

        public override TaskStatus OnUpdate()
        {
            if(Cooldown > 0)
            {
                elapsed += 1;
                elapsed %= Cooldown;
                if(elapsed != 0)
                {
                    return TaskStatus.RUNNING;
                }
            }
            float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
            float throwDeeper = 0.0f;
            if (distance <= 2f)
            {
                throwDeeper = -0.2f;
            }
            if (distance <= bombThrowRadiusPlayer && target.gameObject.CompareTag("Player"))
            {
                GameObject bombToThrow;
                bombToThrow = bomb;             
                ThrowBomb(bombToThrow, 0.2f + throwDeeper);
            }
            if (distance <= bombThrowRadiusTree && target.gameObject.CompareTag("Tree"))
            {
                GameObject bombToThrow;
                bombToThrow = bomb;
                ThrowBomb(bombToThrow, 0.0f + throwDeeper);
            }
            if (distance <= bombThrowRadiusMagicCube && target.gameObject.CompareTag("MagicCube"))
            {
                GameObject bombToThrow;
                bombToThrow = bomb;
                ThrowBomb(bombToThrow, 0.0f + throwDeeper);
            }
            return TaskStatus.COMPLETED;
        }



        public void ThrowBomb(GameObject grenadeToThrow, float heightThrowDirection)
        {

            Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
            direction.y += heightThrowDirection;
            GameObject bomb = GameObject.Instantiate(grenadeToThrow, gameObject.transform.position + new Vector3(0, 2, 0), gameObject.transform.rotation);
            GrenadeExploding grenadeExploding = bomb.GetComponent<GrenadeExploding>();
            if (grenadeExploding == null) Debug.Log("Fehler!");
            grenadeExploding.throwTag = "Enemy";
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(direction * throwForce, ForceMode.VelocityChange);           
        }
    }
}