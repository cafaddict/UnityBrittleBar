﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class MassSpring : MonoBehaviour {

	//Let's use Thread!!!!
	Job calcJob;




	public static List<MassClass> massList = new List<MassClass>();
	public static List<SpringClass> springList = new List<SpringClass>();

	public static List<Rigidbody> RmassList = new List<Rigidbody> ();



	public static List<GameObject> mockspringList = new List<GameObject> ();

	public GameObject mockspring;

	public Rigidbody Mass;


	public Text framrate;
	public Text brittleness;
	static int q;

	static int threadExecutionCount = 0;
	static float prevTimeStamp = 0;
	static double frameCount = 0;
	static double nextUpdate = 0.0;
	static double fps = 0.0;
	static double updateRate = 4.0;  // 4 updates per sec.

//	static int num_spring = 1;

	public int num_node;
	public int num_spring;
	public float stiffness;
	public float damp;
	public float threashold;
	public float deltaTimesec;
	public float addingforce;




	void Start () {

		q = num_node;

		nextUpdate = Time.time;

		calcJob = new Job();
		calcJob.num_node = num_node;
		calcJob.stifness = stiffness/num_spring;
		calcJob.damp = damp;
		calcJob.addingforce = addingforce;
		for (double z = 0; z < q; z++) {
			for (double y = 0; y < q; y++) {
				for (double x = 0; x < q; x++) {

					Vector3 startPosition = new Vector3((float) x, (float) (y+0.5), (float)z);
					Rigidbody rMass = Instantiate(Mass, startPosition, Quaternion.identity);
					RmassList.Add (rMass);
					calcJob.InMass.Add(new MassClass((float)0.5,startPosition,deltaTimesec));

				}


			}
		}


/*
        @-@-@
        
        @-@-@
        
        @-@-@
*/		for (int n = 0; n<num_spring;n++)
		{
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if (i % q != q - 1) {
	//				springList.Add (new SpringClass(massList[i],massList[i+1]));
					calcJob.InSpring.Add(new SpringClass(calcJob.InMass[i],calcJob.InMass[i+1],threashold));
	//				mockspringList.Add(Instantiate(mockspring));
				}


			}

	/*
	        @ @ @
			1 1 1
	        @ @ @
	        1 1 1
	        @ @ @
	*/
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if (!((i % (q * q) >= q * q - q) & (i % (q * q) <= q * q - 1))) {
	//				springList.Add (new SpringClass (massList [i], massList [i + q]));
					calcJob.InSpring.Add(new SpringClass(calcJob.InMass[i],calcJob.InMass[i+q],threashold));
	//				mockspringList.Add(Instantiate (mockspring));

				}
			}
			//위로 
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if (!((i % (q * q * q) >= q * q * q - q * q) & (i % (q * q * q) <= q * q * q - 1))) {
	//				springList.Add (new SpringClass (massList [i], massList [i + q * q]));
					calcJob.InSpring.Add(new SpringClass(calcJob.InMass[i],calcJob.InMass[i+q*q],threashold));
	//				mockspringList.Add(Instantiate (mockspring));
				}
			}
	/*
	        @ @ @
	         / /
	        @ @ @
	         / /
	        @ @ @
	*/
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if ((i % q != q - 1) & (!(i % (q * q) >= q * q - q))) {
					calcJob.InSpring.Add(new SpringClass(calcJob.InMass[i],calcJob.InMass[i+q+1],threashold));
				}
				
			}


	/*
	        @ @ @
	         \ \
	        @ @ @
	         \ \
	        @ @ @
	*/

			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if ((i % q != 0) & (!(i % (q * q) >= q * q - q))) {
					calcJob.InSpring.Add(new SpringClass(calcJob.InMass[i],calcJob.InMass[i+q-1],threashold));
				}
			}

			//front up & right
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if ((i % q != q - 1) & (!(i % (q * q * q) >= q * q * q - q * q))) {
					calcJob.InSpring.Add(new SpringClass(calcJob.InMass[i],calcJob.InMass[i+q*q+1],threashold));
				}
			}
			//front up & left
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if ((i % q != 0) & (!(i % (q * q * q) >= q * q * q - q * q))) {
					calcJob.InSpring.Add(new SpringClass(calcJob.InMass[i],calcJob.InMass[i+q*q-1],threashold));
				}
			}

			//side up & back
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if ((!(i % (q * q) >= q * q - q)) & (!(i % (q * q * q) >= q * q * q - q * q))) {
					calcJob.InSpring.Add (new SpringClass (calcJob.InMass [i], calcJob.InMass [i + q * q + q],threashold));
				}
				
				
			}

			//side up & front
			for (int i = 0; i < calcJob.InMass.Count; i++) {
				if ((!(i % (q * q) < q)) & (!(i % (q * q * q) >= q * q * q - q * q))) {
					calcJob.InSpring.Add (new SpringClass (calcJob.InMass [i], calcJob.InMass [i + q * q - q],threashold));
				}


			}
	//
	//		//up & right & back
	//		for (int i = 0; i < calcJob.InMass.Count; i++) {
	//			if ((i % q != q - 1) & (!(i % (q * q) >= q * q - q)) & (!(i % (q * q * q) >= q * q * q - q * q))) {
	//				calcJob.InSpring.Add (new SpringClass (calcJob.InMass [i], calcJob.InMass [i + q * q + q + 1]));
	////				Debug.DrawLine (calcJob.InMass [i].position, calcJob.InMass [i + q * q + q + 1].position,Color.red,1000f);
	//			}
	//		}
	//
	//		//		//up & left & front
	//		for (int i = 0; i < calcJob.InMass.Count; i++) {
	//			if ((i % q != 0) & (!(i % (q * q) <  q)) & (!(i % (q * q * q) >= q * q * q - q * q))) {
	//				calcJob.InSpring.Add (new SpringClass (calcJob.InMass [i], calcJob.InMass [i + q * q - q - 1]));
	//				//				Debug.DrawLine (calcJob.InMass [i].position, calcJob.InMass [i + q * q - q - 1].position,Color.red,1000f);
	//			}
	//		}
	//
	//		//		//up & right & front
	//		for (int i = 0; i < calcJob.InMass.Count; i++) {
	//			if ((i % q != q - 1) & (!(i % (q * q) < q)) & (!(i % (q * q * q) >= q * q * q - q * q))) {
	//				calcJob.InSpring.Add (new SpringClass (calcJob.InMass [i], calcJob.InMass [i + q * q - q + 1]));
	//				//				Debug.DrawLine (calcJob.InMass [i].position, calcJob.InMass [i + q * q - q + 1].position,Color.red,1000f);
	//
	//			}
	//		}
	////
	////		//up & left & back
	//		for (int i = 0; i < calcJob.InMass.Count; i++) {
	//			if ((i % q != 0) & (!(i % (q * q) >= q * q - q)) & (!(i % (q * q * q) >= q * q * q - q * q))) {
	//				calcJob.InSpring.Add (new SpringClass (calcJob.InMass [i], calcJob.InMass [i + q * q + q - 1]));
	////				Debug.DrawLine (calcJob.InMass [i].position, calcJob.InMass [i + q * q + q - 1].position,Color.red,1000f);
	//			}
	//		}
	//
		}


			
//		for (int i = 0; i < calcJob.InSpring.Count; i++) {
//			mockspringList.Add(Instantiate(mockspring, new Vector3(0,30,30), Quaternion.identity));
//		}
//
		calcJob.Start ();




	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit ();

		}
		frameCount++;
		float currentTimeStamp = Time.time;
		if (currentTimeStamp - prevTimeStamp > 1) {
//			print ("threadExecution count = " + calcJob.executioncount);
			framrate.text = "Frame Rate = " + calcJob.executioncount;
			brittleness.text = "Brittleness = " + (1 / (threashold * stiffness));
			prevTimeStamp = currentTimeStamp;

			calcJob.executioncount = 0;
		}

//		if (Time.time > nextUpdate)
//		{
//			nextUpdate += 1.0/updateRate;
//			fps = frameCount * updateRate;
//			frameCount = 0;
//
//		}


		for (int i = 0; i < calcJob.InMass.Count; i++) {
			RmassList[i].MovePosition(calcJob.InMass[i].new_position);
		}
//		springList = calcJob.InSpring;

			

	}


//	private void UpdateCylinderPosition(GameObject cylinder, Vector3 beginPoint, Vector3 endPoint)
//	{
//		Vector3 offset = endPoint - beginPoint;
//		Vector3 position = beginPoint + (offset / 2.0f);
//
//		cylinder.transform.position = position;
//		cylinder.transform.LookAt(beginPoint);
//		Vector3 localScale = cylinder.transform.localScale;
////		localScale.z = (endPoint - beginPoint).magnitude;
//		localScale.z = 0.1f;
//		localScale.x = 0.1f;
//		cylinder.transform.localScale = localScale;
//	}


//	void OnDrawGizmos() {
//		Gizmos.color = Color.yellow;
//
//		for (int i = 0; i < calcJob.InSpring.Count; i++) {
//			Gizmos.DrawLine (calcJob.InSpring[i].m1.position,calcJob.InSpring[i].m2.position);
//		}
////		Gizmos.DrawSphere(transform.position, 1);
//	}

//	public static void restart() {
//		string[] endings = new string[]{
//			"exe", "x86", "x86_64", "app"
//		};
//		string executablePath = Application.dataPath + "/..";
//		foreach (string file in System.IO.Directory.GetFiles(executablePath)) {
//			foreach (string ending in endings) {
//				if (file.ToLower ().EndsWith ("." + ending)) {
//					System.Diagnostics.Process.Start (executablePath + file);
//					Application.Quit ();
//					return;
//				}
//			}
//
//		}
//	}

}
