using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
[RequireComponent(typeof(PostProcessVolume))]
public class Nightvisioncontroller : MonoBehaviour
{
    [SerializeField] private Color defaultLightColour;
    [SerializeField] private Color boosterLightColour;

    private bool isNightVisionEnableed;

    private PostProcessVolume volume;
    // Start is called before the first frame update
    private void Start()
    {
        RenderSettings.ambientLight = defaultLightColour;

        volume = gameObject.GetComponent<PostProcessVolume>();
        RenderSettings.ambientLight = boosterLightColour;
        volume.weight =1;
    }

    // Update is called once per frame
   
}
