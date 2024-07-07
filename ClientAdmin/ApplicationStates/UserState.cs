namespace ClientAdmin.ApplicationStates
{
    public class UserState
    {
        public Action? GeneralUserAction { get; set; }
        public bool ShowGeneralUser { get; set; }
        public void ResetAllUsers()
        {
            ShowGeneralUser = false;
        }
        public void GeneralUserClicked()
        {
            ResetAllUsers();
            ShowGeneralUser = true;
            GeneralUserAction?.Invoke();
        }
    }
}
