using UnityEngine;

public class ExplodeCube : MonoBehaviour
{
    private bool _collisionSet;
    public GameObject RestartButton,explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && !_collisionSet)//проверяем на тэг и происходило ли раньше
        {
            for (int i = collision.transform.childCount - 1; i >= 0; i--)//смотрим по детям
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>(); //добавляем физику
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
                // добовляем взрывную силу
                child.SetParent(null);// удаляем родителя
            }
            RestartButton.SetActive(true);// порявляется кнопка рестарта
            Destroy(collision.gameObject);//удаляем аллкуб
            _collisionSet = true;//флажок что событие уже произошло
            Camera.main.gameObject.AddComponent<CameraShake>();//подключаем скрипт с тряской камеры

            GameObject newvFx =Instantiate(explosion, new Vector3(collision.contacts[0].point.x,
                collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity) as GameObject;
            //создаем эффект взрыва
            Destroy(newvFx, 2.5f);// удаляем эффект через 2.5 сек

            if (PlayerPrefs.GetString("music") != "No")//проигрываем звук
                GetComponent<AudioSource>().Play();
            Camera.main.gameObject.transform.position = Vector3.MoveTowards(Camera.main.gameObject.transform.position,
               new Vector3(Camera.main.gameObject.transform.position.x, 5.9f, Camera.main.gameObject.transform.position.z),
               2f * Time.deltaTime);
        }
    }
}
