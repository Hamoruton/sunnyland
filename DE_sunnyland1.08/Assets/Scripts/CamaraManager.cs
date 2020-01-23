using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraManager : MonoBehaviour
{
    Camera camera;

    private float width = 1920f;
    private float height = 1080f;
    private float pixelPerUnit = 100f;

    void Awake()
    {
        float aspect = (float)Screen.height / (float)Screen.width;
        float idealAspect = height / width;
        camera = GetComponent<Camera>();

        camera.orthographicSize = (height / 2f / pixelPerUnit);
        if (idealAspect > aspect)
        {
            //画面が横に大きい時の倍率変更
            float idealScale = height / Screen.height;
            float cameraWidth = width / (Screen.width * idealScale);
            camera.rect = new Rect((1f - cameraWidth) / 2f, 0f, cameraWidth, 1f);
        }
        else
        {
            //画面が縦に大きい時の倍率変更
            float idealScale = aspect / idealAspect;
            camera.orthographicSize *= idealScale;
            camera.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}
