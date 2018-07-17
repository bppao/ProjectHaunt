using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] private float m_CameraMoveSpeed;

    private Camera m_MainCamera;
    private GameController m_GameController;
    private LevelManager m_LevelManager;
    private Vector3 m_CameraOffset;
    private List<Transform> m_Characters = new List<Transform>();
    private List<string> m_CharacterDescriptions;
    private int m_CurrentCharacterSelection;
    private Text m_CharacterNameText;
    private Text m_CharacterDescriptionText;

    private const string KNIGHT_DESCRIPTION = "KNIGHT DESCRIPTION GOES HERE";
    private const string WIZARD_DESCRIPTION = "WIZARD DESCRIPTION GOES HERE";
    private const string RANGER_DESCRIPTION = "RANGER DESCRIPTION GOES HERE" + "\n\nNOT AVAILABLE!";
    private const string BARBARIAN_DESCRIPTION = "BARBARIAN DESCRIPTION GOES HERE" +"\n\nNOT AVAILABLE!";

    private void Start()
    {
        m_MainCamera = Camera.main;
        m_GameController = FindObjectOfType<GameController>();
        m_LevelManager = FindObjectOfType<LevelManager>();

        // Find all of the character transforms and store them
        Transform knight = GameObject.Find("KnightModel").transform;
        if (knight != null)
        {
            m_Characters.Add(knight);
        }

        Transform wizard = GameObject.Find("WizardModel").transform;
        if (wizard != null)
        {
            m_Characters.Add(wizard);
        }

        Transform ranger = GameObject.Find("RangerModel").transform;
        if (ranger != null)
        {
            m_Characters.Add(ranger);
        }

        Transform barbarian = GameObject.Find("BarbarianModel").transform;
        if (barbarian != null)
        {
            m_Characters.Add(barbarian);
        }

        // Initialize the camera offset based off of the knight which is the
        // default character selection
        m_CameraOffset = m_MainCamera.transform.position - knight.position;

        // Find the text objects that will update with the character selection
        m_CharacterNameText = GameObject.Find("CharacterClassName").GetComponent<Text>();
        m_CharacterDescriptionText = GameObject.Find("CharacterClassDescription").GetComponent<Text>();

        // Fill the character description list
        m_CharacterDescriptions = new List<string>()
        {
            KNIGHT_DESCRIPTION,
            WIZARD_DESCRIPTION,
            RANGER_DESCRIPTION,
            BARBARIAN_DESCRIPTION
        };

        UpdateSelectionText();
    }

    private void Update()
    {
        // Left and right arrow keys change selection
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Can't select any further left
            if (m_CurrentCharacterSelection == 0) return;

            m_CurrentCharacterSelection--;
            UpdateSelectionText();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Can't select any further right
            if (m_CurrentCharacterSelection == m_Characters.Count - 1) return;

            m_CurrentCharacterSelection++;
            UpdateSelectionText();
        }

        // Enter key confirms selection
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // TODO: Implement ranger and barbarian classes
            if (m_GameController.SelectedCharacterClass == "Ranger" ||
                m_GameController.SelectedCharacterClass == "Barbarian") return;

            // Load the main game level which is the next one in the build settings
            m_LevelManager.LoadNextLevel();
        }

        MoveCameraToSelection();
    }

    private void MoveCameraToSelection()
    {
        m_MainCamera.transform.position = Vector3.Lerp(
            m_MainCamera.transform.position,
            m_Characters[m_CurrentCharacterSelection].position + m_CameraOffset,
            m_CameraMoveSpeed * Time.deltaTime);
    }

    private void UpdateSelectionText()
    {
        // Get the character class name from the model transform name and strip the "model"
        m_GameController.SelectedCharacterClass =
            m_Characters[m_CurrentCharacterSelection].name.Replace("Model", "");
        m_CharacterNameText.text = m_GameController.SelectedCharacterClass;

        // Get the appropriate character class description
        m_CharacterDescriptionText.text = m_CharacterDescriptions[m_CurrentCharacterSelection];
    }
}
