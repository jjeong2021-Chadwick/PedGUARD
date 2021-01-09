using UnityEngine;
using System.Collections;
using TensorFlow;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class SimpleNetController : MonoBehaviour
{
    [SerializeField]
    TextAsset graphModel;

    [SerializeField]
    Texture2D testImage;

    [SerializeField]
    Texture2D testSample;

    [SerializeField]
    CameraController cameraController;

    [SerializeField]
    Text infoText;

    [SerializeField]
    List<RawImage> boxes;

    [SerializeField]
    Slider slider;

    [SerializeField]
    Button runBtn;
    Text btnText;

    CameraImageSplitter splitter;

    TFGraph graph;
    TFSession session;
    TFTensor input;
    float[,,,] inputData = new float[32, SettingsController.REF_SIZE, SettingsController.REF_SIZE, 3];
    bool isInitialized = false;

    // Use this for initialization
    void Start()
    {
        TensorFlowSharp.Android.NativeBinding.Init();
        infoText.text = "LOADING...";

        graph = new TFGraph();
        graph.Import(graphModel.bytes);

        session = new TFSession(graph);
        input = new TFTensor(inputData);

        splitter = new CameraImageSplitter();

        infoText.text = "LOAD COMPLETED";

        runBtn.onClick.AddListener(OnRunBtnClicked);
        btnText = runBtn.GetComponentInChildren<Text>();

        slider.onValueChanged.AddListener(OnSliderChange);
        OnSliderChange(slider.value);

        isInitialized = true;
    }

    bool showSplitted = false;
    bool usingTestImage = false;
    int target = 0;
    List<Color[]> splitted = null;
    bool detected = false;
    float expireSecond = 1.0f;
    bool run = false;
    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        if (!run)
        {
            return;
        }

        expireSecond -= Time.deltaTime;
        SetBoxColor(expireSecond / SettingsController.EXPIRE_TIME);

        if (expireSecond < 0)
        {
            expireSecond = SettingsController.EXPIRE_TIME;
            splitted = splitter.Split(cameraController.webCamTexture);

            ProcessImage(splitted);

            Run();
        }
    }

    void SetBoxColor(float alpha)
    {
        foreach (RawImage img in boxes)
        {
            img.color = new Color(
                detected ? 1.0f : 0.0f,
                detected ? 0.0f : 1.0f,
                0.0f,
                expireSecond / SettingsController.EXPIRE_TIME
            );
        }
    }

    void ProcessImage(List<Color[]> target = null)
    {
        if (target == null)
        {
            target = new List<Color[]>();
            target.Add(testSample.GetPixels());
        }
        inputData = new float[32, SettingsController.REF_SIZE, SettingsController.REF_SIZE, 3];

        int refSize = SettingsController.REF_SIZE;

        for (int i = 0; i < target.Count; i++)
        {
            Color[] pixels = target[i];
            for (int j = 0; j < pixels.Length; j++)
            {
                Color color = pixels[j];
                inputData[i, j % refSize, j / refSize, 0] = color.r * 255;
                inputData[i, j % refSize, j / refSize, 1] = color.g * 255;
                inputData[i, j % refSize, j / refSize, 2] = color.b * 255;
            }
        }

        input = new TFTensor(inputData);
    }

    void OnRunBtnClicked()
    {
        run = !run;
        btnText.text = run ? "Stop" : "Run";
    }

    void OnSliderChange(float value)
    {
        infoText.text = slider.value.ToString();
    }

    void Run()
    {
        detected = false;

        var runner = session.GetRunner();
        
        runner.AddInput(graph["input/Placeholder"][0], input);
        runner.AddInput(graph["input/Placeholder_2"][0], false);
        runner.Fetch(graph["output/ArgMax"][0]);

        var output = runner.Run();

        for (int i = 0; i < output.Length; i++)
        {
            var objs = (int[])(output[i].GetValue());
            string debug = "[";
            var count = 0;
            foreach (var obj in objs)
            {
                debug += obj + ", ";

                if (obj > 0)
                {
                    count++;
                    // detected = true;
                }
            }

            if (count / (float)(SettingsController.WIDTH_COUNT * SettingsController.HEIGHT_COUNT) >= slider.value) { detected = true; }

            debug += "]";

            Debug.Log(debug);
            Debug.Log(detected ? "DETECTED" : "NOT DETECTED");

            SetBoxColor(1.0f);
            if (detected)
            {
                Handheld.Vibrate();
            }
        }
    }
}
