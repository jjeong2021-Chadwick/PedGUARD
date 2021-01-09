using UnityEngine;
using System.Collections;
using System;

public class SettingsController : MonoBehaviour
{
    public static int REF_SIZE = 224;
    public static float EXPIRE_TIME = 5.0f;
    public static float SENSITIVITY = 0.6f;
    public static int STRIDE_WIDTH = 224;
    public static int STRIDE_HEIGHT = 224;
    public static int WIDTH_COUNT = 5;
    public static int HEIGHT_COUNT = 5;

    public static int RESIZE_WIDTH = (WIDTH_COUNT - 1) * STRIDE_WIDTH + REF_SIZE;
    public static int RESIZE_HEIGHT = (HEIGHT_COUNT - 1) * STRIDE_HEIGHT + REF_SIZE;

    [Range(64, 256)]
    [SerializeField]
    int refSize = REF_SIZE;

    [Range(0, 5)]
    [SerializeField]
    float expireTime = EXPIRE_TIME;

    [Range(0, 1)]
    [SerializeField]
    float sensitivity = SENSITIVITY;

    [SerializeField]
    Vector2Int stride = new Vector2Int(STRIDE_WIDTH, STRIDE_HEIGHT);

    [SerializeField]
    Vector2Int windowCount = new Vector2Int(WIDTH_COUNT, HEIGHT_COUNT);

    void Start() { Set(); }

    void Update() { Set(); }

    void Set()
    {
        REF_SIZE = refSize;
        EXPIRE_TIME = expireTime;
        SENSITIVITY = sensitivity;
        STRIDE_WIDTH = Math.Max(stride.x, 1);
        STRIDE_HEIGHT = Math.Max(stride.y, 1);
        WIDTH_COUNT = Math.Max(windowCount.x, 1);
        HEIGHT_COUNT = Math.Max(windowCount.y, 1);

        RESIZE_WIDTH = (WIDTH_COUNT - 1) * STRIDE_WIDTH + REF_SIZE;
        RESIZE_HEIGHT = (int)(((HEIGHT_COUNT - 1) * STRIDE_HEIGHT + REF_SIZE) * 1.0f);
    }
}
