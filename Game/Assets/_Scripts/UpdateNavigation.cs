using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateNavigation : MonoBehaviour {
    public float distanceToTarget = 20f;

    private RaycastHit getObstacle;
    private Bounds obstacleColliders;
    private CharacterController controller;
	// Use this for initialization
	void Start () {
        controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

           
    }
}
