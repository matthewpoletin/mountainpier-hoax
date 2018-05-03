using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Users : MonoBehaviour {

	public GameObject userPrefab;

	// Use this for initialization
	public string userUsername = "";
	public string userUrl = "";

	private IEnumerator Start()
	{

		// Start a download of the given URL
		using (WWW www = new WWW(userUrl))
		{
			// Wait for download to complete
			yield return www;

			GameObject userGameObject = Instantiate(userPrefab, new Vector3(0, 5, 0), Quaternion.identity);
			// Adjsut hierarchy of objects
			userGameObject.transform.SetParent(this.transform, false);
			// Set object's name to username
			userGameObject.name = userUsername;

			// Set text
			userGameObject.transform.Find("Text").GetComponent<Text>().text = userUsername;

			// Set avatar
			// TODO: Add convertion pixelPerUnit from www.texture. width & height
			userGameObject.transform.Find("Image").GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 10000.0f);
		}
	}

}
