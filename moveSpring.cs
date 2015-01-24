using UnityEngine;
using System.Collections;

public class moveSpring : MonoBehaviour {
	public int left=0;
	public int right=0;
	bool jump=false;
	public int jumping=0;
	public bool grounded=true;
	bool started;
	bool[] Arr;

	float currTime=0.0f;
	float lastTime=0.0f;
	int masterI=0;

	// Use this for initialization
	void Start () 
	{
	}
	// Update is called once per frame
	void Update () 
	{
		
	}
	void FixedUpdate()
	{
		if(started)
		{
			currTime=Time.deltaTime+currTime;
			if (currTime>=0.5+lastTime)
			{
				lastTime=currTime;
				Master ();
				if(grounded==true&&jumping==0)
				{
					if (left==1&&right==0)
					{
						Debug.Log(left);
						this.gameObject.rigidbody.velocity=new Vector3(-1,0,0); 
					}
					else if (right==1&&left==0)
					{
						this.gameObject.rigidbody.velocity=new Vector3(1,0,0);
					}

					else
					{
						this.gameObject.rigidbody.velocity=new Vector3(0,0,0); 
					}
					if (jump==true)
					{
						this.gameObject.rigidbody.velocity=new Vector3(0,2,0);
						jumping=3;
						jump=false;
					}
				}
				else if(jumping>0)
				{
					if (jumping>1)
					{
						if(right==1&&left==0){this.gameObject.rigidbody.velocity=new Vector3(2,0,0);}
						else if(left==1&&right==0){this.gameObject.rigidbody.velocity=new Vector3(-2,0,0);}
						else{this.gameObject.rigidbody.velocity=new Vector3(0,-2,0);jumping=0;}
					}
					else if(jumping==1)
					{
						this.gameObject.rigidbody.velocity=new Vector3(0,-2,0);
					}
					jumping--;
				}
				else if(grounded==false)
				{
					this.gameObject.rigidbody.velocity=new Vector3(0,-1,0); 
						grounded=true;
				}
			}
		}
	}

	void Master()
	{
		masterI++;
		left=0;
		right=0;

		if(Arr[Mathf.RoundToInt(masterI/2)]==true)
		{left=1;}
		if(Arr[Mathf.RoundToInt(masterI/2)+1000]==true)
		{right=1;}
		if(Arr[Mathf.RoundToInt(masterI/2)+2000]==true)
		{jump=true;}

	}

	public void ReciveInfo(bool[] info)
	{
		Arr=info;
		started=true;
	}
}
