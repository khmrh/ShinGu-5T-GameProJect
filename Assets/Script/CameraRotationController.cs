using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    public Vector3 lookDownEuler = new Vector3(60f, 0f, 0f); // 아래 보기
    public Vector3 lookUpEuler = new Vector3(30f, 0f, 0f);   // 위 보기
    public float rotateSpeed = 2f;

    private Quaternion targetRotation;
    private bool isRotating = false;

    private void Start()
    {
        LookDownInstant();
    }

    private void Update()
    {
        if (!isRotating) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        if (angle < 0.1f)
        {
            transform.rotation = targetRotation;  //  정확히 고정
            isRotating = false;                  //  멈춤
        }
    }

    public void LookUp()
    {
        targetRotation = Quaternion.Euler(lookUpEuler);
        isRotating = true;
    }

    public void LookDown()
    {
        targetRotation = Quaternion.Euler(lookDownEuler);
        isRotating = true;
    }

    public void LookDownInstant()
    {
        targetRotation = Quaternion.Euler(lookDownEuler);
        transform.rotation = targetRotation;
        isRotating = false;
    }
}
