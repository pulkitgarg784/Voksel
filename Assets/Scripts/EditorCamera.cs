using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Editor Tools/Editor Free Camera")]

public class EditorCamera : MonoBehaviour
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

    private float _xDeg = 0.0f;
    private float _yDeg = 0.0f;
    private float _currentDistance;
    private float _desiredDistance;
    private Quaternion _currentRotation;
    private Quaternion _desiredRotation;
    private Quaternion _rotation;
    private Vector3 _position;
    private Quaternion _initRotation;
    private Vector3 _initPosition;
    private Vector3 _initTargetPosition;

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
        _initPosition = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        _initRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        //temporary target
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
               _initTargetPosition = go.transform.position;
            target = go.transform;
        }
          DoInit();
    }
    
     private void DoInit() {
        distance = Vector3.Distance(transform.position, target.position);
        _currentDistance = distance;
        _desiredDistance = distance;
 
        _position = transform.position;
        _rotation = transform.rotation;
        _currentRotation = transform.rotation;
        _desiredRotation = transform.rotation;

        _xDeg = Vector3.Angle(Vector3.right, transform.right );
        _yDeg = Vector3.Angle(Vector3.up, transform.up );
     }

    void LateUpdate()
    {
        
        // blender zoom
        if (currentControlScheme == Scheme.Blender && Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftControl))
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Zoom);
            _desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate*0.125f * Mathf.Abs(_desiredDistance);
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


            _xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            _yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
 

 
            //Clamp vertical axis
            _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
            _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
            _currentRotation = transform.rotation;
 
            _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * zoomDampening);
            transform.rotation = _rotation;
        }
        else
        {
            cursorManager.Instance.setCursor(cursorManager.CursorType.Default);

        }


        // Zoom according to scroll
        _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate*4 * Mathf.Abs(_desiredDistance);
        //clamp zoom min/max
        _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * zoomDampening);
        
        _position = target.position - (_rotation * Vector3.forward * _currentDistance + targetOffset);
        transform.position = _position;
      
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

    public void SetControlScheme()
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
