using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

	// Prefab for unit
	public GameObject unitPrefab;

	// List of units awailiable
	public Dictionary<string, GameObject> units = new Dictionary<string, GameObject>();

	// Hardcoded coordinated of first unit
	public int x;
	public int y;
	public string username = "matthewpoletin";

	private bool toUpdate = true;

	void Start () {

	}

	void Update()
	{
		if (toUpdate)
		{
			GameObject map = GameObject.Find("Map");
			if (map != null)
			{
				GameObject hex = map.GetComponent<Map>().GetHex(x, y);

				if (hex != null)
				{
					float xPos = hex.transform.position.x;
					float yPos = hex.transform.position.y;
					float zPos = hex.transform.position.z;

					GameObject unit = Instantiate(unitPrefab, new Vector3(xPos, yPos + 0.25f, zPos), Quaternion.identity);
					unit.transform.SetParent(this.transform);

					unit.name = "unit_" + username;

					// Add unit to the list of all units
					units.Add(username, unit);

					toUpdate = false;
				}
			}
		}
	}

	public GameObject CreateUnit(int x, int y)
	{
		// TODO: Check if position is taken

		GameObject map = GameObject.Find("Map");
		if (map != null)
		{
			GameObject hex = map.GetComponent<Map>().GetHex(x, y);

			if (hex != null)
			{
				float xPos = hex.transform.position.x;
				float yPos = hex.transform.position.y;
				float zPos = hex.transform.position.z;

				GameObject unit = (GameObject)Instantiate(unitPrefab, new Vector3(xPos, yPos + 0.25f, zPos), Quaternion.identity);
				unit.transform.SetParent(this.transform);

				unit.name = "unit_" + username;

				// Add unit to the list of all units
				units.Add(username, unit);
			}
		}
		return null;
	}

	bool DeleteUnit()
	{
		return false;
	}

}
