namespace SelfKOrderWeb.Factory
{
    public class Enums
    {
        public enum SearchStauts { success = 1, NotFound = -3, Error = -1, IlligalOperation = -2, NoChanges = 0, Dublicated = -4, NonDeletedUser = -5 };
        public enum main { compmain = 9, brandmain = 9, franchmain = 4 };
        public enum OrderStauts { Completed = 1, Cancelled = 2 };

    }
}
