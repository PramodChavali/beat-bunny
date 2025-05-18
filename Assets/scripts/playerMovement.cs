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
	[SerializeField] private playNoteSound soundPlayer;
	private int currentNoteIndex = 1;
	[SerializeField] private float playerMoveSpeed = 1f;
	private Vector2 newPlayerPos;
	public event EventHandler OnPlayerJump;
	public event EventHandler OnKeyPress;
	private string[] answerArray = new string[2];
	private Vector2 playerPos;
	[SerializeField] private bool devMode;
	private bool safeToJump = false;
	private float timeNextJump;
	

	void Start()
    {
		playerPos = noteSpawner.notesToRender[1].GetComponent<noteScript>().GetPlayerPoint().transform.position;
		transform.position = playerPos;
		OnPlayerJump += WhenPlayerJumps;
		OnKeyPress += WhenKeyIsPressed;
		answerArray = setUpQuestions.ResetQuestion();
		timeNextJump = GetTimeNextJump();
		soundPlayer.PlaySoundOfCurrentNote();
	}

	private void WhenKeyIsPressed(object key, EventArgs e)
	{
		float timeKeyPressed = Time.time;
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			noteSpawner.gameRunning = true;
		}

		//first check which kind of question it was
		//interval question has 2 things in the array, note questions have amongus at the end
		if (answerArray[1] != "amongus") //interval question
		{
			if (answerArray[0] == "1" && (KeyCode) key == KeyCode.Alpha1 || answerArray[0] == "2" && (KeyCode) key == KeyCode.Alpha2 || Input.GetKeyDown(KeyCode.Space))//if the right answer is pressed
			{
				

				if ((timeNextJump - 1f) <= timeKeyPressed && timeKeyPressed <= (timeNextJump + 1f))
				{
					safeToJump = true;
				}
				else
				{
					safeToJump = false;
				}

				if (safeToJump)
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
					soundPlayer.PlaySoundOfCurrentNote();
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
			
			KeyCode answer = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyNeeded);

			if ((KeyCode) key == answer)//they got the right answer
			{
				if ((timeNextJump - 1f) <= timeKeyPressed && timeKeyPressed <= (timeNextJump + 1f))
				{
					safeToJump = true;
				}
				else
				{
					safeToJump = false;
				}

				if (safeToJump)
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
					soundPlayer.PlaySoundOfCurrentNote();
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
		setUpQuestions.ResetText();
		timeNextJump = GetTimeNextJump();
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

		if (noteSpawner.notesToRender[GetCurrentNoteIndex()].GetComponent<noteScript>().IsVisualHidden())
		{
			noteSpawner.EndGame(false);
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

	public float GetTimeNextJump()
	{
		float duration = noteSpawner.notesToRender[currentNoteIndex].GetComponent<noteScript>().GetNoteDuration(noteSpawner.notesToRender[currentNoteIndex].GetComponent<noteScript>().GetNoteType());
		//got the duration of next note

		float timeUntilNextJump = metronome.secondsPerBeat* duration;
		float timeOfNextJump = Time.time + timeUntilNextJump;
		return timeOfNextJump;
	}
}
