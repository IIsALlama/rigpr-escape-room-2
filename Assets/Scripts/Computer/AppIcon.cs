using UnityEngine;
using UnityEngine.EventSystems;

public class AppIcon : MonoBehaviour, IPointerClickHandler
{
    public int appID;

    public void OnPointerClick(PointerEventData eventData)
    {
        AppManager.instance.OpenApp(appID);
    }
}
