namespace BookSeller_App.BAL
{
    public static class UserSessionCV
    {
        private static IHttpContextAccessor _contextAccessor;
        static UserSessionCV() => _contextAccessor = new HttpContextAccessor();
        public static string? Username() => (_contextAccessor != null && _contextAccessor.HttpContext != null) ? _contextAccessor.HttpContext.Session.GetString("UserName") : null;
        public static string? UserPassword() => (_contextAccessor != null && _contextAccessor.HttpContext != null) ? _contextAccessor.HttpContext.Session.GetString("UserPassword") : null;
        public static int? UserId() => (_contextAccessor != null && _contextAccessor.HttpContext != null) ? _contextAccessor.HttpContext.Session.GetInt32("UserID") : null;
    }
}