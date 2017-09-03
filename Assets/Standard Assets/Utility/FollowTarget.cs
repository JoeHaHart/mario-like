using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);


        private void LateUpdate()
        {
            Vector3 pos =transform.position;
            
            Vector3 allPos = target.position + offset;
            pos.x = allPos.x;
            transform.position = pos;
        }
    }
}
