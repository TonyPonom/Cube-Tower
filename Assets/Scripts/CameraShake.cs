using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private float ShakeDur = 1f, shakeAmount = 0.04f, decreaseFactor = 1.5f;//сколко трясет, радиус тряски

    private Vector3 originPos; //оригинальное расположение

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
            // рандом в сфере с заданым радиусом
            ShakeDur -= Time.deltaTime * decreaseFactor;// уменьшаем время
        }
        else
        {
            ShakeDur = 0;
            camTransform.localPosition = originPos;
        }
    }
}
