using System.Collections;
using UnityEngine;

public class DisplayDialogue : MonoBehaviour
{
    [Tooltip("The text that will be displayed")]
    [TextArea]
    public string message;
    [Tooltip("The name of the person talking")]
    [TextArea]
    public string author;
    [Tooltip("Prefab for the message")]
    public PoolObjectDef messagePrefab;
    [Tooltip("Delay before displaying the message")]
    public float delayBeforeShowing;

    
    float m_InitTime = float.NegativeInfinity;

    public bool autoDisplayOnAwake;
    bool m_WasDisplayed;
    DisplayDialogueManager m_DisplayMessageManager;
    
    private NotificationToast notification;

    void OnEnable()
    {
        m_InitTime = Time.time;
        if (m_DisplayMessageManager == null)
            m_DisplayMessageManager = FindObjectOfType<DisplayDialogueManager>();
        
        DebugUtility.HandleErrorIfNullFindObject<DisplayDialogueManager, DisplayMessage>(m_DisplayMessageManager, this);


        m_WasDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!autoDisplayOnAwake) return;
        if (m_WasDisplayed) return;
        
        if (Time.time - m_InitTime > delayBeforeShowing) Display();
    }
    
    public void Display()
    {
        notification = messagePrefab.getObject(true,m_DisplayMessageManager.DialogueRect.transform).GetComponent<NotificationToast>();
        
        notification.Initialize(message);
        
        m_DisplayMessageManager.DialogueRect.UpdateTable(notification.gameObject);

        m_WasDisplayed = true;

        StartCoroutine(messagePrefab.ReturnWithDelay(notification.gameObject,notification.TotalRunTime));

    }

   
}
