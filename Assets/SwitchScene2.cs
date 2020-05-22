using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene2 : MonoBehaviour
{
    public Camera cam;
    public ParticleSystem snow;
    public List<GameObject> meshes;

    private bool snow_ = false;
    private Vector3 origCamPos;
    private Quaternion origCamRot;

    // Start is called before the first frame update
    void Start()
    {
        if (snow.isPlaying)
            snow.Stop();
        origCamPos = cam.transform.position;
        origCamRot = cam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 20
        };
        for (int i = 0; i < meshes.Count; i++)
        {
            if (GUI.Button(new Rect(10, 10 + 50 * i, 120, 40), meshes[i].ToString().Split(' ')[0], buttonStyle))
            {
                meshes[i].SetActive(true);
                for (int j = 0; j < meshes.Count; j++)
                    if (j != i)
                        meshes[j].SetActive(false);
            }
        }
        GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle)
        {
            fontSize = 20
        };
        toggleStyle.normal.textColor = Color.black;
        toggleStyle.hover.textColor = Color.black;
        toggleStyle.onNormal.textColor = Color.black;
        toggleStyle.onHover.textColor = Color.black;

        toggleStyle.border = new RectOffset(14, 0, 14, 0);
        toggleStyle.padding = new RectOffset(15, 0, 0, 0);
        snow_ = GUI.Toggle(new Rect(10, 10 + 50 * meshes.Count, 250, 50), snow_, "Snow effect", toggleStyle);
        if (snow_ && !snow.isPlaying)
            snow.Play();
        else if (!snow_ && snow.isPlaying)
            snow.Stop();

        GUIStyle textStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20
        };
        textStyle.normal.textColor = Color.black;
        GUI.Label(new Rect(10, 200, 150, 150), "Right click:\nRotate\nMiddle click:\nMove\nWheel:\nZoom", textStyle);

        if (GUI.Button(new Rect(10, 350, 150, 40), "Reset camera", buttonStyle))
        {
            cam.transform.SetPositionAndRotation(origCamPos, origCamRot);
        }
            
    }
}
