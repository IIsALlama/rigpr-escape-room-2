using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    public static ContainerManager Instance;

    private Dictionary<int, ContainerSlot> containers = new Dictionary<int, ContainerSlot>();

    void Awake()
    {
        Instance = this;
    }

    public void Register(ContainerSlot container)
    {
        if (!containers.ContainsKey(container.containerID))
            containers.Add(container.containerID, container);
    }

    public ContainerSlot GetContainer(int id)
    {
        containers.TryGetValue(id, out ContainerSlot container);
        return container;
    }
}
