// Adapted from https://sharpcoderblog.com/blog/third-person-camera-in-unity-3d

using UnityEngine;

public class SC_CameraCollision : MonoBehaviour
{
    public Transform referenceTransform;
    private float collisionOffset = 0.3f; //To prevent Camera from clipping through Objects
    private float cameraSpeed = 5f; //How fast the Camera should snap into position if there are no obstacles
    private bool isFirstFrame = true;

    Vector3 defaultPos;
    Vector3 directionNormalized;
    Transform parentTransform;
    float defaultDistance;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
        directionNormalized = defaultPos.normalized;
        parentTransform = transform.parent;
        defaultDistance = Vector3.Distance(defaultPos, Vector3.zero);

        //Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Debug.Log("Start() defaultPos = " + defaultPos + " position: " + transform.position + 
        //           "  reference transform: " + referenceTransform.position);
    }

    // LateUpdate is called after Update
    void LateUpdate()
    {
        dirtyFixCameraBug();
        // Debug.Log("LateUpdate() defaultPos = " + defaultPos + " position: " + transform.position +
        //           "  reference transform: " + referenceTransform.position);
        Vector3 currentPos = defaultPos;
        RaycastHit hit;
        Vector3 dirTmp = parentTransform.TransformPoint(defaultPos) - referenceTransform.position;
        // Debug.Log("dirTmp: " + dirTmp + " collision offset: " + collisionOffset + 
        //           " defaultDistance: " + defaultDistance);
        if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, defaultDistance))
        {
            // Debug.Log("LateUpdate() IF defaultPos = " + defaultPos + " hit collider = " + hit.collider.name);
            currentPos = (directionNormalized * (hit.distance - collisionOffset));

            transform.localPosition = currentPos;
        }
        else
        {
            // Debug.Log("LateUpdate() ELSE defaultPos = " + defaultPos);
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * cameraSpeed);
        }
    }

    private void dirtyFixCameraBug()
    {
        if (!isFirstFrame)
            return;
        isFirstFrame = false;
        transform.position += new Vector3(-3.7f, 0, 5.5f);
    }
}