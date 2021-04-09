using UnityEngine;

public class BackgroundAudio : MonoBehaviour {
    public AudioSource backgroundAudio;
    
    // Start is called before the first frame update
    public void Start()
    {
        var objs = GameObject.FindGameObjectsWithTag("backgroundAudio");
        if (objs.Length < 1) {
            backgroundAudio.tag = "backgroundAudio";
            backgroundAudio.Play();
            DontDestroyOnLoad(backgroundAudio);
        }

    }

}
