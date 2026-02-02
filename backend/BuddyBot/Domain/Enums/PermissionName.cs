using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;
public enum PermissionName
{
    //[Display( Name = "Генерация токена для создания кандидата" )]

    AccountCreationTokenView,
    AccountCreationTokenUpdate,
    AccountCreationTokenDelete,
    AccountCreationTokenCreateHr,
    AccountCreationTokenCreateCandidate,
    AccountCreationTokenRevoke,

    DepartmentView,
    DepartmentCreate,
    DepartmentUpdate,
    DepartmentDelete,

    FeedbackView,
    FeedbackDelete,

    OnboardingAccessRequestCreate,
    OnboardingAccessRequestView,
    OnboardingAccessRequestUpdate,
    OnboardingAccessRequestConfirm,
    OnboardingAccessRequestReject,
    OnboardingAccessRequestDelete,
    OnboardingAccessRequestRestore,

    TeamView,
    TeamCreate,
    TeamUpdate,
    TeamDelete,

    UserLogout,
    UserCreateMentor,
    UserView,
    UserUpdate,
    UserDelete,
}
