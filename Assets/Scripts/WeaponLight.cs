using UnityEngine;
using System.Collections;

public class WeaponLight : MonoBehaviour {

    public float speed = -0.00001f;
    Light mLight;

    void Start()
    {
        mLight = GetComponent<Light>();
        Reset();
    }

    void Reset()
    {
        //transform.localPosition = new Vector3(0.0007f, 0.0046f, -0.0013f);
        transform.localPosition = Vector3.zero;
        mLight.intensity = 0;
    }

	void Update ()
    {
        Move();
	}

    void Move()
    {
        transform.localPosition += new Vector3(0, speed * Time.deltaTime);
        mLight.intensity += mLight.intensity < 8 ? 1 : 0;
        if (transform.localPosition.y <= -0.02488f)
            Reset();
    }
}
