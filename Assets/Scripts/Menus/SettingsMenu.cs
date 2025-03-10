using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Gère les paramètres du jeu, y compris l'audio, la résolution et le mode plein écran.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// Mixer audio utilisé pour gérer le volume du jeu.
    /// </summary>
    public AudioMixer audioMixer;

    /// <summary>
    /// Liste des résolutions d'écran disponibles.
    /// </summary>
    private Resolution[] resolutions;

    /// <summary>
    /// Dropdown pour sélectionner la résolution de l'écran.
    /// </summary>
    public TMP_Dropdown resolutionDropDown;

    /// <summary>
    /// Curseur permettant d'ajuster le volume de la musique.
    /// </summary>
    public Slider musicSlider;

    /// <summary>
    /// Curseur permettant d'ajuster le volume des effets sonores.
    /// </summary>
    public Slider soundSlider;

    /// <summary>
    /// Référence à l'interface utilisateur du menu des paramètres.
    /// </summary>
    public GameObject settingsMenuUI;

    /// <summary>
    /// Initialise les paramètres du menu des options.
    /// </summary>
    public void Start()
    {
        // Récupération des valeurs audio et mise à jour des sliders
        audioMixer.GetFloat("Music", out float musicValueForSlider);
        musicSlider.value = musicValueForSlider;

        audioMixer.GetFloat("Sound", out float soundValueForSlider);
        soundSlider.value = soundValueForSlider;

        // Récupération des résolutions uniques disponibles
        resolutions = Screen.resolutions
                   .Select(resolution => new Resolution { width = resolution.width, height = resolution.height })
                   .Distinct()
                   .ToArray();

        resolutionDropDown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        // Activation du mode plein écran par défaut
        Screen.fullScreen = true;
    }

    /// <summary>
    /// Change la résolution de l'écran en fonction de l'index sélectionné.
    /// </summary>
    /// <param name="resolutionIndex">Index de la résolution sélectionnée dans le dropdown.</param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// Ajuste le volume de la musique.
    /// </summary>
    /// <param name="volume">Niveau du volume.</param>
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    /// <summary>
    /// Ajuste le volume des effets sonores.
    /// </summary>
    /// <param name="volume">Niveau du volume.</param>
    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("Sound", volume);
    }

    /// <summary>
    /// Active ou désactive le mode plein écran.
    /// </summary>
    /// <param name="isFullScreen">Vrai pour activer le plein écran, faux pour le désactiver.</param>
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Ferme le menu des paramètres.
    /// </summary>
    public void CloseSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
    }

    /// <summary>
    /// Supprime toutes les données sauvegardées de l'utilisateur.
    /// </summary>
    public void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
}
