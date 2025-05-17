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

	private string[] noteNames = { "D4", "E4", "G4", "A4", "C5", "D5", "E5", "G5" };
	private float[] durationList = {1f, 2f, 4f};
	private float xPos = -7f;
	private GameObject staffObj;
	private Vector2 position;
	private float totalBeats = 0f;
	private List<object[]> staffNotesList = new List<object[]>();

	private void Start()
	{
		/*
		while (totalBeats < 256)
		{
			if (totalBeats % 4 == 0)
			{
				position = staffObj.transform.position;
				position.x = xPos;
				xPos += 5f;
				staffNotesList.Add(staffObj);
			}


			string pitch = noteNames[Random.Range(0, 7)];
			float noteHeight = pitchToHeight.GetHeightFromPitch(pitch);
			float noteDuration = durationList[Random.Range(0, 3)];


			switch (noteDuration)
			{
				case 1f:
					position = staffObj.transform.position;
					position.x = xPos;
					staffObj.transform.position = position;
					totalBeats += noteDuration;
					staffNotesList.Add(staffObj);
					break;
				case 2f:
					position = staffObj.transform.position;
					position.x = xPos;
					staffObj.transform.position = position;
					totalBeats += noteDuration;
					staffNotesList.Add(staffObj);
					break;

				case 4f:
					position = staffObj.transform.position;
					position.x = xPos;
					staffObj.transform.position = position;
					totalBeats += noteDuration;
					staffNotesList.Add(staffObj);
					break;
			}
			xPos += 5f;
			Debug.Log(staffNotesList);
		}

		*/


		while (totalBeats < 256f) //pregen the notes and then draw them as the player is playing
		{
			if (totalBeats % 4 == 0)
			{
				staffNotesList.Add(["barline", xPos, barline]);
			}
			else
			{
				string pitch = noteNames[Random.Range(0, 7)];
				float noteHeight = pitchToHeight.GetHeightFromPitch(pitch);
				float noteDuration = durationList[Random.Range(0, 3)];
				switch (noteDuration)
				{
					case 1f:
						staffNotesList.Add(["note", xPos, noteHeight, pitch, quarterNote]);
						break;
					case 2f:
						staffNotesList.Add(["note", xPos, noteHeight, pitch, halfNote]);
						break;
					case 4f:
						staffNotesList.Add(["note", xPos, noteHeight, pitch, wholeNote]);
						break;
				}
			}
			xPos += 5;
		}

	}

	private void Update()
	{
		for (int i = 0; i < staffNotesList.Count; i++)
		{
			if ((float) staffNotesList[i][1] >= - 7f && (float) staffNotesList[i][1] <= 15f)
			{
				//this means we can render it cause the camera can see it, so we go ahead
			}
		}
	}
}
	


