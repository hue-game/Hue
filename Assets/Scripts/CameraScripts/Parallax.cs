using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
	[Range(0, 10)]	
	public float distanceFactor = 5f;

	[Range(0, 2)]
	public float speed = 1f;

	public Transform[] layers;

	private float[] _parallaxScales;
	private Vector3 _previousCameraPosition;

	// Use this for initialization
	void Start () {
		_previousCameraPosition = transform.position;
		_parallaxScales = new float[layers.Length];

		for (int i = 0; i < _parallaxScales.Length; i++) {
			_parallaxScales [i] = layers [i].position.z * distanceFactor;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		for (int i = 0; i < layers.Length; i++) {
			Vector3 Parallax = ((_previousCameraPosition - transform.position) / (_parallaxScales [i])) * speed;
			layers[i].position = layers[i].position + Parallax;
		}

		_previousCameraPosition = this.transform.position;
	}
}
