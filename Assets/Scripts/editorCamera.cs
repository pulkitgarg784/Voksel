﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Editor Tools/Editor Free Camera")]

public class editorCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private Quaternion initRotation;
    private Vector3 initPosition;
    private Vector3 initTargetPosition;

    //control scheme    
    public  enum Scheme {Blender,Unity};
    public Scheme currentControlScheme;
    public Dropdown schemeDropdown;

    void Start()
    {
        Init();
        currentControlScheme = Scheme.Blender;
    }

    void OnEnable() { Init(); }
    
     public void Init()
    {
        initPosition = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        initRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        //temporary target
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
               initTargetPosition = go.transform.position;
            target = go.transform;
        }
          doInit();
    }
    
     private void doInit() {
        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;
 
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right );
        yDeg = Vector3.Angle(Vector3.up, transform.up );
     }

    void LateUpdate()
    {
        
        // blender zoom
        if (currentControlScheme == Scheme.Blender && Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftControl))
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Zoom);
            desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate*0.125f * Mathf.Abs(desiredDistance);
        }

        // Pan(unity: mmb, blender: shift+mmb)
        else if (currentControlScheme == Scheme.Unity && Input.GetMouseButton(2)||currentControlScheme == Scheme.Blender && Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftShift))
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Pan);
            target.rotation = transform.rotation;
            target.Translate(Vector3.right * (-Input.GetAxis("Mouse X") * panSpeed));
            target.Translate(transform.up * (-Input.GetAxis("Mouse Y") * panSpeed), Space.World);
        }
        
        // Orbit(Unity:RMB, Blender: MMB)
        else if (currentControlScheme == Scheme.Unity && Input.GetMouseButton(1) || currentControlScheme == Scheme.Blender && Input.GetMouseButton(2) )
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Orbit);


            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
 

 
            //Clamp vertical axis
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;
 
            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
            transform.rotation = rotation;
        }
        else
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Default);

        }


        // Zoom according to scroll
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate*4 * Mathf.Abs(desiredDistance);
        //clamp zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
      
    }


    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        if(target!=null){
            Gizmos.DrawSphere(target.position, 1);
        }
    }

    public void setControlScheme()
    {
        if (schemeDropdown.value == 0)
        {
            currentControlScheme = Scheme.Blender;
        }
        if (schemeDropdown.value == 1)
        {
            currentControlScheme = Scheme.Unity;
        }
    }
}
