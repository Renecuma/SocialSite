namespace SocialSite.Application.Common;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string User = "User";

    public const string AdminOrManager = Admin + "," + Manager;

    public static readonly string[] All = [Admin, Manager, User];
}
