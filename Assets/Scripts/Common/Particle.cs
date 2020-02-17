using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    //파티클의 정보와 제어를 담당하고 있는 클래스입니다.

    [Header("ParticleInfomation")]
    //파티클의 종류와 이름를 의미합니다.
    public ParticleKind Kind;
    public ParticleName Name;

    private Transform _transform;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        Play();
        StartCoroutine(DisableCheck());
    }
    
    public void Play()
    {
        _particleSystem.Stop();
        _particleSystem.Play();
    }

    public void Stop()
    {
        _particleSystem.Stop();
    }

    public void Pause()
    {
        _particleSystem.Pause();
    }

    public bool IsPlay()
    {
        return _particleSystem.isPlaying;
    }
    
    public bool IsPause()
    {
        return _particleSystem.isPaused;
    }

    IEnumerator DisableCheck()
    {
        while(true)
        {
            if (!_particleSystem.isPlaying) break;

            yield return new WaitForSeconds(1.0f);
        }

        gameObject.SetActive(false);
    }
}
