using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CornellTech
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        /*
            MonoBehaviour singleton
        */
		private static T instance = null;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = (T) FindObjectOfType(typeof(T));
                }

				//might have to do a missing reference exception check here
 
                return instance;
            }
        }

		//Inherited from MonoBehaviour

		protected void SaveInstance(T objectReference)
		{
			instance = objectReference;
		}

		protected bool OtherExists()
		{
			return (Instance!=null) && (Instance!=this);
		}

    }
}
