using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RestService : MonoBehaviour
{
	// TODO: add local request timeout
	//private const float waitTimeout = 10.0f;

	public enum Method { Get, Post, Patch, Put, Delete, Head, Otions }
	
	public void Request<T>(Method method, string uri, string data, Action<T> callback)
	{
		Debug.Log("RestService: send " + method + "request to " + uri);
		StartCoroutine(ExecuteRequest(method, uri, null, callback));
	}
	
	private static IEnumerator ExecuteRequest<T>(Method method, string uri, string data, Action<T> callback)
	{
		// TODO: Add support for different methods
		Action<string, string> action;
		switch (method)
		{
			case Method.Post:
				action = (string s, string d) => UnityWebRequest.Post(s, d);
				break;
			case Method.Get:
				action = (string s, string d) => UnityWebRequest.Get(s);
				break;
			default:
				Debug.LogError("POST method is not implemented yet");
				break;
		}
		
//		using (UnityWebRequest request = action(url, data))
		using (UnityWebRequest request = UnityWebRequest.Get(uri))
		{
			// TODO: Add authentication
//			request.SetRequestHeader("accessToken", "accessToken");
			yield return request.Send();
			if (request.isNetworkError || request.isHttpError)
			{
				Debug.Log(request.error);
			}
			else
			{
				if (request.isDone)
				{
					string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
					Debug.Log(jsonResult);
					T entities = JsonHelper.GetJson<T>(jsonResult);

					if (callback != null)
						callback(entities);
				}
			}
		}
	}
	
}
