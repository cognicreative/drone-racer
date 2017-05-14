using UnityEngine;

namespace DroneRacerFpv.Utils
{
    public static class SceneHelper
    {
        static Component SearchForChild(Component parent, string childName)
        {
            Component child = null;

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).name == childName)
                {
                    child = parent.transform.GetChild(i);
                    break;
                }
            }

            if (child == null)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    child = SearchForChild(parent.transform.GetChild(i), childName);

                    if (child != null)
                    {
                        break;
                    }
                }
            }

            return child;
        }

        public static T FindFirstChildInHierarchy<T>(Component parent, string childName)
        {
            T child = default(T);

            Component c = SearchForChild(parent, childName);

            if (c != null)
            {
                child = c.GetComponent<T>();
            }

            return child;
        }

        public static Component FindFirstChildInHierarchy(Component parent, string childName)
        {
            return SearchForChild(parent, childName);
        }

        public static void SetHierarchyActive(Component parent, bool active)
        {
            parent.gameObject.SetActive(active);

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(active);
            }

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                SetHierarchyActive(parent.transform.GetChild(i), active);
            }
        }
    }
}