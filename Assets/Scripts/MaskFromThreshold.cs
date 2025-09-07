using UnityEngine;

public class MaskFromThreshold : MonoBehaviour
{
    public RenderTexture sourceCameraRT;  // WebcamFeed.cameraRT 넣기
    public RenderTexture maskRT;          // 출력 (흑/백)
    [Range(0,1)] public float threshold = 0.5f;
    public bool flipX = true;

    Material _mat;

    void Start()
    {
        if (maskRT == null && sourceCameraRT != null)
            maskRT = new RenderTexture(sourceCameraRT.width, sourceCameraRT.height, 0, RenderTextureFormat.R8);

        _mat = new Material(Shader.Find("Unlit/ThresholdMask"));
    }

    void Update()
    {
        if (_mat == null || sourceCameraRT == null || maskRT == null) return;
        _mat.SetFloat("_Threshold", threshold);
        _mat.SetFloat("_FlipX", flipX ? 1f : 0f);
        Graphics.Blit(sourceCameraRT, maskRT, _mat);
    }

    void OnDestroy(){ if (_mat) Destroy(_mat); }
}
