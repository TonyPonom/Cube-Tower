using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;
    private float camMoveToYPosition, camMoveSpeed = 2f;

    public Text scoreTxt;

    public GameObject[] cubesToCreate;

    public GameObject  AllCubes,vFx;
    public GameObject[] canvasStartPage;
    private Rigidbody allcubesRb;//��� ����������� ������
    private bool IsLose,FirstCube;

    public Color[] bgColors;
    private Color toCameraColor;

    private List<Vector3> allCubesPositions = new List<Vector3>//������������ ������ � �������� ���������
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(-1,0,-1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
    };

    private int prevCountMaxHorizontal;
    private Transform mainCam;
    private Coroutine showCubePlace;

    private List<GameObject> possibleCubesToCreate = new List<GameObject>();

    IEnumerator ShowCubePlace()///��������� ��������
    {
        while (true)
        {
            SpawnPositions();

            yield return new WaitForSeconds(cubeChangePlaceSpeed);// ��������� ����� ��������� �����
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();//������������ ������ ����� ������(System.Collections.Generic)
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));

        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0)
            IsLose = true;
        else
            cubeToPlace.position = positions[0];
    }

    private bool IsPositionEmpty(Vector3 targetpos)
    {
        if (targetpos.y == 0)
            return false;

        foreach (Vector3 pos in allCubesPositions)
        {
            if (pos.x == targetpos.x && pos.y == targetpos.y && pos.z == targetpos.z)
                return false;
        }
        return true;
    }
    public int CountForColor;
    public void Start()
    {
        if (PlayerPrefs.GetInt("score") < 5)
            possibleCubesToCreate.Add(cubesToCreate[0]);
        else if (PlayerPrefs.GetInt("score") < 10)
            AddPossibleCubes(2);
        else if (PlayerPrefs.GetInt("score") < 20)
            AddPossibleCubes(3);
        else if (PlayerPrefs.GetInt("score") < 35)
            AddPossibleCubes(4);
        else if (PlayerPrefs.GetInt("score") < 45)
            AddPossibleCubes(5);
        else if (PlayerPrefs.GetInt("score") < 55)
            AddPossibleCubes(6);
        else if (PlayerPrefs.GetInt("score") < 65)
            AddPossibleCubes(7);
        else if (PlayerPrefs.GetInt("score") < 75)
            AddPossibleCubes(8);
        else 
            AddPossibleCubes(9);

        possibleCubesToCreate.Add(cubesToCreate[0]);
        scoreTxt.text = "<size=40><color=#B51E1E>Best</color></size>" + PlayerPrefs.GetInt("score") +
           "\n<size=30>Now</size> 0";

        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToYPosition = 5.9f + nowCube.y - 1f;

        allcubesRb = AllCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());// ���������� �������� (���������� System.Collections)
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace != null && AllCubes != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR //��� ������ �� ��������� ������� ��� ��� ������ ������ �������
            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return
#endif

            if (!FirstCube)//������� ������, ����� ���������� ����
            {
                FirstCube = true;
                foreach (GameObject obj in canvasStartPage)
                {
                    Destroy(obj);
                }
            }
            GameObject creatCube = null;
            if (possibleCubesToCreate.Count == 1)
                creatCube = possibleCubesToCreate[0];
            else
                creatCube = possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)];

            GameObject newCube = Instantiate(creatCube,
                cubeToPlace.position, Quaternion.identity) as GameObject; //������� ������� ������
            //��������� ������ (��� �������, ����������, ��� ��������� ���)

            newCube.transform.SetParent(AllCubes.transform);//������������� ��������
            nowCube.setVector(cubeToPlace.position);// ����� �������� ���������� �� ������� ������������� ����� ���������
            allCubesPositions.Add(nowCube.getVector());// ��������� ���������� � ������ �������

            if (PlayerPrefs.GetString("music") != "No")// ����������� ����
                GetComponent<AudioSource>().Play();

            GameObject newvFx = Instantiate(vFx, cubeToPlace.position, Quaternion.identity) as GameObject;
            // ������� ������
            Destroy(newvFx, 1.5f);// ������� ������ ����� 1.5 ���

            allcubesRb.isKinematic = true; // ������������� ����������
            allcubesRb.isKinematic = false;

            SpawnPositions(); //����� �����
            MoveCameraChangeBg();//������ ���� ���� ������� ������
        }

        if (!IsLose && allcubesRb.velocity.magnitude > 0.1f)//��������� && ��� ��� �������� ������
        {
            Destroy(cubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(showCubePlace);//���������� ��������
        }
        if (!IsLose)
        {
            mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
                new Vector3(mainCam.localPosition.x, camMoveToYPosition, mainCam.localPosition.z),
                camMoveSpeed * Time.deltaTime);//������ ������ ��������� ������
            if (Camera.main.backgroundColor != toCameraColor)//������ �������� ���� ����
                Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor,
                    Time.deltaTime / 1.5f);
        }
        else
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position,
               new Vector3(mainCam.transform.position.x, 5.9f, mainCam.transform.position.z),
               camMoveSpeed * Time.deltaTime);
        }
    }
    private void MoveCameraChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHor;

        foreach(Vector3 pos in allCubesPositions)// ������� ������������� ����������
        {
            if (Math.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Convert.ToInt32(pos.x);

            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);

            if (Math.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Convert.ToInt32(pos.z);
        }
        if (PlayerPrefs.GetInt("score") < maxY - 1)
            PlayerPrefs.SetInt("score", maxY - 1);
        scoreTxt.text = "<size=40><color=#B51E1E>Best</color></size>" + PlayerPrefs.GetInt("score") +
            "\n<size=30>Now</size>" + (maxY - 1);

        camMoveToYPosition = 5.9f + nowCube.y - 1f;//�� ������� ���� ������� ������ �� y

        maxHor = maxX > maxZ ? maxX : maxZ;
        if(maxHor % 3 == 0 && prevCountMaxHorizontal != maxHor)// ������ 3 ���� �� ��� x,z �������� ������
        {
            mainCam.localPosition -= new Vector3(0, 0, 2.5f);
            prevCountMaxHorizontal = maxHor;
        }
        if (maxY % 6 == 0)
            CountForColor += 1;
        if (CountForColor > 3)
            CountForColor -= 3;
        if(CountForColor!=0)
            toCameraColor = bgColors[CountForColor-1];
        //if (maxY >= 7) //������ ����� ��� ���������
        //    toCameraColor = bgColors[2];
        //else if(maxY >= 5)
        //    toCameraColor = bgColors[1];
        //else if (maxY >= 2)
        //    toCameraColor = bgColors[0];
    }
    private void AddPossibleCubes(int till)
    {
        for (int i=0; i < till; i++)
            possibleCubesToCreate.Add(cubesToCreate[i]);
    }

    struct CubePos
    {
        public int x, y, z;

        public CubePos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 getVector()
        {
            return new Vector3(x, y, z);
        }

        public void setVector(Vector3 pos)
        {
            x = Convert.ToInt32(pos.x);//(System)
            y = Convert.ToInt32(pos.y);
            z = Convert.ToInt32(pos.z);
        }
    }
}
