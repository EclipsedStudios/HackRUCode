using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
    private GameObject spawnedController;
    public GameObject gun;

    void Start()
    {
        gun = GameObject.Find("grapplingGun");
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
                spawnedController = Instantiate(controllerPrefabs[0], transform);
        }
        else
        {
            StartCoroutine(checkForControllers());
        }
    }

    IEnumerator checkForControllers()
    {
        yield return new WaitForSeconds(5);
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
                spawnedController = Instantiate(controllerPrefabs[0], transform);
        }
        else
            StartCoroutine(checkForControllers());
    }

    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float primaryButtonValue);
        gun.GetComponent<GrapplingGun>().isBeingUsed = primaryButtonValue > 0.8f;
        targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripStrength);
        if (gripStrength > 0.8f)
        {
            gun.GetComponent<BoxCollider>().isTrigger = true;
            gun.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            gun.GetComponent<BoxCollider>().isTrigger = false;
            gun.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}