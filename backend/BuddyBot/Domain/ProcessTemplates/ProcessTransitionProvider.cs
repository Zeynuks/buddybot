using Domain.Enums;

namespace Domain.ProcessTemplates;
public static class ProcessTransitionProvider
{
    public static readonly Dictionary<(StepKind, string), StepKind> PreboardingTransitions = new()
    {
        {(StepKind.PreboardingStart, StepKind.PreboardingWelcome.ToString()), StepKind.PreboardingWelcome},
        {(StepKind.PreboardingWelcome, StepKind.CompanyIntro.ToString()), StepKind.CompanyIntro},
        {(StepKind.CompanyIntro, StepKind.OfferDecision.ToString()), StepKind.OfferDecision},

        {(StepKind.OfferDecision, StepKind.DocumentsPreparation.ToString()), StepKind.DocumentsPreparation},
        {(StepKind.OfferDecision, StepKind.ContactHR.ToString()), StepKind.ContactHR},
        {(StepKind.OfferDecision, StepKind.PreboardingFAQ.ToString()), StepKind.PreboardingFAQ},

        {(StepKind.PreboardingFAQ, StepKind.OfferDecision.ToString()), StepKind.OfferDecision},
        {(StepKind.ContactHR, StepKind.DocumentsPreparation.ToString()), StepKind.DocumentsPreparation},

        {(StepKind.DocumentsPreparation, StepKind.NotificationInfo.ToString()), StepKind.NotificationInfo},
        {(StepKind.NotificationInfo, StepKind.ResolveQuestions.ToString()), StepKind.ResolveQuestions},
        {(StepKind.ResolveQuestions, StepKind.PreboardingComplete.ToString()), StepKind.PreboardingComplete},
        {(StepKind.PreboardingComplete, StepKind.WaitAdminApprove.ToString()), StepKind.WaitAdminApprove},
    };

    public static readonly Dictionary<(StepKind, string), StepKind> OnboardingTransitions = new()
    {
        { (StepKind.OnboardingPendingStart, StepKind.MeetTeamIntro.ToString()), StepKind.MeetTeamIntro },
        { (StepKind.MeetTeamIntro, StepKind.MeetHead.ToString()), StepKind.MeetHead },
        { (StepKind.MeetHead, StepKind.MeetMentor.ToString()), StepKind.MeetMentor },
        { (StepKind.MeetMentor, StepKind.CompanyPolicies.ToString()), StepKind.CompanyPolicies },
    };

    public static StepKind? GetNextStep( ProcessKind processKind, StepKind currentStep, string userAction )
    {
        return processKind switch
        {
            ProcessKind.Preboarding when PreboardingTransitions.TryGetValue( (currentStep, userAction), out var nextStep ) => nextStep,
            ProcessKind.Onboarding when OnboardingTransitions.TryGetValue( (currentStep, userAction), out var nextStep ) => nextStep,
            _ => null
        };
    }
}
