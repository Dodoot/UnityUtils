using UnityEngine;

public class ScreenShakeManager : Singleton<ScreenShakeManager>
{
    [SerializeField] Camera shakedCamera = null;

    private float _globalShakeAmount;

    private float _tempShakeAmount;
    private float _tempShakeTimer;

    public static void SetGlobalShake(float value)
    {
        _instance._globalShakeAmount = value;
    }

    public static void SetTempShake(float value, float time)
    {
        _instance._tempShakeAmount = value;
        _instance._tempShakeTimer = time;
    }

    private void Update()
    {
        var shakeAmount = _globalShakeAmount;

        if (_tempShakeTimer > 0)
        {
            _tempShakeTimer -= Time.deltaTime;

            shakeAmount += _tempShakeAmount * _tempShakeTimer;
        }
        else
        {
            _tempShakeTimer = 0f;
        }

        if (shakedCamera == null)
        {
            shakedCamera = Camera.main;
        }

        shakedCamera.transform.localPosition = new Vector3(
            Random.Range(-1f, 1f) * shakeAmount, 
            Random.Range(-1f, 1f) * shakeAmount, 
            shakedCamera.transform.localPosition.z);
    }
}