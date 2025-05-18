using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private spawnNotes noteSpawner;
	[SerializeField] private setUpQuestions setUpQuestions;
	[SerializeField] private playMetronome metronome;
	[SerializeField] private GameObject beatDisplay;
	private int currentNoteIndex = 1;
	[SerializeField] private float playerMoveSpeed = 1f;
	private Vector2 newPlayerPos;
	public event EventHandler OnPlayerJump;
	public event EventHandler OnKeyPress;
	private string[] answerArray = new string[2];
	private Vector2 playerPos;

	void Start()
    {
		playerPos = noteSpawner.notesToRender[1].GetComponent<noteScript>().GetPlayerPoint().transform.position;
		transform.position = playerPos;
		OnPlayerJump += WhenPlayerJumps;
		OnKeyPress += WhenKeyIsPressed;
		answerArray = setUpQuestions.ResetQuestion();
    }

	private void WhenKeyIsPressed(object key, EventArgs e)
	{
		
		//first check which kind of question it was
		//interval question has 2 things in the array, note questions have amongus at the end
		if (answerArray[1] != "amongus") //interval question
		{
			if (answerArray[0] == "1" && (KeyCode) key == KeyCode.Alpha1 || answerArray[0] == "2" && (KeyCode) key == KeyCode.Alpha2)//if the right answer is pressed
			{

				//check if player jumped too early
				float correctJumpBeat = noteSpawner.notesToRender[GetCurrentNoteIndex()].GetComponent<noteScript>().GetNoteDuration(noteSpawner.notesToRender[GetCurrentNoteIndex()].GetComponent<noteScript>().GetNoteType());
				correctJumpBeat++;
				float[] validJumpBeats = {correctJumpBeat - 0.5f, correctJumpBeat};

				Debug.Log(correctJumpBeat.ToString() + " is the correct beat");
				if (validJumpBeats[0] == metronome.beatInBar || validJumpBeats[1] == metronome.beatInBar)
				{
					//player jumps
					int nextNoteIndex = currentNoteIndex + 1;
					//gets the next place to set the playerpos to
					while (true)
					{
						if (noteSpawner.notesToRender[nextNoteIndex].GetComponent<noteScript>().GetNoteType() != "barline")
						{
							newPlayerPos = noteSpawner.notesToRender[nextNoteIndex].GetComponent<noteScript>().GetPlayerPoint().transform.position;

							break;
						}
						else
						{
							nextNoteIndex += 1;
						}

					}

					//transform.position = Vector2.Lerp(playerPos, newNotePos, playerMoveSpeed * Time.deltaTime);
					currentNoteIndex++;
					//transform.position = newPlayerPos;
					//position changed! FIRE THE EVENTTT
					OnPlayerJump?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					noteSpawner.EndGame(false);
				}
			}
			else //player jumped at the wrong time
			{
				noteSpawner.EndGame(false);

			}
				
		}
		else if (answerArray[1] == "amongus")//note question
		{
			string keyNeeded = answerArray[0];
			Debug.Log(keyNeeded);
			
			KeyCode answer = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyNeeded);

			if ((KeyCode) key == answer)//they got the right answer
			{
				//check if player jumped too early
				float correctJumpBeat = noteSpawner.notesToRender[GetCurrentNoteIndex()].GetComponent<noteScript>().GetNoteDuration(noteSpawner.notesToRender[GetCurrentNoteIndex()].GetComponent<noteScript>().GetNoteType());
				correctJumpBeat++;
				float[] validJumpBeats = { correctJumpBeat - 0.5f, correctJumpBeat };

				Debug.Log(correctJumpBeat.ToString() + " is the correct beat");
				if (validJumpBeats[0] == metronome.beatInBar || validJumpBeats[1] == metronome.beatInBar)
					if (correctJumpBeat == metronome.beatInBar)
				{
					//player jumps
					int nextNoteIndex = currentNoteIndex + 1;
					//gets the next place to set the playerpos to
					while (true)
					{
						if (noteSpawner.notesToRender[nextNoteIndex].GetComponent<noteScript>().GetNoteType() != "barline")
						{
							newPlayerPos = noteSpawner.notesToRender[nextNoteIndex].GetComponent<noteScript>().GetPlayerPoint().transform.position;

							break;
						}
						else
						{
							nextNoteIndex += 1;
						}

					}

					//transform.position = Vector2.Lerp(playerPos, newNotePos, playerMoveSpeed * Time.deltaTime);
					currentNoteIndex++;
					//transform.position = newPlayerPos;
					//position changed! FIRE THE EVENTTT
					OnPlayerJump?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					noteSpawner.EndGame(false);
				}
			}
			else //player jumped at the wrong time
			{
				noteSpawner.EndGame(false);

			}
		}
	}

	private void WhenPlayerJumps(object sender, EventArgs e)
	{

		answerArray = setUpQuestions.ResetQuestion();
	}

	// Update is called once per frame
	void Update()
	{
		playerPos = noteSpawner.notesToRender[currentNoteIndex].GetComponent<noteScript>().GetPlayerPoint().transform.position;
		transform.position = playerPos;

		beatDisplay.GetComponentInChildren<TextMeshProUGUI>().GetComponentInChildren<TextMeshProUGUI>().text = ((int) metronome.beatInBar).ToString();

		foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(key))
			{
				OnKeyPress?.Invoke(key, EventArgs.Empty);
			}
		}

	}
	public int GetCurrentNoteIndex()
	{
		return currentNoteIndex;
	}
	
	public int GetNextNoteIndex(int noteIndex)
	{
		int nextNoteIndex = noteIndex + 1;

		while (true)
		{
			if (noteSpawner.notesToRender[nextNoteIndex].GetComponent<noteScript>().GetNoteType() != "barline")
			{
				newPlayerPos = noteSpawner.notesToRender[nextNoteIndex].GetComponent<noteScript>().GetPlayerPoint().transform.position;

				break;
			}
			else
			{
				nextNoteIndex += 1;
			}

			
		}
		return nextNoteIndex;
	
	}
}
