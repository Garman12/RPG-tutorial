using UnityEngine;

public class MMOCharacterController : MonoBehaviour
{
    private Vector3 movement;
    private float mouseX, mouseY;
    public Transform playerCam, character, centerPoint;
    public float currentZoom = 10f;
    public float gravity = 20f, jumpSpeed = 8f;
    public float mouseYPosition = 1f;
    public float moveSpeed = 10f, strafeSpeed = 8f,rotateSpeed = 2f;
    public float zoomMax = -10f,zoomMin = -2f, zoomSpeed = 2f;

    // Update is called once per frame
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

        if (Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;
            Cursor.visible = false;
        }
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        playerCam.LookAt(centerPoint);
        centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);

        movement.z = Input.GetAxis("Vertical") * moveSpeed;
        movement.x = Input.GetAxis("Horizontal") * strafeSpeed;
        //movement = new Vector3(moveLR, 0f, moveFB);
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

        if (Input.GetAxis("Vertical") > 0f | Input.GetAxis("Vertical") < 0f)
        {
            Quaternion turnAngle = Quaternion.Euler(0f, centerPoint.eulerAngles.y, 0);
            character.rotation = turnAngle;
        }

    }

}
