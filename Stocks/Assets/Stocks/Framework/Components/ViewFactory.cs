using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

namespace Framework.Components
{
    [Serializable]
    public class ViewFactory
    {
        public Transform Root;
        public GameObject Prefab;
        List<GameObject> Views = new List<GameObject>();
        Queue<GameObject> Pool = new Queue<GameObject>();

        public GameObject Insert(int index)
        {
            GameObject view;
            if (Pool.Count == 0)
            {
                view = GameObject.Instantiate(Prefab, Root);
            }
            else
            {
                view = Pool.Dequeue();
            }

            if(index > Views.Count)
            {
                index = Views.Count;
            }

            if (Application.isPlaying)
                Views.Insert(index, view);
            view.transform.SetSiblingIndex(index);
            view.SetActive(true);
            return view;
        }

        public GameObject Add()
        {
            return Insert(Views.Count);
        }

        public bool RemoveAt(int index)
        {
            if(Views.Count > index)
            {
                var view = Views[index];
                Remove(view);
                return true;
            }
            return false;
        }

        public void Remove(GameObject view)
        {
            view.SetActive(false);
            if (Application.isPlaying)
                Pool.Enqueue(view);
            Views.Remove(view);
        }

        //

        public void Clear()
        {
            if (Application.isPlaying)
            {
                for (int i = Views.Count - 1; i >= 0; i--)
                {
                    Remove(Views[i]);
                }
            }
            else
            {
                while (Root.transform.childCount > 0)
                {
                    GameObject.DestroyImmediate(Root.transform.GetChild(0).gameObject);
                }
            }
        }

        public int Count
        {
            get
            {
                return Views.Count;
            }
        }

        public GameObject this[int index]
        {
            get
            {
                return Views[index];
            }
        }

        public GameObject First
        {
            get
            {
                if (Views.Count == 0)
                    return null;
                return Views[0];
            }
        }

        public GameObject Last
        {
            get
            {
                if (Views.Count == 0)
                    return null;
                return Views[Views.Count - 1];
            }
        }
    }
}
