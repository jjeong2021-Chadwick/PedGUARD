using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraImageSplitter
{
    public CameraImageSplitter()
    {
    }

    public List<Color[]> Split(Texture input)
    {
        Color[] pixels = GetPixels(input);
        List<Color[]> splitted = new List<Color[]>();

        int refSize = SettingsController.REF_SIZE;
        int width = SettingsController.RESIZE_WIDTH;
        int height = SettingsController.RESIZE_HEIGHT;
        int strideWidth = SettingsController.STRIDE_WIDTH;
        int strideHeight = SettingsController.STRIDE_HEIGHT;

        int numWidthSteps = SettingsController.WIDTH_COUNT;
        int numHeightSteps = SettingsController.HEIGHT_COUNT;

        for (int i = 0; i < numWidthSteps; i++)
        {
            for (int j = 0; j < numHeightSteps; j++)
            {
                Color[] split = new Color[refSize * refSize];

                int xOffset = i * strideWidth;
                int yOffset = j * strideHeight * width;

                for (int k = 0; k < refSize * refSize; k++)
                {
                    split[k] = pixels[xOffset + yOffset + k % refSize + k / refSize * width];
                }

                splitted.Add(split);
            }
        }

        return splitted;
    }

    Color[] GetPixels(Texture input)
    {
        int width = SettingsController.RESIZE_WIDTH;
        int height = SettingsController.RESIZE_HEIGHT;

        RenderTexture rt = new RenderTexture(input.width, input.height, 24);
        RenderTexture.active = rt;
        Graphics.Blit(input, rt);
        Texture2D tex = new Texture2D(width, height);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        /*
        Texture2D tex = new Texture2D(webCamTexture.width, webCamTexture.height);
        tex.SetPixels(webCamTexture.GetPixels());
        tex.Resize(SettingsController.RESIZE_WIDTH, SettingsController.RESIZE_HEIGHT, TextureFormat.RGBA32, false);
        */
        return tex.GetPixels();
    }
}
