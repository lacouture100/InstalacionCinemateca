using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WWWVideo : MonoBehaviour {
    void Start() {
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture() {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.slurp-ramen.com/wp-content/uploads/2017/06/hello.png");
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
