using UnityEngine;

public class ExplodeCube : MonoBehaviour
{
    private bool _collisionSet;
    public GameObject RestartButton,explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && !_collisionSet)//��������� �� ��� � ����������� �� ������
        {
            for (int i = collision.transform.childCount - 1; i >= 0; i--)//������� �� �����
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>(); //��������� ������
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
                // ��������� �������� ����
                child.SetParent(null);// ������� ��������
            }
            RestartButton.SetActive(true);// ����������� ������ ��������
            Destroy(collision.gameObject);//������� ������
            _collisionSet = true;//������ ��� ������� ��� ���������
            Camera.main.gameObject.AddComponent<CameraShake>();//���������� ������ � ������� ������

            GameObject newvFx =Instantiate(explosion, new Vector3(collision.contacts[0].point.x,
                collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity) as GameObject;
            //������� ������ ������
            Destroy(newvFx, 2.5f);// ������� ������ ����� 2.5 ���

            if (PlayerPrefs.GetString("music") != "No")//����������� ����
                GetComponent<AudioSource>().Play();
            Camera.main.gameObject.transform.position = Vector3.MoveTowards(Camera.main.gameObject.transform.position,
               new Vector3(Camera.main.gameObject.transform.position.x, 5.9f, Camera.main.gameObject.transform.position.z),
               2f * Time.deltaTime);
        }
    }
}
