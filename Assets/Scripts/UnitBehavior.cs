using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehavior : MonoBehaviour {
	
	public int x;
	public int y;
	
	// Object we operate
	public GameObject unit;

	private void Start()
	{
		this.unit = this.transform.gameObject;
	}

}
