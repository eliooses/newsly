using UnityEngine;
using System.Collections;

public class article : MonoBehaviour {

	public GameObject Logo;
	public GameObject URLText;
	public GameObject MainPic;
	public GameObject TitleText;
	public GameObject ContentText;

	public GameObject Left;
	public GameObject Right;
	public GameObject Up;

	public Camera blurCamera;
	public GameObject blurredPic;

	public int order=0;

	// Use this for initialization
	void Start () {
		InitArticle();

	}

	void InitArticle(){
		URLText.GetComponent<TextMesh>().renderer.sortingOrder = -order*20 + 10;
		TitleText.GetComponent<TextMesh>().renderer.sortingOrder = -order*20 + 10;
		ContentText.GetComponent<TextMesh>().renderer.sortingOrder = -order*20+ 10;

		TextUtil texter - new TextUtil();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
