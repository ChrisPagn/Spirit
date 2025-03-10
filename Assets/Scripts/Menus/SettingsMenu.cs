using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// G�re les param�tres du jeu, y compris l'audio, la r�solution et le mode plein �cran.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// Mixer audio utilis� pour g�rer le volume du jeu.
    /// </summary>
    public AudioMixer audioMixer;

    /// <summary>
    /// Liste des r�solutions d'�cran disponibles.
    /// </summary>
    private Resolution[] resolutions;

    /// <summary>
    /// Dropdown pour s�lectionner la r�solution de l'�cran.
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
    /// R�f�rence � l'interface utilisateur du menu des param�tres.
    /// </summary>
    public GameObject settingsMenuUI;

    /// <summary>
    /// Initialise les param�tres du menu des options.
    /// </summary>
    public void Start()
    {
        // R�cup�ration des valeurs audio et mise � jour des sliders
        audioMixer.GetFloat("Music", out float musicValueForSlider);
        musicSlider.value = musicValueForSlider;

        audioMixer.GetFloat("Sound", out float soundValueForSlider);
        soundSlider.value = soundValueForSlider;

        // R�cup�ration des r�solutions uniques disponibles
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

        // Activation du mode plein �cran par d�faut
        Screen.fullScreen = true;
    }

    /// <summary>
    /// Change la r�solution de l'�cran en fonction de l'index s�lectionn�.
    /// </summary>
    /// <param name="resolutionIndex">Index de la r�solution s�lectionn�e dans le dropdown.</param>
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
    /// Active ou d�sactive le mode plein �cran.
    /// </summary>
    /// <param name="isFullScreen">Vrai pour activer le plein �cran, faux pour le d�sactiver.</param>
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Ferme le menu des param�tres.
    /// </summary>
    public void CloseSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
    }

    /// <summary>
    /// Supprime toutes les donn�es sauvegard�es de l'utilisateur.
    /// </summary>
    public void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
}
