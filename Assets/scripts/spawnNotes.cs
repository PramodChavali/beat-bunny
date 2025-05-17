using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawnNotes : MonoBehaviour
{
	[SerializeField] private noteList pitchToHeight;
	[SerializeField] private GameObject quarterNote;
	[SerializeField] private GameObject halfNote;
	[SerializeField] private GameObject wholeNote;
	[SerializeField] private GameObject barline;
	[SerializeField] public float moveSpeed;
	[SerializeField] private GameObject trebleClef;


	private string[] noteNames = { "D4", "E4", "F4", "G4", "A4", "B4", "C5", "D5", "E5", "F5", "G5" };
	private float[] durationList = {1f, 2f, 4f};
	private float beatsInBar = 0f;
	private float xPos = -7f;
	private GameObject staffObj;
	private Vector2 position;
	private float totalBeats = 0f;
	private List<List<object>> staffNotesList = new List<List<object>>();
	public List<GameObject> notesToRender = new List<GameObject>();
	private GameObject staffObjVisual;
	private bool barlined = false;
	private int index = 0;

	private void Awake()
	{


		while (totalBeats < 256f) //pregen the notes and then draw them as the player is playing
		{
			List<object> staffObjInfo = new List<object>();
			if (totalBeats % 4 == 0 && barlined == false)
			{
				beatsInBar = 0f;
				staffObjInfo.Add("barline");
				staffObjInfo.Add(xPos);
				staffObjInfo.Add(barline);
				xPos += 5f;
				barlined = true;

				staffObj = new GameObject();
				GameObject objToRender = new GameObject();

				for (int i = 0; i < staffObjInfo.Count; i++)
				{
					Debug.Log(staffObjInfo[i].ToString());
				}
				objToRender = (GameObject) staffObjInfo[2];


				staffObj = Instantiate(objToRender);
				position = staffObj.transform.position;
				position.x = (float) staffObjInfo[1];
				staffObj.transform.position = position;
				staffObj.GetComponent<noteScript>().HideNoteVisual();
				staffObj.GetComponent<noteScript>().SetNoteType("barline");
				notesToRender.Add(staffObj);

			}
			else
			{
				string pitch = noteNames[UnityEngine.Random.Range(0, 11)];
				float noteHeight = pitchToHeight.GetHeightFromPitch(pitch);
				float noteDuration = durationList[UnityEngine.Random.Range(0, 3)];
				
				while ((noteDuration + beatsInBar) > 4)
				{
					noteDuration = durationList[UnityEngine.Random.Range(0, 3)];
				}
				
				staffObjInfo.Add("note");
				staffObjInfo.Add(xPos);
				staffObjInfo.Add(noteHeight);
				staffObjInfo.Add(pitch);

				switch (noteDuration)
				{
					case 1f:
						staffObjInfo.Add(quarterNote);
						break;
					case 2f:
						staffObjInfo.Add(halfNote);
						break;
					case 4f:
						staffObjInfo.Add(wholeNote);
						break;
				}

				GameObject staffObj;
				GameObject objToRender;

				objToRender = (GameObject) staffObjInfo[4];


				staffObj = Instantiate(objToRender);
				position = staffObj.transform.position;
				position.x = (float) staffObjInfo[1];
				position.y = (float) staffObjInfo[2];
				staffObj.transform.position = position;
				staffObj.GetComponent<noteScript>().HideNoteVisual();
				staffObj.GetComponent<noteScript>().SetNoteType(noteDuration);
				staffObj.GetComponent<noteScript>().SetIndex(index);
				staffObj.GetComponent<noteScript>().SetPitch(pitch);

				notesToRender.Add(staffObj);

				totalBeats += noteDuration;
				beatsInBar += noteDuration;
				xPos += 5;
				barlined = false;
				index++;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < notesToRender.Count; i++)
		{
			if (notesToRender[i].transform.position.x >= -7f && notesToRender[i].transform.position.x <= 15f)
			{
				notesToRender[i].GetComponent<noteScript>().ShowNoteVisual();

			}
			if (notesToRender[i].transform.position.x >= -7f)
			{ 
				position = notesToRender[i].transform.position;
				position.x -= moveSpeed * Time.deltaTime;
				notesToRender[i].transform.position = position;
			}
			else
			{
				notesToRender[i].GetComponent <noteScript>().HideNoteVisual();
			}
		}	
	}
}

	


