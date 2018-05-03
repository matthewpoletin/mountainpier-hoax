using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

	// Coordinates in array
	public int x;
	public int y;

	public GameObject[] GetNeigobors()
	{
		Map map = GameObject.Find("map").GetComponent<Map>();

		GameObject[] hexes = new GameObject[6];

		GameObject left = map.GetHex(x - 1, y);
		GameObject right = map.GetHex(x + 1, y);

		GameObject topLeft = map.GetHex(x, y + 1);
		GameObject topRight = map.GetHex(x + 1, y + 1);
		GameObject botLeft = map.GetHex(x, y - 1);
		GameObject botRight = map.GetHex(x + 1, y - 1);

		hexes[1] = left;
		hexes[2] = right;
		hexes[3] = topLeft;
		hexes[4] = topRight;
		hexes[5] = botLeft;
		hexes[6] = botRight;

		return hexes;
	}

}
