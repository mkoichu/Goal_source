using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Assets.Scripts.General;
using MainDefinitions;

public class CameraShake : MonoBehaviour
{

    private float magnitude = 0.2f;
    private float magIncreaseRate = 0.0001f;
    private bool shake_off = false;


    // Start is called before the first frame update
    private void Start()
    {
        // Bus.OnGameOver.Subscribe(this, () => StartCoroutine(ShakeOFF()));
        // Bus.StartARTimeEffect.Subscribe(this, type =>
        // {
            /*if (type == ARTimeEffectType.Lightning)
                ShakeON();
            if (type == ARTimeEffectType.SuperSaiyan)
                BoostMagnitude();*/   //REMOVED FOR DEBUG (AR + unity+)
        // });

        
    }


    IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        while(shake_off == false)
        {
            if(magnitude < 5)
            {
                magnitude += magIncreaseRate;
            }
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
           // elapsed += Time.deltaTime;
            yield return null;
        }


        transform.localPosition = originalPos;
        magnitude = 0f;
    }


    private void ShakeON()
    {
        shake_off = false;
        StartCoroutine(Shake());
    }

    IEnumerator ShakeOFF()
    {
        yield return new WaitForSeconds(2);
        shake_off = true;
    }

    private void BoostMagnitude()
    {
        magnitude += 0.5f;
    }
}
