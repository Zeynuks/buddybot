using Domain.Enums;

namespace Infrastructure.Foundation.Database.Seeding;
public static class DefaultRolePermissions
{
    public static readonly IReadOnlyDictionary<RoleName, PermissionName[]> Map =
        new Dictionary<RoleName, PermissionName[]>
        {
            [ RoleName.Admin ] = Enum.GetValues<PermissionName>(),
            [ RoleName.SuperHR ] = Enum.GetValues<PermissionName>(),
            [ RoleName.HR ] = new[]
            {
                PermissionName.AccountCreationTokenCreateCandidate,
                //PermissionName.AccountCreationTokenCreateHr,
                //PermissionName.AccountCreationTokenDelete,
                PermissionName.AccountCreationTokenRevoke,
                PermissionName.AccountCreationTokenUpdate,
                PermissionName.AccountCreationTokenView,

                //PermissionName.DepartmentCreate,
                //PermissionName.DepartmentDelete,
                //PermissionName.DepartmentUpdate,
                PermissionName.DepartmentView,

                //PermissionName.FeedbackDelete,
                PermissionName.FeedbackView,

                PermissionName.OnboardingAccessRequestConfirm,
                PermissionName.OnboardingAccessRequestCreate,
                PermissionName.OnboardingAccessRequestDelete,
                PermissionName.OnboardingAccessRequestReject,
                PermissionName.OnboardingAccessRequestRestore,
                PermissionName.OnboardingAccessRequestUpdate,
                PermissionName.OnboardingAccessRequestView,

                //PermissionName.TeamCreate,
                //PermissionName.TeamDelete,
                //PermissionName.TeamUpdate,
                PermissionName.TeamView,

                PermissionName.UserCreateMentor,
                PermissionName.UserDelete,
                PermissionName.UserLogout,
                PermissionName.UserUpdate,
                PermissionName.UserView
            }
        };
}
