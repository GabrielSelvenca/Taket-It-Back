using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CursorInputHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Texture2D cursorHoverWrite;
    public Texture2D cursorHoverClick;
    public Vector2 hotspot = new Vector2(16, 16);
    public CursorMode cursorMode = CursorMode.Auto;

    public AudioClip stampSound;
    public AudioClip writeSound;

    private AudioSource audioSource;
    public Texture2D cursorDefault;

    private bool isInputField;

    void Start()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, cursorMode);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInputField = GetComponent<TMP_InputField>() != null || GetComponentInChildren<TMP_InputField>() != null;

        if (isInputField)
            Cursor.SetCursor(cursorHoverWrite, hotspot, cursorMode); //GABRIEL, ESSA LINHA NÃO ESTÁ FUNCIONANDO, USA O SEU CHAT GPT PAGO OU SEU CÉREBRO ALTAMENTE DESENVOLVIDO
        else
            Cursor.SetCursor(cursorHoverClick, hotspot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, cursorMode);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInputField && writeSound != null)
            audioSource.PlayOneShot(writeSound);
        else if (!isInputField && stampSound != null)
            audioSource.PlayOneShot(stampSound);
    }
}
