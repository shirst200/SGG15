using UnityEngine;
using System.Collections;

public class NetworkScript : MonoBehaviour {

	public bool[] gotSeq;
	public bool running;
	
	private float bttnW = 150;
	private float bttnH = 40;
	
	private bool Refreshing = false;
	private HostData[] hostData;
	
	private string gameName = "GGC15";


	//controller
	public bool[] Setting;
	public GameObject Bot;
	public int noColumns;
	public int noRows;
	public string[] RowNames;
	public int noBots;

	void Start(){
		Setting = new bool[8000];
	}
	
	void Update()
	{
		if (Refreshing)
		{
			if (MasterServer.PollHostList().Length > 0)
			{
				Debug.Log(MasterServer.PollHostList().Length.ToString());
				Refreshing = false;
				hostData = MasterServer.PollHostList();
			}
		}
	}
	
	void startServer()
	{
		//initialize the server
		Network.InitializeServer(2, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameName, "Ryans Test Server of Greatness", "This is my game to test networking");
	}
	
	void RefreshHostList()
	{
		MasterServer.RequestHostList(gameName);
		Refreshing = true;
		
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server Initialized!");
	}
	
	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse == MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log("Registered Server");
		}
	}

	[RPC]
	void setTasks(bool[] info){
		gotSeq = info;
		Bot.BroadcastMessage("ReciveInfo",info);
	}

	[RPC]
	void setRunning(bool info){
		running = info;
	}
	
	void OnGUI()
	{

		if(Network.isClient){
			//controller
			if(GUI.Button(new Rect(0,0,Screen.width/5,Screen.height/(noRows+1)),"Menu")){}
			if(GUI.Button(new Rect(Screen.width/2-Screen.width/10,0,Screen.width/5,Screen.height/(noRows+1)),"Start/Stop")){
				networkView.RPC("setTasks", RPCMode.AllBuffered, Setting);
				Debug.Log("DOne it");
			}
			if(GUI.Button(new Rect(Screen.width-Screen.width/15,0,Screen.width/15,Screen.height/(noRows+1)),"Clear")){}
			if(GUI.Button(new Rect(Screen.width-Screen.width/15*2,0,Screen.width/15,Screen.height/(noRows+1)),"Next Page")){}
			if(GUI.Button(new Rect(Screen.width-Screen.width/15*3,0,Screen.width/15,Screen.height/(noRows+1)),"Last Page")){}
			for (int n=0;n<noRows;n++)
			{
				for(int i=0;i<noColumns;i++)
				{
					if(GUI.Button(new Rect(Screen.width/(noColumns+1)*(i+1),Screen.height/(noRows+2)*(n+1.5f),Screen.width/(noColumns+1),Screen.height/(noRows+2)),""+(i+1)))
					{
						Setting[i+n*1000]=true;
					}
				}
			}
			for(int n=0;n<noRows;n++)
			{
				GUI.Button(new Rect(0,Screen.height/(noRows+2)*(n+1.5f),Screen.width/(noColumns+1),Screen.height/(noRows+2)),RowNames[n]);
			}
			for(int i=0;i<noBots;i++)
			{
				if(GUI.Button(new Rect(Screen.width/6*i,Screen.height-Screen.height/(noRows+12),Screen.width/6,Screen.height/(noRows+12)),"Robot"+(i+1))){}
			}
		}
		
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(50, 50, bttnW, bttnH), "Start Server"))
			{
				startServer();
			}
			
			if (GUI.Button(new Rect(50, 100, bttnW, bttnH), "Refresh"))
			{
				Debug.Log("Refreshing...");
				RefreshHostList();
			}
			
			GUILayout.BeginArea(new Rect(Screen.width - 500, 0, 500, Screen.height), "Server List");
			GUILayout.Space(20);
			foreach (HostData hostedGame in MasterServer.PollHostList())
			{
				GUILayout.BeginHorizontal("Box");
				GUILayout.Label(hostedGame.gameName);
				if (GUILayout.Button("Connect"))
				{
					Network.Connect(hostedGame);
				}
				
				GUILayout.EndHorizontal();
			}
			
			GUILayout.EndArea();
		}
	}
}
