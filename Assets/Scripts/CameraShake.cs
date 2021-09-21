using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private float ShakeDur = 1f, shakeAmount = 0.04f, decreaseFactor = 1.5f;//������ ������, ������ ������

    private Vector3 originPos; //������������ ������������

    private void Start()
    {
        camTransform = GetComponent<Transform>();
        originPos = camTransform.localPosition;
    }
    private void Update()
    {
        if(ShakeDur > 0)
        {
            camTransform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            // ������ � ����� � ������� ��������
            ShakeDur -= Time.deltaTime * decreaseFactor;// ��������� �����
        }
        else
        {
            ShakeDur = 0;
            camTransform.localPosition = originPos;
        }
    }
}
