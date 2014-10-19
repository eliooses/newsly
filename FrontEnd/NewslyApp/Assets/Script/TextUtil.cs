using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TextUtil {
	
	public void TextForm(GameObject textObject, string text,bool ifBanner, float screenWidth, float screenHeight, GameObject wind1, float widthLimit = 8.25f ){
		
		
		//float ortho = Camera.main.orthographicSize/10f;
		
		int maxChar = 30;
		if(!ifBanner){
			maxChar = 30;
		}
		string[] lines = GetTextParts(text, maxChar);
		
		string textToProduce = "";
		for(int i=0;i<lines.Length; i++){
			textToProduce +=lines[i]+"\n";
		}
		
		textObject.GetComponent<TextMesh>().text = textToProduce;
		
		//Orientation pos chnager
		var scaler = 1f;
		
		if(!ifBanner && screenWidth>screenHeight){
			scaler = 1.25f;
		}
		
		//resize
		float limitWindX = screenWidth-1f;
		
		if(!ifBanner){
			limitWindX =   widthLimit;
		}
		
		float resizeX =  limitWindX/textObject.renderer.bounds.size.x;
		float resizeY =  3.5f/textObject.renderer.bounds.size.y;
		
		float min = Math.Min (resizeX,resizeY);
		
		
		//reposition
		var moveUp = 0f;
		if(lines.Length>1){
			moveUp = scaler * lines.Length * 0.1f;
		}
		
		var pos = new Vector3(wind1.transform.position.x, wind1.transform.position.y - moveUp);
		Debug.Log (pos);
		
		textObject.transform.position = pos;
		textObject.transform.localScale *= min;
		
	}
	
	
	
	public void TextBox(GameObject textObject, string text, float x, float y ){
		
		float ortho = 1f;//Camera.main.orthographicSize/10f;

		//Text Formatting
		int maxChar = Mathf.RoundToInt(3 * x);
		
		string[] lines = GetTextParts(text, maxChar);
		
		
		string textToProduce = "";
		for(int i=0;i<lines.Length; i++){
			textToProduce +=lines[i]+"\n";
		}
		
		textObject.GetComponent<TextMesh>().text = textToProduce;
		
		//Resizing
		var xText = textObject.renderer.bounds.size.x;
		textObject.GetComponent<TextMesh>().characterSize *=  x /xText;
		
		var yText = textObject.renderer.bounds.size.y;
		
		if(yText>y){
			
			maxChar = Mathf.RoundToInt((yText / y) * 3 * x);
			
			lines = GetTextParts(text, maxChar);
			
			textToProduce = "";
			for(int i=0;i<lines.Length; i++){
				textToProduce +=lines[i]+"\n";
			}
			
			textObject.GetComponent<TextMesh>().text = textToProduce;
			
			
			xText = textObject.renderer.bounds.size.x;
			textObject.GetComponent<TextMesh>().characterSize *= x /xText;
		}
		
		
		
		textObject.GetComponent<TextMesh>().transform.localScale *= ortho;
		
	}
	
	
	
	
	// text splitator :D ;)
	
	public string [] GetTextLines( string str )
	{
		string [] parts = str.Split('\n');
		return parts;
	}
	public string [] GetTextParts( string str, int maxLength )
	{
		string [] lines = GetTextLines( str );
		string [] words = str.Split(new Char [] {' ', '\n' });
		for(int i = 0; i < words.Length;i ++ )
		{
			int p = words[i].Length;
			if( maxLength < p ) maxLength = p;
		}
		
		List < string > allParts = new List<string>();
		
		for(int i = 0; i < lines.Length; i++ )
		{
			if( lines[i] == "" )continue;
			
			string [] temp = GetTextParts1( lines[i], maxLength );
			for(int j = 0; j < temp.Length; j++ )
			{
				if( temp[j]=="")continue;
				allParts.Add( temp[j] );
			}
		}
		
		string [] result = new string[ allParts.Count ];
		for(int i = 0; i < allParts.Count; i++ )
			result[i] = allParts[i];
		return result;
		
	}
	private string [] GetTextParts1( string str, int maxLength )
	{
		string [] parts = str.Split(' ');
		string tmp, temp = "";
		
		for(int i = 0; i < parts.Length;i ++ )
		{
			if( maxLength < parts[i].Length )
				maxLength = parts[i].Length;
		}
		
		//Debug.LogError(maxLength);
		List < string> buf = new List<string>();
		
		for(int i = 0; i < parts.Length; i++ )
		{
			
			tmp = "";
			if( temp == "" )
				tmp = parts[i];
			else
				tmp = temp + " " + parts[i];
			
			
			if( tmp.Length > maxLength )
			{
				buf.Add( temp );
				temp = "";
				i--;
			}
			else
			{
				temp = tmp;
			}
		}
		
		if( temp != "" )
			buf.Add(temp);
		
		parts = new string[ buf.Count ];
		for(int i = 0; i < parts.Length;i++)
		{
			parts[i] = buf[i];
		}
		return parts;
	}
	
}
