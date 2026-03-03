using SimpleTwineDialogue;
using System.Collections;
using UnityEngine;

public class HatchDropZone : MonoBehaviour
{
    public ItemType allowedItem;
    public float deleteDelay;
    [SerializeField] private RotatingWarningLight rotatingWarningLight;

    //To dynamically set the allowed type from any script example -> trigger.SetAllowedType(ItemType.Food); (trigger is the reference to the HatchDropZone script)
    private void Awake()
    {
        rotatingWarningLight = FindAnyObjectByType<RotatingWarningLight>();
    }

    private void OnTriggerEnter(Collider other)
    {
        HoldableItem item = other.GetComponentInParent<HoldableItem>();
        if (item == null) return;


        if (item.itemType == allowedItem)
        {
            Debug.Log("Correct item dropped in hatch!");
            switch(item.itemType)
            {
                case ItemType.Food:
                    TextAdventure.instance.NextFile();
                    // Handle food item logic
                    break;
                case ItemType.Can:
                    // Handle tool bomb logic
                    break;

            }

        }
        else
        {
            Debug.Log($"Wrong item dropped in hatch, {item.itemType} Expected: {allowedItem}");
            rotatingWarningLight.Activate();
            StartCoroutine(DeleteAfterDelay(item));
        }
    }

    private IEnumerator DeleteAfterDelay(HoldableItem item)
    {
        yield return new WaitForSeconds(deleteDelay);
        if(item != null)
        {
            item.DeleteItem();
            rotatingWarningLight.Deactivate();
        }
    }

}
