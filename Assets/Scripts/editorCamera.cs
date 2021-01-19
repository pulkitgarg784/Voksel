using UnityEngine;
using System.Collections;

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

    private int mouseButton = 1; // Right button

 
      void Start() { Init(); }

    void OnEnable() { Init(); }
    
     public void Init()
    {
          initPosition = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        initRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        //create a temporary target at 'distance' from the cameras current viewpoint
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
 
    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    void LateUpdate()
    {
        // Zoom
        if (Input.GetMouseButton(mouseButton) && Input.GetKey(KeyCode.LeftAlt))
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Zoom);
            desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate*0.125f * Mathf.Abs(desiredDistance);
        }

          // Pan
        else if (Input.GetMouseButton(2)|| Input.GetMouseButton(mouseButton) && Input.GetKey(KeyCode.LeftControl))
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Pan);

            //grab the rotation of the camera so we can move in a psuedo local XY space
            target.rotation = transform.rotation;
            target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
            target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
        }
         
          // Alt + Space: Reset Camera
        else if (Input.GetKey(KeyCode.Space)&& Input.GetKey(KeyCode.LeftAlt))
        {
            transform.position = initPosition;
               transform.rotation = initRotation;
               target.transform.position = initTargetPosition;
               doInit();
        }
 
          // Orbit
        else if (Input.GetMouseButton(mouseButton))
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Orbit);

            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
 

 
            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            // set camera rotation
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;
 
            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
            transform.rotation = rotation;
        }
        else
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Default);

        }
       
        ////////Orbit Position
      
          // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate*2 * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
 
        // calculate position based on the new currentDistance
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
}
