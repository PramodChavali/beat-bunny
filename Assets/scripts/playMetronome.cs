using UnityEngine;

public class playMetronome : MonoBehaviour
{
    private AudioSource metrenomeClick;
	[SerializeField] private float BPM;
	private float frequency;
	private float timer;

	private void Start()
	{
		metrenomeClick = GetComponent<AudioSource>();
		frequency = 60f / BPM;
		Debug.Log("Frequency is " + frequency.ToString());
		timer = frequency;
	}
	private void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			//metrenomeClick.Play();
			timer = frequency;
		}
		
	}
}
