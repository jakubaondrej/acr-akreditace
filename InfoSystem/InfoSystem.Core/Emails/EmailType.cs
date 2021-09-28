namespace InfoSystem.Core.Emails
{

    public static class TypeEmail
    {
        public const string Basic = "BasicEmailTemplate.html";
        public const string UserCreated = "UserCreated.html";
    }
    public enum EmailType
    {
        Basic,
        UserCreated,

    }
}
