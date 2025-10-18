using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioMixer mainAudioMixer; // drag MainAudioMixer di sini
    public Slider volumeSlider;

    [Header("Mouse Settings")]
    public Slider sensitivitySlider;
    public Text sensitivityValueText; // optional: tampilkan angka sensi di UI

    private const string MASTER_VOLUME = "MasterVolume";
    private const string MOUSE_SENS = "MouseSensitivity";

    private float currentVolume;
    private float currentSensitivity;

    void Awake()
    {
        // 🔍 Otomatis cari slider jika belum di-assign di Inspector
        if (volumeSlider == null)
        {
            volumeSlider = GetComponentInChildren<Slider>(true); // cari slider pertama
            Debug.Log("Auto-assign VolumeSlider: " + (volumeSlider != null));
        }

        if (sensitivitySlider == null)
        {
            // Cari semua slider di anak Canvas, ambil yang bukan volumeSlider
            Slider[] sliders = GetComponentsInChildren<Slider>(true);
            foreach (var s in sliders)
            {
                if (s != volumeSlider)
                {
                    sensitivitySlider = s;
                    break;
                }
            }
            Debug.Log("Auto-assign SensitivitySlider: " + (sensitivitySlider != null));
        }
    }

    void Start()
    {
        LoadSettings();
    }

    // --- AUDIO ---
    public void OnVolumeChanged(float value)
    {
        if (mainAudioMixer != null)
            mainAudioMixer.SetFloat(MASTER_VOLUME, Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        currentVolume = value;
    }

    // --- MOUSE ---
    public void OnSensitivityChanged(float value)
    {
        currentSensitivity = value;
        PlayerPrefs.SetFloat("TempMouseSensitivity", currentSensitivity);

        if (sensitivityValueText != null)
            sensitivityValueText.text = value.ToString("F2");
    }

    // --- LOAD ---
    public void LoadSettings()
    {
        currentVolume = PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
        currentSensitivity = PlayerPrefs.GetFloat(MOUSE_SENS, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.value = currentVolume;
            OnVolumeChanged(currentVolume);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = currentSensitivity;
            OnSensitivityChanged(currentSensitivity);
        }

        Debug.Log("Settings loaded.");
    }

    // --- RESET (dipanggil dari UIButtonHandler) ---
    public void ResetUI()
    {
        volumeSlider.value = 1f;
        sensitivitySlider.value = 1f;
        OnVolumeChanged(1f);
        OnSensitivityChanged(1f);
    }

    // --- SAVE (Apply) ---
    public void ApplySettings()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME, currentVolume);
        PlayerPrefs.SetFloat(MOUSE_SENS, currentSensitivity);
        PlayerPrefs.Save();

        Debug.Log("Settings applied.");
    }
}
