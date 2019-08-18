using UnityEngine;
using UnityEngine.UI;

public class MMOCharacterController : MonoBehaviour
{
    private Vector3 movement;                                                           //movement vector
    private float mouseX, mouseY;                                                       //mouse position
    public Transform playerCam, character, centerPoint;                                 //camera, character, and camera center point transforms
    public float gravity = 20f, jumpSpeed = 8f;                                         //variables storing gravity factor and jump speed
    public float mouseYPosition = 1f;                                                   //height at which the camera is off the ground initially
    public float moveSpeed = 10f, strafeSpeed = 8f, rotateSpeed = 2f;                    //speed of the character, speed it strafes, and speed it rotates
    public float zoomSpeed = 2f, currentZoom = 10f, zoomMax = -10f, zoomMin = -2f;      //zoom speed, current camera zoom, and max/min zoom level
    private CursorLockMode wantedMode;                                                  //stores current desired mode of mouse lock

    void Update()
    {
        currentZoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (currentZoom > zoomMin)
        {
            currentZoom = zoomMin;
        }
        if (currentZoom < zoomMax)
        {
            currentZoom = zoomMax;
        }

        playerCam.transform.localPosition = new Vector3(0f, 0f, currentZoom);

        if (Input.GetMouseButton(1) | Input.GetMouseButton(0))
        {
            mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;
        }
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        playerCam.LookAt(centerPoint);
        centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);

        movement.z = Input.GetAxis("Vertical") * moveSpeed;
        movement.x = Input.GetAxis("Horizontal") * strafeSpeed;
        //gravity works
        if (character.GetComponent<CharacterController>().isGrounded && Input.GetButton("Jump"))
        {
            movement.y = jumpSpeed;
        }

        movement.y -= gravity * Time.deltaTime;

        movement = character.rotation * movement;
    }
    void LateUpdate()
    {

        character.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
        centerPoint.position = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);

        if (!Input.GetMouseButton(0)&&Input.GetMouseButton(1))
        {
            Quaternion turnAngle = Quaternion.Euler(0f, centerPoint.eulerAngles.y, 0);
            character.rotation = turnAngle;
        }

    }


    // Apply requested cursor state
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        // Release cursor on escape keypress
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = wantedMode = CursorLockMode.None;

        switch (Cursor.lockState)
        {
            case CursorLockMode.None:
                GUILayout.Label("Cursor is normal");
                if (GUILayout.Button("Lock cursor"))
                    wantedMode = CursorLockMode.Locked;
                if (GUILayout.Button("Confine cursor"))
                    wantedMode = CursorLockMode.Confined;
                break;
            case CursorLockMode.Confined:
                GUILayout.Label("Cursor is confined");
                if (GUILayout.Button("Lock cursor"))
                    wantedMode = CursorLockMode.Locked;
                if (GUILayout.Button("Release cursor"))
                    wantedMode = CursorLockMode.None;
                break;
            case CursorLockMode.Locked:
                GUILayout.Label("Cursor is locked");
                if (GUILayout.Button("Unlock cursor"))
                    wantedMode = CursorLockMode.None;
                if (GUILayout.Button("Confine cursor"))
                    wantedMode = CursorLockMode.Confined;
                break;
        }

        GUILayout.EndVertical();

        SetCursorState();
    }
}
