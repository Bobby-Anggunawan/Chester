using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSceneCode : MonoBehaviour
{

    public GameObject meshSloth;
    public GameObject meshPlant;

    // Start is called before the first frame update
    void Start()
    {
        meshPlant.GetComponent<Renderer>().sharedMaterial.SetColor("_OutlineColor", Color.red);
        meshSloth.GetComponent<Renderer>().sharedMaterial.SetColor("_OutlineColor", Color.blue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
