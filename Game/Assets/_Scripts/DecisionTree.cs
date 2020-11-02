using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour {


    #region Singleton

    public static DecisionTree instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public float prioPlayer;
    public float prioPowerup;
    public float prioFood;
    public float prioTree;
    public float prioMagicCube;
    public float prioMiddle;
    public GameObject pointMiddle;
    public GameObject pointMiddle2;
    public GameObject pointMiddle3;
    public GameObject water;

    private GameObject player;
    private ArrayList arrayListTree;
    private ArrayList arrayListCube;
    private ArrayList arrayListCollectables;

	// Use this for initialization
	public void loadVariables ()
    {
		GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        arrayListTree = new ArrayList(trees);
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("MagicCube");
        arrayListCube = new ArrayList(cubes);
        player = PlayerManager.instance.player;
        reloadCollectables();
    }

    public void reloadCollectables()
    {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        arrayListCollectables = new ArrayList(collectables);
    }

    private bool IsDestroyed(GameObject gameObject)
    {
        // UnityEngine overloads the == opeator for the GameObject type
        // and returns null when the object has been destroyed, but 
        // actually the object is still there but has not been cleaned up yet
        // if we test both we can determine if the object has been destroyed.
        return gameObject == null && !ReferenceEquals(gameObject, null);
    }

    public GameObject getNearestGameObject(GameObject ai)
    {
        float distanceMin = 10000f;
        GameObject current = null;
        float currentDistance;
        foreach(GameObject o in arrayListTree)
        {
            if(!IsDestroyed(o))
            {
                currentDistance = Vector3.Distance(ai.transform.position, o.transform.position);
                if (currentDistance < distanceMin)
                {
                    distanceMin = currentDistance;
                    current = o;
                }
            }
        }
        foreach (GameObject o in arrayListCube)
        {
            if(!IsDestroyed(o))
            {
                currentDistance = Vector3.Distance(ai.transform.position, o.transform.position);
                if (currentDistance < distanceMin)
                {
                    distanceMin = currentDistance;
                    current = o;
                }
            }
        }
        foreach (GameObject o in arrayListCollectables)
        {
            if (!IsDestroyed(o))
            {
                currentDistance = Vector3.Distance(ai.transform.position, o.transform.position);
                if(currentDistance < distanceMin)
                {
                    distanceMin = currentDistance;
                    current = o;
                }
            }
        }
        currentDistance = Vector3.Distance(ai.transform.position, player.transform.position);
        if(currentDistance < distanceMin)
        {
            distanceMin = currentDistance;
            current = player;
        }
        return current;
    }

    public GameObject getGameObjectWithHighestPrio(GameObject ai)
    {
        ArrayList prios = new ArrayList();
        Dictionary<float, GameObject> dict = new Dictionary<float, GameObject>();
        float currentDistance;
        foreach (GameObject o in arrayListTree)
        {
            if (!IsDestroyed(o))
            {
                currentDistance = Vector3.Distance(ai.transform.position, o.transform.position);
                currentDistance *= prioTree;
                currentDistance *= getPrioFromWater(o);
                prios.Add(currentDistance);
                dict.Add(currentDistance, o);
            }
        }
        foreach (GameObject o in arrayListCube)
        {
            if (!IsDestroyed(o))
            {
                currentDistance = Vector3.Distance(ai.transform.position, o.transform.position);
                currentDistance *= prioMagicCube;
                currentDistance *= getPrioFromWater(o);
                prios.Add(currentDistance);
                dict.Add(currentDistance, o);
            }
        }
        foreach (GameObject o in arrayListCollectables)
        {
            if (!IsDestroyed(o))
            {
                currentDistance = Vector3.Distance(ai.transform.position, o.transform.position);
                if (LayerMask.LayerToName(o.layer) == "Collectible") currentDistance *= prioFood;
                else if (LayerMask.LayerToName(o.layer) == "PowerUp") currentDistance *= prioPowerup;
                else Debug.Log("Layer comparion not working!");
                currentDistance *= getPrioFromWater(o);
                prios.Add(currentDistance);
                dict.Add(currentDistance, o);
            }
        }

        currentDistance = Vector3.Distance(ai.transform.position, player.transform.position);
        currentDistance *= prioPlayer;
        currentDistance *= getPrioFromWater(player);
        prios.Add(currentDistance);
        dict.Add(currentDistance, player);

        if(!IsDestroyed(pointMiddle))
        {
            currentDistance = Vector3.Distance(ai.transform.position, pointMiddle.transform.position);
            currentDistance *= prioMiddle;
            currentDistance *= getPrioFromWater(pointMiddle);
            prios.Add(currentDistance);
            dict.Add(currentDistance, pointMiddle);
        }

        if (!IsDestroyed(pointMiddle2))
        {
            currentDistance = Vector3.Distance(ai.transform.position, pointMiddle2.transform.position);
            currentDistance *= prioMiddle;
            currentDistance *= getPrioFromWater(pointMiddle2);
            prios.Add(currentDistance);
            dict.Add(currentDistance, pointMiddle2);
        }

        if (!IsDestroyed(pointMiddle3))
        {
            currentDistance = Vector3.Distance(ai.transform.position, pointMiddle3.transform.position);
            currentDistance *= prioMiddle;
            currentDistance *= getPrioFromWater(pointMiddle3);
            prios.Add(currentDistance);
            dict.Add(currentDistance, pointMiddle3);
        }

        prios.Sort();
        GameObject best = null;
        dict.TryGetValue((float) prios[0], out best);
        if (best.CompareTag("Collectable") || best.CompareTag("Player")) return best;
        else
        {
            int z = Random.Range(0, 3);
            //Debug.Log("Priorität für Objekt: " + prios[z]);
            dict.TryGetValue((float)prios[z], out best);
        }
        if (best == null) Debug.Log("dict not working!!");
        if (IsDestroyed(best)) Debug.Log("gameObject is already destroyed!");
        return best;
    }

    private float getPrioFromWater(GameObject o)
    {
        if (water.transform.position.y >= o.transform.position.y + 4f)
        {
            return 150.0f;
        }
        else if (water.transform.position.y >= o.transform.position.y + 3f)
        {
            return 100.0f;
        }
        else if (water.transform.position.y >= o.transform.position.y + 2f)
        {
            return 50.0f;
        }
        else if (water.transform.position.y >= o.transform.position.y + 1f)
        {
            return 15.5f;
        }
        else if (water.transform.position.y >= o.transform.position.y + 0.5f)
        {
            return 7.0f;
        }
        else if (water.transform.position.y >= o.transform.position.y)
        {
            return 4.0f;
        }
        else if (water.transform.position.y < o.transform.position.y)
        {
            return 1.0f;
        }
        return 50.0f;
    }

    public float distanceToEdgeOfWorld()
    {
        return 0.0f;
    }
}
