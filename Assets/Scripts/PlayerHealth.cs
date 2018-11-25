using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	public int maxLife = 5;
	public int curLife;

	GameObject gameOverUI;
	public List<Image> lifeImages;
	GameObject[] lifeUI;
	public Sprite fullLife;
	public Sprite emptyLife;

	void Start()
	{
		gameOverUI = GameObject.Find("GameOverUI");
		lifeUI = GameObject.FindGameObjectsWithTag("LifeUI");

		if(maxLife != 0)
		for (int i = 0; i < maxLife; i++)
		{
			if(lifeImages != null && lifeUI != null && i <= lifeImages.Count && i <= lifeUI.Length )
			{
				if(lifeUI[i] != null)
					lifeImages[i] = lifeUI[i].GetComponent<Image>();
			}
		}

		curLife = maxLife;
	}

	void Update()
	{
		if (maxLife < curLife)
		{
			maxLife = curLife;
		}

		for (int i = 0; i < lifeImages.Count; i++)
		{
			if (i < maxLife)
			{
				lifeImages[i].sprite = fullLife;
			}
			else
			{
				lifeImages[i].sprite = emptyLife;
			}
			if (i < curLife)
			{
				lifeImages[i].enabled = true;
			}
			else
			{
				lifeImages[i].enabled = false;
			}
		}
	}

	void CheckLifeAmount()
	{
		for (int i = 0; i < maxLife; i++)
		{
			if (curLife <= i)
			{
				lifeImages[i].enabled = false;
			}
			else
			{
				lifeImages[i].enabled = true;
			}

		}
	}

	void OnPlayerKilled()
	{
		if (curLife - 1 > 0)
		{
			curLife -= 1;
		}
		else
		{
			gameOverUI.gameObject.SetActive(true);
		}
	}

}
