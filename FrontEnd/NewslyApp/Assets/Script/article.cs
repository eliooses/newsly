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

	public int order=1;

	public GameObject webview;

	ArticleManager Manager;

	private string url;
	private bool webviewOn = false;

	// Use this for initialization
	void Start () {
		Manager = Camera.main.GetComponent<ArticleManager>();
	}

	public void MoveBack(){
		//iTween.MoveTo(this.gameObject, new Vector3(0,0,1), 1f);

		this.gameObject.transform.position = new Vector3(0,0,1);
		URLText.SetActive(false);
		TitleText.SetActive(false);
		ContentText.SetActive(false);

	} 

	public void MoveFirst(){
		iTween.MoveTo(this.gameObject, new Vector3(0,0,0), 1f);
		
		URLText.SetActive(true);
		TitleText.SetActive(true);
		ContentText.SetActive(true);
		
	}

	public void InitArticle(string title, string content, string url, string picURL){

		StartCoroutine(setTexture(MainPic,picURL));


		iTween.Stop(this.gameObject);

		this.transform.position = new Vector3(0,0,4);
		this.transform.rotation = Quaternion.identity;

		this.url = url;



		URLText.GetComponent<TextMesh>().renderer.sortingOrder = 10;
		TitleText.GetComponent<TextMesh>().renderer.sortingOrder = 10;
		ContentText.GetComponent<TextMesh>().renderer.sortingOrder = 10;

		URLText.SetActive(true);
		TitleText.SetActive(true);
		ContentText.SetActive(true);
		
		Left.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,0.5f);
		Right.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,0.5f);
		Up.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,0.5f);


		URLText.GetComponent<TextMesh>().text = "The Guardian";
		TextUtil texter = new TextUtil();
		if(content == ""){
			ContentText.GetComponent<TextMesh>().text = "";
			//texter.TextBox(ContentText, content, 4.6f, 2f);
		}else{
			texter.TextBox(ContentText, content, 4.6f, 2f);
		}

		texter.TextBox(TitleText,title , 4.6f, 2.8f);

	}


	IEnumerator setTexture(GameObject pic, string url){

		if(url==null || url == ""){
			//friend.SetActive(false);
			yield break;
		}
		WWW www = new WWW(url);
		yield return www;

		var rect = new Rect(0, 0, www.texture.height, www.texture.height);
		var pivot = new Vector3(0.5f,0.5f,0);
		
		if(www.texture.height<=www.texture.width){
		    rect = new Rect(0, 0, www.texture.height, www.texture.height);
		
		}
		else{
			 rect = new Rect(0, 0, www.texture.width, www.texture.width);
		}


		pic.GetComponent<SpriteRenderer>().sprite =Sprite.Create( www.texture, rect, pivot);

		//pivot = new Vector3(10f,10f,0f);
		blurredPic.GetComponent<SpriteRenderer>().sprite = Sprite.Create(www.texture, rect, pivot);
		//friend.transform.localScale = new Vector3(3.4f, 3.4f, 1);
		//friend.transform.position = friend.transform.parent.transform.position;
		
		//var friendPic = new friendPhoto();
		//friendPic.facebookID = id;
		//friendPic.friendSprite = friend.GetComponent<SpriteRenderer>().sprite;
		//friendsAvatar.Add(friendPic);
	}



	void LeftEnded(){
		Left.GetComponent<BoxCollider>().size = new Vector3(1f,1f,1f);
		iTween.Stop(this.gameObject);
		Left.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,0.5f);
		iTween.RotateTo(this.gameObject, new Vector3(0,0,0), 0.5f);

		flip(false);

	}
	
	void RightEnded(){
		Right.GetComponent<BoxCollider>().size = new Vector3(1f,1f,1f);
		iTween.Stop(this.gameObject);
		Right.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,0.5f);
		iTween.RotateTo(this.gameObject, new Vector3(0,0,0), 0.5f);

		flip(true);
	}
	
	void UpEnded(){
		Up.GetComponent<BoxCollider>().size = new Vector3(1f,1f,1f);
		iTween.Stop(this.gameObject);
		Up.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,0.5f);
		iTween.RotateTo(this.gameObject, new Vector3(0,0,0), 0.5f);

		//Functional for moving article
		openWebview();
	}
	

	//Animation

	void flip(bool right){
		iTween.MoveTo(this.gameObject, new Vector3(right?10f:-10f,3f,0), 2.6f);
		iTween.RotateTo(this.gameObject, new Vector3(0,right?-60f:60f,0), 1f);

		StartCoroutine("changer");
	}

	IEnumerator changer(){
		//yield return new WaitForSeconds(0.1f);
		if(Manager.firstIsUsingInput)
		{
			//Manager.article2.GetComponent<article>().MoveFirst();
		}
		else{
			//Manager.article1.GetComponent<article>().MoveFirst();
		}
		yield return new WaitForSeconds(1f);
		Manager.changeState();
	}

	void openWebview(){
		webviewOn= true;


		iTween.RotateTo(this.gameObject, new Vector3(20,0,0),.5f );
		webview.SetActive(true);
		
		webview.GetComponent<UWKWebView>().LoadURL(url);
		webview.GetComponent<UWKWebView>().SetZoomFactor(2f);

		StartCoroutine("openWeb");
	}

	IEnumerator openWeb(){
		yield return new WaitForSeconds(.5f);
	
		foreach(Transform child in transform){
			child.gameObject.SetActive(false);
		}

	
	}

	void closeWebsite(){
		iTween.RotateTo(this.gameObject, new Vector3(0,0,0),1f );

		webview.SetActive(false);

		foreach(Transform child in transform){
			child.gameObject.SetActive(true);
		}
		webviewOn = false;
	}

	void LeftStarted(){
		Left.GetComponent<BoxCollider>().size *=10;
		Left.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,1f);
		iTween.RotateTo(this.gameObject, new Vector3(0,10,0), 1f);

	}

	void RightStarted(){

		Right.GetComponent<BoxCollider>().size *=10;
		Right.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,1f);
		iTween.RotateTo(this.gameObject, new Vector3(0,-10,0), 1f);
	}

	void UpStarted(){

		Up.GetComponent<BoxCollider>().size *=10;
		Up.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,1f);
		iTween.RotateTo(this.gameObject, new Vector3(10,0,0), 1f);
	}

	private Vector2 fp,lp;


	// Update is called once per frame
	void Update () {


		if((Manager.firstIsUsingInput &&  Manager.article1==this.gameObject) || (!Manager.firstIsUsingInput &&  Manager.article2==this.gameObject)){

		
			foreach (Touch touch in Input.touches)
			{
				if (touch.phase == TouchPhase.Began)
				{
					fp = touch.position;
					lp = touch.position;
				}
				if (touch.phase == TouchPhase.Moved )
				{
					lp = touch.position;
				}
				if(touch.phase == TouchPhase.Ended)
				{ 
					
					if((fp.x - lp.x) > 80 && !webviewOn) // left swipe
					{
						
						LeftEnded();
						
					}
					else if((fp.x - lp.x) < -80 && !webviewOn) // right swipe
					{
						RightEnded();
					}
					else if((fp.y - lp.y) < -80 && !webviewOn) // up swipe
					{
						UpStarted();
						UpEnded();
					}
					else if((fp.y - lp.y) > 80 ) // down swipe
					{
						if(webview.activeSelf){
							closeWebsite();
						}
					}
				}
			}
				
				
		}	


		if(Input.touchCount > 0){
			Ray ray = Camera.main.ScreenPointToRay( Input.GetTouch(0).position );
			RaycastHit hit;
			
			if ( Physics.Raycast(ray, out hit))
			{
				if(Input.GetTouch(0).phase == TouchPhase.Began){
					
					if( hit.transform == Left.transform){
						LeftStarted();
					}
					if(hit.transform == Right.transform){
						RightStarted();
					}
					if(hit.transform == Up.transform){
						UpStarted();
					}
				}
				else if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled){
					if(hit.transform == Left.transform){
						LeftEnded();
					}
					if(hit.transform == Right.transform){
						RightEnded();
						
					}
					if(hit.transform == Up.transform){
						UpEnded();
					}
				}
			} 

		}
		
	}
}
