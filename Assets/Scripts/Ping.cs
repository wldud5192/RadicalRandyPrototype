using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject hackingPE;
    public GameObject ping;
    float coolDown;
    public bool canActivate = true;
    AudioSource activateSound;

    void Start()
    {
        activateSound = GetComponent<AudioSource>();
    }

   /* if reusable
       void Update ()
    {
        if(!canActivate)
        {
            coolDown += Time.deltaTime;
        }

        if (coolDown > 10f)
        {
            canActivate = true;
        }
        
    }

        also instantiate PING & Assign the prefab in the inspector
    */

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && ping.gameObject != null && canActivate)
        {
            coolDown = 0;
            canActivate = false;
            activateSound.Play();
            ping.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(Scale(10));

        } 
    }
    IEnumerator Scale(float time)
    {
        Vector3 originalScale = ping.transform.localScale;
        Vector3 destinationScale = new Vector3(500f, 0, 500f);
        float currentTime = 0.0f;
        do
        {
            ping.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

        ping.GetComponent<AudioSource>().Play();
        Destroy(ping.gameObject, ping.GetComponent<AudioSource>().clip.length);
        Destroy(hackingPE);
        

    }
}

=======
	public bool scanner;
	public GameObject ping;
	public bool count;

	void Start()
	{

	}

	void Update()
	{
		if (count)
		{
			for (int t = 0; t > 10; t++)
			{
				ping.gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Player" && ping.gameObject != null)
		{
			scanner = true;
			ping.GetComponent<MeshRenderer>().enabled = true;
			StartCoroutine(Scale(10));

		}
	}

	IEnumerator Scale(float time)
	{
		float currentTime = 0.0f;

		count = true;
		Vector3 originalScale = ping.transform.localScale;
		Vector3 destinationScale = new Vector3(0.50f, 0.50f);
	
		while (currentTime <= time)
		{
			ping.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
			currentTime += Time.deltaTime;
			yield return null;
		}
	}
}
>>>>>>> 266e1ac9bed8717781d10347fcf2fe423eabe0b0
