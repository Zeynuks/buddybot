export const PermissionName = {
    AccountCreationTokenView: "AccountCreationTokenView",
    AccountCreationTokenUpdate: "AccountCreationTokenUpdate",
    AccountCreationTokenDelete: "AccountCreationTokenDelete",
    AccountCreationTokenCreateHr: "AccountCreationTokenCreateHr",
    AccountCreationTokenCreateCandidate: "AccountCreationTokenCreateCandidate",
    AccountCreationTokenRevoke: "AccountCreationTokenRevoke",

    DepartmentView: "DepartmentView",
    DepartmentCreate: "DepartmentCreate",
    DepartmentUpdate: "DepartmentUpdate",
    DepartmentDelete: "DepartmentDelete",

    FeedbackView: "FeedbackView",
    FeedbackDelete: "FeedbackDelete",

    OnboardingAccessRequestCreate: "OnboardingAccessRequestCreate",
    OnboardingAccessRequestView: "OnboardingAccessRequestView",
    OnboardingAccessRequestUpdate: "OnboardingAccessRequestUpdate",
    OnboardingAccessRequestConfirm: "OnboardingAccessRequestConfirm",
    OnboardingAccessRequestReject: "OnboardingAccessRequestReject",
    OnboardingAccessRequestDelete: "OnboardingAccessRequestDelete",
    OnboardingAccessRequestRestore: "OnboardingAccessRequestRestore",

    TeamView: "TeamView",
    TeamCreate: "TeamCreate",
    TeamUpdate: "TeamUpdate",
    TeamDelete: "TeamDelete",

    UserRefreshToken: "UserRefreshToken",
    UserLogin: "UserLogin",
    UserLogout: "UserLogout",
    UserCreateMentor: "UserCreateMentor",
    UserView: "UserView",
    UserUpdate: "UserUpdate",
    UserDelete: "UserDelete",
} as const;

export type PermissionName = (typeof PermissionName)[keyof typeof PermissionName];
