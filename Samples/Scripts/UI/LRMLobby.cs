using LightReflectiveMirror;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LRMLobby : MonoBehaviour
{
    [SerializeField] private Transform serverListParent;
    [SerializeField] private Button serverButtonPrefab;
    private LightReflectiveMirrorTransport LRM;
    private void Start()
    {
        if (LRM == null)
            LRM = (LightReflectiveMirrorTransport)Transport.activeTransport;
        LRM.serverListUpdated.AddListener(OnServerListUpdated);
        RefreshServerList();
    }
    public void RefreshServerList()
    {
        LRM.RequestServerList();
    }
    public void OnServerListUpdated()
    {
        foreach (Transform serverButton in serverListParent)
        {
            Destroy(serverButton.gameObject);
        }
        for (int i = 0; i < LRM.relayServerList.Count; i++)
        {
            Button newServerButton = Instantiate(serverButtonPrefab, serverListParent);

            TMP_Text serverLabel = newServerButton.transform.GetChild(0).GetComponent<TMP_Text>();
            serverLabel.text = LRM.relayServerList[i].serverName;

            int serverId = LRM.relayServerList[i].serverId;
            newServerButton.onClick.AddListener(() => ConnectToServer(serverId));
        }
    }
    private void ConnectToServer(int serverId)
    {
        NetworkManager.singleton.networkAddress = serverId.ToString();
        NetworkManager.singleton.StartClient();
    }
    private void OnDisable()
    {
        if (LRM != null)
            LRM.serverListUpdated.RemoveListener(OnServerListUpdated);
    }
}
