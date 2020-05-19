using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class Reflections : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    RenderTexture texture;
 
    Material material;
    Camera reflectionCamera;


    // private void OnEnable() {
    //     RenderPipeline.beginCameraRendering += CaptureCamera;    
    // }

    // private void OnDisable() {
    //     RenderPipeline.beginCameraRendering -= CaptureCamera;
    // }

    private void Awake() {
        // texture = new RenderTexture(Screen.width / 4, Screen.height / 4, 24, RenderTextureFormat.ARGB32);

        texture.width = Screen.width / 4;
        texture.height = Screen.height /4;

        reflectionCamera = GetComponent<Camera>();
        reflectionCamera.targetTexture = texture;
        reflectionCamera.projectionMatrix = mainCamera.projectionMatrix;
        // material.SetTexture("_ReflTex", texture);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // public void CaptureCamera(ScriptableRenderContext context, Camera cam) {
    //     transform.position = mainCamera.transform.position;
    //     // UniversalRenderPipeline.RenderSingleCamera(context, reflectionCamera);
    // }

}
