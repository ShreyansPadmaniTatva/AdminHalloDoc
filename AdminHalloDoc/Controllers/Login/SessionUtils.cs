using AdminHalloDoc.Entities.ViewModel.AdminViewModel;

namespace AdminHalloDoc.Controllers.Login
{
    public class SessionUtils
    {
        public static UserInfo GetLogginUser(ISession session)
        {
            UserInfo userInfo = null;
            if (!string.IsNullOrEmpty(session.GetString("UserId")))
            {
                userInfo = new UserInfo();
                userInfo.FirstName = session.GetString("FirstName");
                userInfo.LastName = session.GetString("LastName");
                userInfo.Role = session.GetString("Role");
                userInfo.UserId = Convert.ToInt32(session.GetString("UserId"));
            }

            return userInfo;
        }
        public static void setLogginUser(ISession session, UserInfo admin)
        {
            if (admin != null)
            {
                session.SetString("FirstName", admin.FirstName);
                session.SetString("LastName", admin.LastName);
                session.SetString("Role", admin.Role);
                session.SetString("UserId", admin.UserId.ToString());

            }

            return;
        }
    }
}
