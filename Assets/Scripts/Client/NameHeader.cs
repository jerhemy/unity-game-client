using UnityEngine;
using System.Collections;
using TMPro;

public class NameHeader : MonoBehaviour {
 
    public string name;
    public Camera cam;
    TextMesh texto;
 
// Use this for initialization
    void Start () {
        cam = Camera.main;
        texto = GetComponentInChildren<TextMesh>();
        texto.text = "<"+name+">";
    }
 
// Update is called once per frame
    void Update () {
        if (cam != null) {
            transform.LookAt (cam.transform);
        }else
        {
            cam = Camera.main;
        }
    }
}