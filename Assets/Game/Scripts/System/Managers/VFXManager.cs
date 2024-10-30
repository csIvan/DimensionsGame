using UnityEngine;
using System;


[System.Serializable]
public class VFX {
    public string name;
    public GameObject prefab;
    public float duration;
    public bool loop;
}


public class VFXManager : MonoBehaviour {

    public static VFXManager Instance;


    public VFX[] effects;

    // --------------------------------------------------------------------
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
    }


    // --------------------------------------------------------------------
    public void Play(string name, Vector3 position, Quaternion rotation) {
        VFX effect = Array.Find(effects, e => e.name == name);
        if (effect == null) {
            Debug.Log("VFX: " + name + " not found.");
            return;
        }

        GameObject newEffect = Instantiate(effect.prefab, position, rotation);
        Destroy(newEffect, effect.duration);
    }
}
