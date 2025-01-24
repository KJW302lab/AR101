using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageMultiTracking : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackManager;
    [SerializeField] private List<GameObject> objectList;

    private Dictionary<string, GameObject> _objectDict;

    private void Awake()
    {
        _objectDict = new();

        foreach (var prefab in objectList)
            _objectDict.Add(prefab.name, prefab);
    }

    private void OnEnable()
    {
        trackManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        trackManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var image in args.added)
            RefreshImage(image);

        foreach (var image in args.updated)
            RefreshImage(image);
    }

    private void RefreshImage(ARTrackedImage trackedImage)
    {
        var imageName = trackedImage.referenceImage.name;

        var obj = _objectDict[imageName];

        obj.transform.position = trackedImage.transform.position;
        obj.transform.rotation = trackedImage.transform.rotation;
        obj.SetActive(true);
    }
}