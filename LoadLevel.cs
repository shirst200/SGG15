using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadLevel : MonoBehaviour {

	public int width;
	public int height;
	public GameObject[] blockTypes;
	public Transform container;
	public GameObject[][] rows;
	public Transform background;
	public byte[] chunkSave;
	private const string FILE_NAME = "levelOne.chunks";

	// Use this for initialization
	void Start () {
		chunkSave =  new byte[width*height];
		rows = new GameObject[height][];
		//Scale the camera to the desired size of the map
		transform.camera.orthographicSize = (float)height/2;
		transform.position = new Vector3((float)width/2, (float)height/2, -1f);
		//Create background image and scale
	//	background.position = new Vector3((float)width/2, (float)height/2, 10f);
	//	background.localScale = new Vector3(width, height, 1);
		//Create slots in container
		for(int i = 0; i < height; i++){
			GameObject newRow = new GameObject("Row"+i.ToString());
			newRow.transform.parent = container;
			GameObject[] collums = new GameObject[width];
			for(int j = 0; j < width; j++){
				collums[j] = new GameObject("Collumn"+j.ToString());
				collums[j].transform.parent = newRow.transform;
			}
			rows[i] = collums;
		}
		Load();
	}

	public void Load(){	
		FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
		BinaryReader r = new BinaryReader(fs);
		for (int i = 0; i < chunkSave.Length; i++) 
		{
			Vector3 cPos = new Vector3((int)(i%width)+.5f,(int)(i/width)+.5f,0);
			PlaceAndSort(cPos, r.ReadByte());
		}
		r.Close();
		fs.Close();
	}

	public void PlaceAndSort(Vector3 pos, byte type){
		if(pos.y+.5f <= height && pos.y+.5f >= 1f && pos.x+.5f <= width && pos.x+.5f >= 1f){
			chunkSave[(int)(pos.y)*width+(int)pos.x] = type;
			GameObject tempCube;
			tempCube = Instantiate(blockTypes[type], pos, Quaternion.Euler(0,0,-(0*90))) as GameObject;
			GameObject oldPlace = rows[((int)(pos.y+.5f)-1)][((int)(pos.x+.5f)-1)];
			tempCube.transform.parent = oldPlace.transform.parent;
			tempCube.name = oldPlace.name;
			rows[((int)(pos.y+.5f)-1)][((int)(pos.x+.5f)-1)] = tempCube;
			Destroy(oldPlace);
		}
	}
}