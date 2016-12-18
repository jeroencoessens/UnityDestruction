using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLightCalculation : MonoBehaviour
{
    public RotateSimple Rotater;
    private float _speed;
    private float _equalizer = 0.00108f;

    private float _timerLight = 1.0f;
    private bool _shouldRise = false;
    private Light _lightComponent;

	void Start ()
	{
        _lightComponent = GetComponent<Light>();
        _timerLight = _lightComponent.intensity;

	    _speed = Rotater.Speed;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_shouldRise)
	    {
	        _timerLight -= Time.deltaTime * _speed * _equalizer;
	    }
	    else
	    {
            _timerLight += Time.deltaTime * _speed * _equalizer;
        }

        if (_timerLight < 0.25f)
	    {
	        _shouldRise = true;
	    }

	    if (_timerLight > 1.15f)
	    {
	        _shouldRise = false;
	    }

        _lightComponent.intensity = _timerLight;
	}
}
