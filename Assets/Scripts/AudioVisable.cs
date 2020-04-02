using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisable : MonoBehaviour
{
    private AudioSource audioSource;
    public float[] samples = new float[512];
    private float[] freqBand = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];

    private float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    public static float _Amplitude, _AmplitudeBuffer;
    private float _AmplitudeHighest;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = freqBand[i];
            }
            _audioBand[i] = (freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    private void Update()
    {
        GetSpectrumAduioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    private void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_CurrentAmplitude > _AmplitudeHighest)
            _AmplitudeHighest = _CurrentAmplitude;
        _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
    }

    private void GetSpectrumAduioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            freqBand[i] = average * 10;
        }
    }

    private void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }
            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
}