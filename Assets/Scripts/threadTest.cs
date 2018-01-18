using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threadTest : MonoBehaviour {

	Job myJob;
	void Start ()
	{
		Debug.Log ("thread test is started");
		myJob = new Job();
		myJob.InData = new Vector3[1];
		myJob.OutData = new Vector3[1];
		myJob.InData [0] = new Vector3 (3, 4, 5);


		myJob.Start(); // Don't touch any data in the job class after you called Start until IsDone is true.
	}
	int m = 0;
	void Update()
	{
		Debug.Log ("in Update");
		if (myJob != null)
		{
			Debug.Log ("my job is not null");
			Debug.Log ("Indata after done: " + myJob.InData [0]);
			if (myJob.Update())
			{
				
				Debug.Log ("my jot is updated");
				if (myJob.IsDone == true) {
					Debug.Log ("Indata after done: " + myJob.InData [0]);
					myJob.InData [0] = new Vector3 (m, 4, 5);
					m++;
					Debug.Log ("????");
				}
				// Alternative to the OnFinished callback
//				myJob = null;
			}
		}
	}
}
