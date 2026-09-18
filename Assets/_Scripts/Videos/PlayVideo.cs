using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public static event Action OnVideoFinished;

    public VideoLibrary VideoLibraryData;
    private VideoPlayer _videoPlayer;
    private VideoClip _videoClip;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        if (_videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found on the GameObject.");
        }

        _videoPlayer.timeReference = VideoTimeReference.Freerun;
    }

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState newState)
    {
        switch (newState)
        {
            case HandleGameState.GameState.PreGameMenu:
                break;
            case HandleGameState.GameState.Transition:
                break;
            case HandleGameState.GameState.LevelStart:
                SelectRandomClip(1);
                AddClipToPlayer(_videoClip);
                StartCoroutine(PlayVideoCoroutine());
                break;
            case HandleGameState.GameState.Gameplay:
                break;
            case HandleGameState.GameState.GamePaused:
                break;
            case HandleGameState.GameState.Shop:

                break;
            case HandleGameState.GameState.LevelEnd:

                break;
            case HandleGameState.GameState.ChoosePowerup:

                break;
            case HandleGameState.GameState.BossFight:
                break;
            case HandleGameState.GameState.RunEnd:

                break;
            case HandleGameState.GameState.XPTally:
                break;
            case HandleGameState.GameState.GameRestart:
                break;
            case HandleGameState.GameState.GameFinished:
                break;
            case HandleGameState.GameState.Credits:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private IEnumerator PlayVideoCoroutine()
    {
        if (_videoPlayer != null)
        {
            _videoPlayer.Play();
            yield return new WaitForSecondsRealtime((float)_videoPlayer.length);
            _videoPlayer.Stop();
            OnVideoFinished?.Invoke();
        }
    }

    private void SelectRandomClip(int worldIndex)
    {
        if (VideoLibraryData != null && VideoLibraryData.World1VideoClips.Length > 0)
        {
            switch (worldIndex)
            {
                case 1:
                    _videoClip = VideoLibraryData.SelectWorld1Clip();
                    break;
                case 2:
                    _videoClip = VideoLibraryData.SelectWorld2Clip();
                    break;
                case 3:
                    _videoClip = VideoLibraryData.SelectWorld3Clip();
                    break;
                default:
                    Debug.LogWarning("Invalid world index.");
                    return;
            }
        }
        else
        {
            Debug.LogWarning("No video clips available in the VideoLibraryData.");
        }
    }

    private void AddClipToPlayer(VideoClip clip)
    {
        if (_videoPlayer != null)
        {
            _videoPlayer.clip = clip;
        }
    }
}
