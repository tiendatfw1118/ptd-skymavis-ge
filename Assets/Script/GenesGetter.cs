using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
public class GenesGetter : MonoBehaviour
{

    [SerializeField] string axiesID;
    private string genes;
    private bool _isFetchingGenes = false;
    // Start is called before the first frame update
    public void OnButtonClicked()
    {
        if (string.IsNullOrEmpty(axiesID) || _isFetchingGenes) return;
        _isFetchingGenes = true;
        StartCoroutine(GetAxiesGenes(axiesID));
    }
    public IEnumerator GetAxiesGenes(string axieId)
    {
        Debug.Log("Getting Genes");
        string searchString = "{ axie (axieId: \"" + axieId + "\") { id, genes, newGenes}}";
        JObject jPayload = new JObject();
        jPayload.Add(new JProperty("query", searchString));

        var wr = new UnityWebRequest("https://graphql-gateway.axieinfinity.com/graphql", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jPayload.ToString().ToCharArray());
        wr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        wr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        wr.SetRequestHeader("Content-Type", "application/json");
        wr.timeout = 10;
        yield return wr.SendWebRequest();
        if (wr.error == null)
        {
            var result = wr.downloadHandler != null ? wr.downloadHandler.text : null;
            if (!string.IsNullOrEmpty(result))
            {
                JObject jResult = JObject.Parse(result);
                string genesStr = (string)jResult["data"]["axie"]["newGenes"];
                genes = genesStr;
                Debug.Log(genesStr);
            }
        }
        _isFetchingGenes = false;
    }
}
