using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEditor;

public class Yaogan : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Transform gun;
    public Transform Body;
    public float maxRange = 1.3f;
    Vector3 v2, gunv2;
    Vector3 movedic;
    bool flag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        v2 = transform.position;
        gunv2 = gun.position;
        flag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        movedic = transform.position - v2;
        transform.localPosition = Vector3.ClampMagnitude(movedic, 60f);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
    }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.streamingAssetsPath);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (flag)
        {

            float depth = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 center =
                new Vector3(Screen.width / 2, Screen.height / 2, depth);
            Vector3 mouse =
                new Vector3(transform.position.x, transform.position.y, depth);

            // Transform to world space
            center = Camera.main.ScreenToWorldPoint(center);
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            Vector3 mouseVec = Vector3.ClampMagnitude(movedic, maxRange);

            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useLayerMask = true;
            contactFilter2D.layerMask = LayerMask.GetMask("Default");
            Collider2D[] colliders = new Collider2D[5];
            if (gun.GetComponent<Rigidbody2D>().OverlapCollider(contactFilter2D, colliders) > 0)
            {

                Vector3 v1 = gun.position - Body.position;
                Body.GetComponent<Rigidbody2D>().AddForce((v1 - mouseVec) * 100);
                Body.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(
                    Body.GetComponent<Rigidbody2D>().velocity, 6);
            }

            Vector3 newHammerPos = Body.position + mouseVec;
            Vector3 hammerMoveVec = newHammerPos - gun.position;
            newHammerPos = gun.position + hammerMoveVec * 0.2f;
            gun.GetComponent<Rigidbody2D>().MovePosition(newHammerPos);
            gun.rotation = Quaternion.FromToRotation(
               Vector3.right, newHammerPos - Body.position);
        }
    }
}
