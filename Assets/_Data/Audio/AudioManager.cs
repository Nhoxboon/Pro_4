
using UnityEngine;

public class AudioManager : NhoxBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [SerializeField] protected bool playBGM = true;
    
    [Header("BGM Details")] [SerializeField]
    protected AudioSource[] bgm;

    protected int currentBGMIndex;

    protected override void Awake()
    {
        base.Awake();
        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
        
        if (instance != null)
        {
            DebugTool.LogError("Only one AudioManager allow to exist.");
            return;
        }

        instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBGM();
    }
    
    protected void LoadBGM()
    {
        if (bgm.Length > 0) return;
        bgm = GetComponentsInChildren<AudioSource>();
        DebugTool.Log(transform.name + " LoadBGM", gameObject);
    }

    public void PlaySFX(AudioSource sfx)
    {
        if(sfx.clip == null) return;
        if (sfx.isPlaying) sfx.Stop();
        
        sfx.Play();
    }

    protected void PlayMusicIfNeeded()
    {
        if (!playBGM) return;
        if(!bgm[currentBGMIndex].isPlaying)
            PlayRandomBGM();
    }

    [ContextMenu("Play Random BGM")]
    public void PlayRandomBGM()
    {
        currentBGMIndex = Random.Range(0, bgm.Length);
        PlayBGM(currentBGMIndex);
    }

    public void PlayBGM(int bgmToPlay)
    {
        StopAllBGM();
        
        currentBGMIndex = bgmToPlay;
        bgm[bgmToPlay].Play();
    }
    
    [ContextMenu("Stop All BGM")]
    public void StopAllBGM()
    {
        foreach (var sound in bgm)
            sound.Stop();
    }
}
