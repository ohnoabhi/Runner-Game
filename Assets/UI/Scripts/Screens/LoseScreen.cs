public class LoseScreen : BaseScreen
{
    public void OnClickContinue()
    {
        ScreenController.instance.Show("Menu");
    }
}