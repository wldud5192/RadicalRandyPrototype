using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
public class DestroyableTile : MonoBehaviour
{
    bool alreadyActivated = false;
    public Vector3 boxScale;
	BoxCollider overlapBox;
	public List<Collider> enemyList;
	public Collider[] overlappedObjects;

	public float scanDistance;
    AudioSource crackSound;
    ExitScript exit;

	[Tooltip("Hides the ball scaled by Scan Distance")]
	public bool disableGizmos;

    private void OnValidate()
    {
        gameObject.GetComponents<BoxCollider>()[1].size = new Vector3(2.1f, 2.0f, 2.1f);
    }

    void Start()
    {

        Component[] destroyOnLoad = GetComponents(this.GetType());
        if (destroyOnLoad.Length > 1)
        {
            Destroy(destroyOnLoad[0]);

        }

        enemyList = new List<Collider>();
		overlappedObjects = new Collider[8];
        crackSound = GetComponent<AudioSource>();
        exit = GameObject.FindGameObjectWithTag("Exit").GetComponent<ExitScript>();
        gameObject.GetComponents<BoxCollider>()[1].size = new Vector3(2.1f, 2.0f, 2.1f);


    }


	private void OnTriggerEnter(Collider other)
	{
        //ScanForNearestEnemy();
       

        if (other.gameObject.tag == "Player" && !alreadyActivated)
        {
            crackSound.Play();
            //GetComponent<Animator>.SetBool("CrackAnim", true)
            exit.currentNum -= 1;
            MeshRenderer cpu = GetComponentInChildren<MeshRenderer>();
            cpu.enabled = false;

            alreadyActivated = true;
        }
	}

    /* 
	private void OnDrawGizmos()
	{
		if (!disableGizmos)
		{
			Gizmos.color = new Color(255, 0, 0, 128);
			Gizmos.DrawSphere(transform.position, scanDistance);
		}
	}

	[ContextMenu("Scan Enemies")]
	void ScanForNearestEnemy()
	{
		Debug.Log("Scanning for Enemies...");
		Physics.OverlapSphereNonAlloc(transform.position, scanDistance, overlappedObjects);

		if (overlappedObjects == null)
		{
			Debug.LogError("Overlap hit NOTHING!!!!");
			return;
		}

		foreach (Collider overlappedObj in overlappedObjects)
		{
			if (overlappedObj == null)
				continue;

			if (overlappedObj.name.ToLower().Contains("enemy") || overlappedObj.CompareTag("Enemy") //|| overlappedObj.GetComponent<AIMasterScript>() != null// || !overlappedObj.name.ToLower().Contains("tile"))
			{
				enemyList.Add(overlappedObj);
			}
		}

		if (enemyList.Count > 0)
		{
			Collider closestEnemy = enemyList[0];
			float distanceFromEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position);

			for (int i = 0; i < enemyList.Count; i++)
			{
				float tempDistance = Vector3.Distance(transform.position, enemyList[i].transform.position);

				Debug.Log( enemyList[i].name + Vector3.Distance(transform.position, enemyList[i].transform.position));

				if (tempDistance < distanceFromEnemy)
				{
					distanceFromEnemy = tempDistance;
					closestEnemy = enemyList[i];
					Debug.Log("Closest enemy is " + closestEnemy.name + " at " + distanceFromEnemy);
				}
			}
		}
	} */


    
}
