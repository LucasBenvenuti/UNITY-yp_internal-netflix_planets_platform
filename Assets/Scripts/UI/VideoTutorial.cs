using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoTutorial : MonoBehaviour
{
    public RawImage rawImage;
    public Texture[] images;
    public float delay;

    IEnumerator Start()
    {
        while (true)
        {
            for (int i = 0; i < images.Length; i++)
            {
                rawImage.texture = images[i];
                yield return new WaitForSeconds(delay);
            }
        }
    }


    /* video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "tutorial.mp4");
        video.Prepare();
        while (!video.isPrepared)
        {
            yield return null;
        }
        video.Play();
    */
}
