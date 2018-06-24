using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent ]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    float direction = 1;
    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (period == 0) return;

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float sin = Mathf.Sin(cycles * tau);
        float movementFactor = (sin / 2) + (1f / 2f);
        Vector3 offset = movementVector * movementFactor;
        transform.position = startPos + offset;
	}
}
