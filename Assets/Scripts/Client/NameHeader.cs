using UnityEngine;
using System.Collections;
using TMPro;

public class NameHeader : MonoBehaviour {
 
    public string headerName;
    public Camera cam;
    TextMesh texto;
 
// Use this for initialization
    void Start () {
        cam = Camera.main;
        texto = GetComponentInChildren<TextMesh>();
        texto.text = "<"+headerName+">";
    }
 
// Update is called once per frame
    void Update () {
        if (cam) {
            transform.LookAt (cam.transform);
        }else
        {
            cam = Camera.main;
        }
    }
}