namespace AspNetCore.Identity.LiteDB
{
    /// <summary>
    /// Wraps the base UserLoginInfo so that it can be serialized using litedb.
    /// Represents a linked login for a user (i.e. a facebook/google account)
    /// </summary>
    public class SerializableUserLoginInfo
    {
        public SerializableUserLoginInfo()
        {

        }

        /// <summary>Constructor</summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        public SerializableUserLoginInfo(string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }

        /// <summary>
        ///     Provider for the linked login, i.e. Facebook, Google, etc.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>User specific key for the login provider</summary>
        public string ProviderKey { get; set; }
    }
}
