using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "VideoLibraryData", menuName = "Scriptable Objects/Video/VideoLibraryData")]
public class VideoLibrary : ScriptableObject
{
    public VideoClip[] World1VideoClips;
    public VideoClip[] World2VideoClips;
    public VideoClip[] World3VideoClips;

    public VideoClip SelectWorld1Clip()
    {
        return World1VideoClips[Random.Range(0, World1VideoClips.Length)];
    }

    public VideoClip SelectWorld2Clip()
    {
        return World2VideoClips[Random.Range(0, World2VideoClips.Length)];
    }

    public VideoClip SelectWorld3Clip()
    {
        return World3VideoClips[Random.Range(0, World3VideoClips.Length)];
    }
}
