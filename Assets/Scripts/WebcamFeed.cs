using UnityEngine;

public class WebcamFeed : MonoBehaviour
{
    [Header("Output")]
    public RenderTexture cameraRT;        // 여기로 웹캠 프레임이 복사됨
    public int width = 1280;
    public int height = 720;
    public int fps = 30;

    [Header("Optional")]
    [Tooltip("특정 카메라를 쓰고 싶으면 일부 이름 입력 (예: FaceTime)")]
    public string preferredDeviceSubstring = "";  // 비워두면 기본 카메라

    WebCamTexture _webcam;
    Material _blitMat;

    void Start()
    {
        // RenderTexture 없으면 생성
        if (cameraRT == null)
            cameraRT = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
            { enableRandomWrite = false };

        // 사용할 카메라 선택 (부분 문자열로 매칭)
        string deviceName = null;
        var devices = WebCamTexture.devices;
        if (!string.IsNullOrEmpty(preferredDeviceSubstring))
        {
            foreach (var d in devices)
            {
                if (d.name.ToLower().Contains(preferredDeviceSubstring.ToLower()))
                {
                    deviceName = d.name; break;
                }
            }
        }

        _webcam = string.IsNullOrEmpty(deviceName)
            ? new WebCamTexture(width, height, fps)
            : new WebCamTexture(deviceName, width, height, fps);

        _webcam.Play();

        // 복사용 기본 머티리얼
        _blitMat = new Material(Shader.Find("Unlit/Texture"));
    }

    void Update()
    {
        if (_webcam == null || !_webcam.isPlaying) return;
        // WebCamTexture → RenderTexture로 복사
        Graphics.Blit(_webcam, cameraRT, _blitMat);
    }

    void OnDestroy()
    {
        if (_webcam != null) _webcam.Stop();
        if (_blitMat != null) Destroy(_blitMat);
    }
}
