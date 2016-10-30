using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateBillboard : EditorWindow
{

    [MenuItem("GameObject/3D Object/Billboard")]
    static void Create()
    {
        GameObject targetObj = new GameObject("New Billboard");
        MeshFilter meshFilter = targetObj.AddComponent<MeshFilter>();   //MeshFilter 컴포넌트 추가

        targetObj.AddComponent<MeshRenderer>();  //MeshRenderer컴포넌트 추가

        Mesh mesh = new Mesh();
               
        Vector3 vLeftTop, vRightTop, vLeftBottom, vRightBottom;

        vLeftTop = new Vector3(-0.5f, 0.5f, 0);
        vRightTop = new Vector3(0.5f, 0.5f, 0);
        vLeftBottom = new Vector3(-0.5f, -0.5f, 0);
        vRightBottom = new Vector3(0.5f, -0.5f, 0);

        mesh.vertices = new Vector3[]{                   //정점 4개만들어줌
            vLeftTop, vRightTop,
            vLeftBottom, vRightBottom
        };

        mesh.uv = new Vector2[]{                        //정점 별로 UV좌표 찍어줌
            new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f)
        };

        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };        //삼각형그릴때 순서

        mesh.RecalculateNormals();              //일반좌표계로 다시 계산해주는 함수
        meshFilter.mesh = mesh;                 //메쉬 필터에 Mesh를 넣어줌 

        //Mesh를 Asset으로 저장하지 않으면 Prefab 형태로 빌보드를 사용할 수 없음
    }
}
