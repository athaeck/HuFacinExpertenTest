using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public GameObject[] matArray;
    public GameObject Camera;
/* 
    private void Start()
    {
        for (int i = 0; i < matArray.Length; i++)
            matArray[i].GetComponent<Material>().color = new Color(matArray[i].GetComponent<Renderer>().material.color.r, matArray[i].GetComponent<Renderer>().material.color.g, matArray[i].GetComponent<Renderer>().material.color.b, 1); //without this, the value will end at something like 0.9992367

        Camera.SetActive(false);
    } 
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartCoroutine(DoAThingOverTime(1, 0, 5));        
        }
    }
    IEnumerator DoAThingOverTime(float start, float end, float duration)
    {
        
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            for (int i = 0; i < matArray.Length; i++)
            {
                float normalizedTime = t / duration;
                //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end

                matArray[i].GetComponent<Renderer>().material.color = new Color(matArray[i].GetComponent<Renderer>().material.color.r, matArray[i].GetComponent<Renderer>().material.color.g, matArray[i].GetComponent<Renderer>().material.color.b, Mathf.Lerp(start, end, normalizedTime));
            }
            yield return null;
        }

        for (int i = 0; i < matArray.Length; i++)
        {
            matArray[i].GetComponent<Renderer>().material.color = new Color(matArray[i].GetComponent<Renderer>().material.color.r, matArray[i].GetComponent<Renderer>().material.color.g, matArray[i].GetComponent<Renderer>().material.color.b, end); //without this, the value will end at something like 0.9992367
            matArray[i].SetActive(false);
         
        }
            GameObject.Destroy(Camera);
            GameObject.Destroy(matArray[0].transform.parent.gameObject);
    }
}


