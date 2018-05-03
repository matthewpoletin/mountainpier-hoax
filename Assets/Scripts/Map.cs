using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;

	public int width = 10;
	public int height = 10;

	// Is map created already
	private bool isInstanciated = false;

	private float xOffset = 0.900f;
	private float zOffset = 0.790f;

	// Use this for initialization
	private void Start () {
		for (int x = 1; x <= width; x++)
		{
			for (int y = 1; y <= height; y++)
			{
				float xPos = x * xOffset;
				if(y % 2 == 1)
				{
					xPos += xOffset/2;
				}
				GameObject hexGameObject = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);

				// Set gameobject's name
				hexGameObject.name = "Hex_" + x + "_" + y;

				// Organize hierarchy
				hexGameObject.transform.SetParent(this.transform);

				// 
				hexGameObject.GetComponent<Hex>().x = x;
				hexGameObject.GetComponent<Hex>().y = y;
			}
		}
		isInstanciated = true;
	}

	public GameObject GetHex(int x, int y)
	{
		if (x > this.width)
		{
			Debug.LogError("GetHex: Pos " + x + "outers width (" + this.width + ")");
			return null;
		}
		
		if (y > this.height)
		{
			Debug.LogError("GetHex: Pos " + y + "outers height (" + this.height + ")");
			return null;
		}

		if (!this.isInstanciated)
			return null;
		
		return GameObject.Find("Hex_" + x + "_" + y);
	}

}
