namespace Web.Common.Statics
{
    public static class StringRegex
    {
        public const string EMAIL_REGEX = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public const string NAME_REGEX = @"^[a-zA-Z0-9\s]{1,50}$";
    }
}
