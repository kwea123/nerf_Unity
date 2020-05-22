using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DepthRender : MonoBehaviour
{
    public Camera cam;
    public int width, height;
    public List<string> image_paths, depth_paths, names;
    public GameObject cube;

    private int textureSet=0;
    private Texture2D[] itexs, dtexs;

    IEnumerator GetTex(string path, int type, int c)
    {
        UnityWebRequest www = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, path));
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            if (type == 0) // image
            {
                itexs[c].LoadImage(results);
            }
            else // depth
            {
                dtexs[c].LoadRawTextureData(results);
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        Color color = itexs[c].GetPixel(i, j);
                        color.a = dtexs[c].GetPixel(i, j).r; // assign alpha as depth value (in 0~1 NDC)
                        itexs[c].SetPixel(i, j, color);
                    }
                }
                itexs[c].Apply();
                textureSet += 1;
            }
        }
    }

    void Start()
    {
        cam.depthTextureMode |= DepthTextureMode.Depth;
        itexs = new Texture2D[image_paths.Count];
        dtexs = new Texture2D[image_paths.Count];

        for (int c = 0; c < image_paths.Count; c++)
        {
            itexs[c] = new Texture2D(width, height);
            dtexs[c] = new Texture2D(width, height, TextureFormat.RFloat, false);

            StartCoroutine(GetTex(image_paths[c], 0, c));
            StartCoroutine(GetTex(depth_paths[c], 1, c));
        }
    }

    void Update()
    {
        if (textureSet >= 1)
        {
            GetComponent<Renderer>().material.SetTexture("_MainTex", itexs[0]);
            textureSet = 0;
        }
        float z = Input.GetAxis("Mouse ScrollWheel");
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        cube.transform.position += 5 * z * Vector3.forward;
        cube.transform.position += 0.25f * x * Vector3.right;
        cube.transform.position += 0.25f * y * Vector3.up;

        if (Input.GetMouseButtonDown(2))
        {
            cube.transform.position = Vector3.zero;
        }
    }

    void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 20
        };
        for (int c = 0; c < image_paths.Count; c++)
        {
            if (GUI.Button(new Rect(10, 10 + 50 * c, 120, 40), names[c], buttonStyle))
            {
                GetComponent<Renderer>().material.SetTexture("_MainTex", itexs[c]);
            }
        }

        GUIStyle textStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20
        };
        textStyle.normal.textColor = Color.black;
        GUI.Label(new Rect(10, 200, 150, 250), "Mouse:\nMove ball\nWheel:\nForward/\nBackward\nMiddle click:\nReset ball", textStyle);

    }
}