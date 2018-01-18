using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiation : MonoBehaviour {


	public Transform Mass;
	public Transform Spring;
	public List<Transform> massList = new List<Transform>();
	public List<Transform> springList = new List<Transform>();
	// Use this for initialization
	void Start () {
		for (double z = 0; z < 5; z++) {
			for (double y = 0; y < 5; y++) {
				for (double x = 0; x < 5; x++) {
					Instantiate(Mass, new Vector3((float) x, (float) (y+0.5), (float)z), Quaternion.identity);
					massList.Add (Mass);
				}


			}
		}
		int i = 0;

	}
	
	// Update is called once per frame
	void Update () {

	}
}
