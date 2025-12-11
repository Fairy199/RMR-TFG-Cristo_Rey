using UnityEngine;

public class WindowsHandler : MonoBehaviour
{
    public GameObject[] windows;
    public void EnableWindow(int idWindow)
    {
        windows[idWindow].SetActive(true);

        for (int i = 0; i < windows.Length; i++)
        {
            if (idWindow != i)
            {
                windows[i].SetActive(false);
            }
        }
    }
}
