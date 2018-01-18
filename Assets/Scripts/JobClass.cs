using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class Job : ThreadedJob
{
	public Vector3[] InData;  // arbitary job data
	public Vector3[] OutData; // arbitary job data

	public int executioncount = 0;


	public List<MassClass> InMass = new List<MassClass>();
	public List<MassClass> OutMass;

	public List<SpringClass> InSpring = new List<SpringClass>();
	public List<SpringClass> OutSpring;



	private void letsupdate()
	{
		for (int i = 0; i < InMass.Count; i++) {
			if ((i % 10) < (10 / 2)) {
				InMass [i].massaddForce (new Vector3 ((float)-80, 0, 0));
			} 
			else {
				InMass[i].massaddForce (new Vector3 (80,0,0));
			}
			InMass [i].massupdate ();
			//			Debug.Log ("MassForce = " + InMass [i].massforce);

		}

		for (int i = 0; i < InSpring.Count; i++) {
			SpringClass spring = InSpring [i];

			if (spring.cutted == false) {
				MassClass m1 = spring.m1;
				MassClass m2 = spring.m2;
				spring.updateValues ();

				Vector3 dir12 = spring.springdir;
				float o_length = spring.original_spring_length;
				float n_length = spring.new_spring_length;
				float cte = 3 * (n_length - o_length);

				Vector3 force1;
				force1.x = cte * dir12.x;
				force1.y = cte * dir12.y;
				force1.z = cte * dir12.z;

				Vector3 force2 = -force1;

				m1.massforce.x = -force1.x - 1 * m1.velocity.x;
				m1.massforce.y = -force1.y - 1 * m1.velocity.y;
				m1.massforce.z = -force1.z - 1 * m1.velocity.z;

				m2.massforce.x = -force2.x - 1 * m2.velocity.x;
				m2.massforce.y = -force2.y - 1 * m2.velocity.y;
				m2.massforce.z = -force2.z - 1 * m2.velocity.z;

				m1.massupdate ();
				m2.massupdate ();

				if (n_length > spring.threashold) {
					spring.cut ();
				}



			}
		}
	}

	protected override void ThreadFunction()
	{
//		Debug.Log ("InMass.Count = " + InMass.Count);
		while (true) {
			letsupdate();
			executioncount++;
		}

//

		// Do your threaded task. DON'T use the Unity API here
//		while (true) {
//			Debug.Log("I'm Here");
//			for (int i = 0; i < 50; i++)
//			{
//				InData [0] = new Vector3 (i, 0, 0);
//				Thread.Sleep (10);
////				OutData[i % OutData.Length] += InData[(i+1) % InData.Length];
//			}
//		}


		

	}
	protected override void OnFinished()
	{
//		this.Start ();
		// This is executed by the Unity main thread when the job is finished
//		foreach (MassClass mass in InMass) {
//			mass.Mass.MovePosition (mass.new_position);
//		}
//		for (int i = 0; i < InData.Length; i++)
//		{
//			Debug.Log("Results(" + i + "): " + InData[i]);
//		}
	}


}