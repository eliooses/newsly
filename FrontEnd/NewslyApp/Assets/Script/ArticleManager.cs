using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using LitJson;

public class ArticleManager : MonoBehaviour {


	public GameObject article1;
	public GameObject article2;

	private JsonData articles;
	private int nth=0;


	private string headline = "headline";
	private string trailtext = "trailtext";
	private string url = "url";
	private string picurl = "picurl";

	public bool firstIsUsingInput = true;
	// Use this for initialization
	void Start () {
		article1.SetActive(false);
		article2.SetActive(false);

		//Json.Deserialize(t.Text) as Dictionary < string,
		
		StartCoroutine("download");
	
	}

	void init(){

		article1.SetActive(true);
		article2.SetActive(true);

		Debug.Log(articles[nth].Count);

		//10.12.75.28:8080/getallnews
		article1.GetComponent<article>().InitArticle(articles[nth][headline].ToString(), articles[nth][trailtext].ToString(),articles[nth][url].ToString(), articles[nth][picurl].ToString());
		nth++;
		article2.GetComponent<article>().InitArticle(articles[nth][headline].ToString(),articles[nth][trailtext].ToString(),articles[nth][url].ToString(), articles[nth][picurl].ToString());
		article1.GetComponent<article>().MoveFirst();
		article2.GetComponent<article>().MoveBack();


	}
	IEnumerator download()
	{
		
		//Load JSON data from a URL
		string url = "10.12.75.28:8080/getallnews";
		WWW www = new WWW(url);
		
		//Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
		yield return www;

		if (www.error == null)
		{     
			//Process exercises found in JSON file 

			ProcessArticles(www.data);
		}
		else
		{
			Debug.Log("ERROR: " + www.error);
		}
		
	}

	void ProcessArticles(string data){
		Debug.Log (data);
		articles = JsonMapper.ToObject(data); // convert json data to object. 

		init();
	} 
	
	public void changeState(){
		
		if(nth<articles.Count){
			nth++;
		}
		if(firstIsUsingInput){
			firstIsUsingInput = false;
			article1.GetComponent<article>().InitArticle(articles[nth][headline].ToString(), articles[nth][trailtext].ToString(),articles[nth][url].ToString(), articles[nth][picurl].ToString());
			article1.GetComponent<article>().MoveBack();
			article2.GetComponent<article>().MoveFirst();
		}
		else{
			firstIsUsingInput = true;
			article2.GetComponent<article>().InitArticle(articles[nth][headline].ToString(), articles[nth][trailtext].ToString(),articles[nth][url].ToString(), articles[nth][picurl].ToString());
			article2.GetComponent<article>().MoveBack();
			article1.GetComponent<article>().MoveFirst();
		}
		Debug.Log(firstIsUsingInput?"First Using":"Second Using");

	}
	// Update is called once per frame
	void Update () {
	
	}
}
