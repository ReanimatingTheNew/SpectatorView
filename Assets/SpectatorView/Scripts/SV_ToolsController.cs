using HoloToolkit.Unity;

public class SV_ToolsController : Singleton<SV_ToolsController>
{
    public bool IsOpen = true;

    private void Start()
    {
        if (IsOpen)
        {
            gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        IsOpen = false;
        gameObject.SetActive(false);
    }
}
