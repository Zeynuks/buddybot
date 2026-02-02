using Application.CQRSInterfaces;
using Application.Interfaces;
using Application.Results;
using Application.UseCases.AccountCreationTokens.Commands.CreateCandidateInvite;
using Application.UseCases.AccountCreationTokens.Commands.CreateHRInvite;
using Application.UseCases.AccountCreationTokens.Commands.DeleteToken;
using Application.UseCases.AccountCreationTokens.Commands.MarkTokenAsExpired;
using Application.UseCases.AccountCreationTokens.Commands.RevokeToken;
using Application.UseCases.AccountCreationTokens.Commands.UpdateToken;
using Application.UseCases.AccountCreationTokens.Queries.GetAccountCreationTokenByValue;
using Application.UseCases.AccountCreationTokens.Queries.GetAccountCreationTokens;
using Application.UseCases.AccountCreationTokens.Queries.GetExpiredTokens;
using Application.UseCases.CandidateProcesses.Commands.GoNextStep;
using Application.UseCases.CandidateProcesses.Commands.ResetCandidateProcess;
using Application.UseCases.CandidateProcesses.Commands.TransferToOnboarding;
using Application.UseCases.CandidateProcesses.Commands.TransferToPersonalArea;
using Application.UseCases.CandidateProcesses.Query.GetActiveCandidateProcess;
using Application.UseCases.CandidateProcesses.Query.GetCandidateProcess;
using Application.UseCases.Cities.Commands.CreateCity;
using Application.UseCases.Cities.Commands.DeleteCity;
using Application.UseCases.Cities.Commands.UpdateCity;
using Application.UseCases.Cities.Queries.GetCities;
using Application.UseCases.Cities.Queries.GetCitiesLookup;
using Application.UseCases.Cities.Queries.GetCityById;
using Application.UseCases.Countries.Commands.CreateCountry;
using Application.UseCases.Countries.Commands.DeleteCountry;
using Application.UseCases.Countries.Commands.UpdateCountry;
using Application.UseCases.Countries.Queries.GetCountries;
using Application.UseCases.Countries.Queries.GetCountriesLookup;
using Application.UseCases.Countries.Queries.GetCountryById;
using Application.UseCases.Departments.Commands.CreateDepartment;
using Application.UseCases.Departments.Commands.DeleteDepartment;
using Application.UseCases.Departments.Commands.UpdateDepartment;
using Application.UseCases.Departments.Queries.GetDepartmentById;
using Application.UseCases.Departments.Queries.GetDepartments;
using Application.UseCases.Departments.Queries.GetDepartmentsLookup;
using Application.UseCases.Feedbacks.Commands.ConfirmFeedback;
using Application.UseCases.Feedbacks.Commands.CreateFeedback;
using Application.UseCases.Feedbacks.Commands.DeleteFeedback;
using Application.UseCases.Feedbacks.Commands.ExportFeedbacks;
using Application.UseCases.Feedbacks.Queries.GetDraftFeedback;
using Application.UseCases.Feedbacks.Queries.GetFeedbacks;
using Application.UseCases.OnboardingAccessRequests.Commands.ConfirmOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.CreateOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.DeleteOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.RejectOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.RestoreOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.UpdateOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Queries.GetDueCandidates;
using Application.UseCases.OnboardingAccessRequests.Queries.GetOnboardingAccessRequestByCandidateId;
using Application.UseCases.OnboardingAccessRequests.Queries.GetOnboardingAccessRequests;
using Application.UseCases.Services.AuthTokenServices;
using Application.UseCases.Services.LoginServices;
using Application.UseCases.Services.QrCodeGeneratorService;
using Application.UseCases.Services.RefreshTokenServices;
using Application.UseCases.Teams.Commands.CreateTeam;
using Application.UseCases.Teams.Commands.DeleteTeam;
using Application.UseCases.Teams.Commands.UpdateTeam;
using Application.UseCases.Teams.Queries.GetTeamById;
using Application.UseCases.Teams.Queries.GetTeams;
using Application.UseCases.Teams.Queries.GetTeamsLookup;
using Application.UseCases.Users.Commands.ActivateUserFromToken;
using Application.UseCases.Users.Commands.CreateMentor;
using Application.UseCases.Users.Commands.DeleteUser;
using Application.UseCases.Users.Commands.UpdateCandidate;
using Application.UseCases.Users.Commands.UpdateUser;
using Application.UseCases.Users.Queries.GetUserById;
using Application.UseCases.Users.Queries.GetUserByTelegramId;
using Application.UseCases.Users.Queries.GetUsers;
using Application.UseCases.Users.Queries.GetUsersLookup;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication( this IServiceCollection services )
    {
        services.AddScoped<ICommandHandlerWithResult<CreateCountryCommand, Country>, CreateCountryCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateCountryCommand, Country>, UpdateCountryCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteCountryCommand, string>, DeleteCountryCommandHandler>();
        services.AddScoped<IQueryHandler<PagedResult<Country>, GetCountriesQuery>, GetCountriesQueryHandler>();
        services.AddScoped<IQueryHandler<List<Country>, GetCountriesLookupQuery>, GetCountriesLookupQueryHandler>();
        services.AddScoped<IQueryHandler<Country, GetCountryByIdQuery>, GetCountryByIdQueryHandler>();

        services.AddScoped<ICommandHandlerWithResult<CreateCityCommand, City>, CreateCityCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateCityCommand, City>, UpdateCityCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteCityCommand, string>, DeleteCityCommandHandler>();
        services.AddScoped<IQueryHandler<PagedResult<City>, GetCitiesQuery>, GetCitiesQueryHandler>();
        services.AddScoped<IQueryHandler<List<City>, GetCitiesLookupQuery>, GetCitiesLookupQueryHandler>();
        services.AddScoped<IQueryHandler<City, GetCityByIdQuery>, GetCityByIdQueryHandler>();

        services.AddScoped<ICommandHandlerWithResult<CreateDepartmentCommand, Department>, CreateDepartmentCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateDepartmentCommand, Department>, UpdateDepartmentCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteDepartmentCommand, string>, DeleteDepartmentCommandHandler>();
        services.AddScoped<IQueryHandler<PagedResult<Department>, GetDepartmentsQuery>, GetDepartmentsQueryHandler>();
        services.AddScoped<IQueryHandler<List<Department>, GetDepartmentsLookupQuery>, GetDepartmentsLookupQueryHandler>();
        services.AddScoped<IQueryHandler<Department, GetDepartmentByIdQuery>, GetDepartmentByIdQueryHandler>();

        services.AddScoped<ICommandHandlerWithResult<CreateTeamCommand, Team>, CreateTeamCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateTeamCommand, Team>, UpdateTeamCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteTeamCommand, string>, DeleteTeamCommandHandler>();
        services.AddScoped<IQueryHandler<PagedResult<Team>, GetTeamsQuery>, GetTeamsQueryHandler>();
        services.AddScoped<IQueryHandler<List<Team>, GetTeamsLookupQuery>, GetTeamsLookupQueryHandler>();
        services.AddScoped<IQueryHandler<Team, GetTeamByIdQuery>, GetTeamByIdQueryHandler>();

        services.AddScoped<ICommandHandlerWithResult<CreateOnboardingAccessRequestCommand, OnboardingAccessRequest>, CreateOnboardingAccessRequestCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteOnboardingAccessRequestCommand, string>, DeleteOnboardingAccessRequestCommandHandler>();
        services.AddScoped<IQueryHandler<PagedResult<OnboardingAccessRequest>, GetOnboardingAccessRequestsQuery>, GetOnboardingAccessRequestsQueryHandler>();
        services.AddScoped<ICommandHandlerWithResult<ConfirmOnboardingAccessRequestCommand, OnboardingAccessRequest>, ConfirmOnboardingAccessRequestCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<RejectOnboardingAccessRequestCommand, OnboardingAccessRequest>, RejectOnboardingAccessRequestCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<RestoreOnboardingAccessRequestCommand, OnboardingAccessRequest>, RestoreOnboardingAccessRequestCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateOnboardingAccessRequestCommand, OnboardingAccessRequest>, UpdateOnboardingAccessRequestCommandHandler>();
        services.AddScoped<IQueryHandler<OnboardingAccessRequest, GetOnboardingAccessRequestByCandidateIdQuery>, GetOnboardingAccessRequestByCandidateIdQueryHandler>();
        services.AddScoped<IQueryHandler<List<User>, GetDueCandidatesQuery>, GetDueCandidatesQueryHandler>();

        services.AddScoped<ICommandHandlerWithResult<CreateFeedbackCommand, Feedback>, CreateFeedbackCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<ConfirmFeedbackCommand, Feedback>, ConfirmFeedbackCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteFeedbackCommand, string>, DeleteFeedbackCommandHandler>();
        services.AddScoped<IQueryHandler<PagedResult<Feedback>, GetFeedbacksQuery>, GetFeedbacksQueryHandler>();
        services.AddScoped<IQueryHandler<Feedback, GetDraftFeedbackQuery>, GetDraftFeedbackQueryHandler>();
        services.AddScoped<ICommandHandlerWithResult<ExportFeedbacksCommand, byte[]>, ExportFeedbacksCommandHandler>();

        services.AddScoped<ICommandHandlerWithResult<CreateMentorCommand, User>, CreateMentorCommandHandler>();
        services.AddScoped<IQueryHandler<User, GetUserByTelegramIdQuery>, GetUserByTelegramIdQueryHandler>();

        services.AddScoped<ICommandHandlerWithResult<CreateCandidateInviteCommand, AccountCreationToken>, CreateCandidateInviteCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<CreateHRInviteCommand, AccountCreationToken>, CreateHRInviteCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<RevokeTokenCommand, AccountCreationToken>, RevokeTokenCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteTokenCommand, string>, DeleteTokenCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<MarkTokenAsExpiredCommand, AccountCreationToken>, MarkTokenAsExpiredCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateTokenCommand, AccountCreationToken>, UpdateTokenCommandHandler>();
        services.AddScoped<IQueryHandler<List<AccountCreationToken>, GetExpiredTokensQuery>, GetExpiredTokensQueryHandler>();
        services.AddScoped<IQueryHandler<PagedResult<AccountCreationToken>, GetAccountCreationTokensQuery>, GetAccountCreationTokensQueryHandler>();
        services.AddScoped<IQueryHandler<AccountCreationToken, GetAccountCreationTokenByValueQuery>, GetAccountCreationTokenByValueQueryHandler>();

        services.AddScoped<ICommandHandlerWithResult<ActivateUserFromTokenCommand, User>, ActivateUserFromTokenCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateCandidateCommand, User>, UpdateCandidateCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<UpdateUserCommand, User>, UpdateUserCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<DeleteUserCommand, string>, DeleteUserCommandHandler>();
        services.AddScoped<IQueryHandler<User, GetUserByIdQuery>, GetUserByIdQueryHandler>();

        services.AddScoped<IAuthTokenService, AuthTokenService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IQrCodeGeneratorService, QrCodeGeneratorService>();

        services.AddScoped<ICommandHandlerWithResult<GoNextStepCommand, CandidateProcess>, GoNextStepCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<ResetCandidateProcessCommand, CandidateProcess>, ResetCandidateProcessCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<TransferToOnboardingCommand, CandidateProcess>, TransferToOnboardingCommandHandler>();
        services.AddScoped<ICommandHandlerWithResult<TransferToPersonalAreaCommand, CandidateProcess>, TransferToPersonalAreaCommandHandler>();
        services.AddScoped<IQueryHandler<CandidateProcess, GetCandidateProcessQuery>, GetCandidateProcessQueryHandler>();
        services.AddScoped<IQueryHandler<CandidateProcess, GetActiveCandidateProcessQuery>, GetActiveCandidateProcessQueryHandler>();
        services.AddScoped<IQueryHandler<PagedResult<User>, GetUsersQuery>, GetUsersQueryHandler>();
        services.AddScoped<IQueryHandler<List<User>, GetUsersLookupQuery>, GetUsersLookupQueryHandler>();

        return services;
    }
}
