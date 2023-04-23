using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBuilding : MonoBehaviour
{
    [HideInInspector]
    public bool isTarget = false;
    //Switch to FMOD for sound later
    AudioSource source;
    public float damp = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        source = GameManager.instance.gameObject.GetComponent<AudioSource>();
        GameManager.targets.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTarget) return;
        if (GameManager.instance.radioOn)
        {
            if (!source.isPlaying) source.Play();
            float d = Vector3.Distance(GameManager.instance.player.transform.position, this.transform.position);
            if (d < 30)
            {
                source.volume = 1 - d / 30;
                source.volume *= damp;
            }
            else
            {
                source.volume = 0;
            }
        }
        else
        {
            source.Stop();
        }
        
    }
}
