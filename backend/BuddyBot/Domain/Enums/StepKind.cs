namespace Domain.Enums;
public enum StepKind
{
    // Пребординг
    PreboardingStart,
    PreboardingWelcome,
    CompanyIntro,
    OfferDecision,
    PreboardingFAQ,
    ContactHR,
    DocumentsPreparation,
    NotificationInfo,
    ResolveQuestions,
    PreboardingComplete,
    WaitAdminApprove,

    // Онбординг
    OnboardingPendingStart,
    MeetTeamIntro,
    MeetHead,
    MeetMentor,
    CompanyPolicies,

    // Личный кабинет
    PersonalAreaHome
}