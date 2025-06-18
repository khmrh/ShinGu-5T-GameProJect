using UnityEngine;

public class WorldButton : MonoBehaviour
{
    public Transform hingeTransform;                // ȸ���� ��ø (�� pivot)
    public AudioSource clickSound;

    public Vector3 closedRotation = new Vector3(0f, 0f, 0f);
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);   // Y������ ����

    public float rotateSpeed = 2f;

    private Quaternion targetRotation;
    private bool isOpen = false;
    private bool isRotating = false;

    private void Start()
    {
        targetRotation = Quaternion.Euler(closedRotation);
        hingeTransform.rotation = targetRotation;
    }

    private void Update()
    {
        if (isRotating)
        {
            hingeTransform.rotation = Quaternion.Slerp(hingeTransform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            if (Quaternion.Angle(hingeTransform.rotation, targetRotation) < 0.1f)
            {
                hingeTransform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (isRotating) return; // ȸ�� �߿��� Ŭ�� ����

        clickSound?.Play();

        isOpen = !isOpen;
        targetRotation = Quaternion.Euler(isOpen ? openRotation : closedRotation);
        isRotating = true;
    }
}
