using UnityEngine;

namespace Base.Util
{
    public static class ObjectFinder
    {
        //For Better Onvalidate Reference Sets
        public static void FindObjectInChilderenWithType<T>(ref T objectToFind, Transform transform) where T : Component
        {
            objectToFind = transform.GetComponentInChildren<T>();
            if (objectToFind == null)
            {
                Debug.LogError("Failed To Set Reference On " + transform.name);
            }
        }
        public static void FindObjectInChilderenWithName<T>(ref T objectToFind, Transform transform, string name) where T : Component
        {
            Transform tempTransform = transform.Find(name);
            if (tempTransform)
            {
                objectToFind = tempTransform.GetComponent<T>();
                if (objectToFind == null)
                {
                    Debug.LogError("Failed To Set Reference On " + transform.name);
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    FindObjectInChilderenWithName(ref objectToFind, transform.GetChild(i), name);
                    if (objectToFind != null) return;
                }
            }
        }
    }
}