using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    RawImage target;

    public WebCamTexture webCamTexture { get; private set; }

    public void SetTexture(Texture2D tex)
    {
        if (tex == null)
        {
            target.texture = webCamTexture;
        }
        else
        {
            target.texture = tex;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        string selectedName = null;

        if (devices.Length == 0)
        {
            return;
        }
        else if (devices.Length == 1)
        {
            selectedName = devices[0].name;
        }
        else
        {
            foreach (WebCamDevice device in devices)
            {
                if (!device.isFrontFacing)
                {
                    selectedName = device.name;
                }
            }
        }

        webCamTexture = new WebCamTexture(selectedName);
        target.texture = webCamTexture;
        webCamTexture.requestedFPS = 60;
        webCamTexture.Play();
    }

    private void OnDestroy()
    {
        webCamTexture.Stop();
        Destroy(webCamTexture);
        webCamTexture = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
