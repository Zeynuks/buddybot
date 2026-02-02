using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.UseCases.Feedbacks.Commands.ExportFeedbacks;
public class ExportFeedbacksCommandHandler(
    IFeedbackRepository feedbackRepository,
    ILogger<ExportFeedbacksCommand> logger )
    : CommandBaseHandlerWithResult<ExportFeedbacksCommand, byte[]>( logger )
{
    protected override async Task<Result<byte[]>> HandleImplAsync( ExportFeedbacksCommand command )
    {
        List<Feedback> feedbacks = await feedbackRepository.GetAll();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine( "Id;Имя кандидата;Тип этапа;Оценка;Комментарий;Дата создания" );

        foreach ( Feedback feedback in feedbacks )
        {
            var firstName = feedback.Candidate?.ContactInfo?.FirstName ?? "";
            var lastName = feedback.Candidate?.ContactInfo?.LastName ?? "";
            var candidateFullName = $"{firstName} {lastName}".Trim();

            string processKind = feedback.ProcessKind.ToString();
            string rating = feedback.Rating.ToString();

            string comment = feedback.Comment?.Replace( "\"", "\"\"" ) ?? "";
            string createdAt = feedback.CreatedAtUtc.ToString( "yyyy-MM-dd HH:mm:ss" );

            sb.AppendLine( $"{feedback.Id};\"{candidateFullName}\";{processKind};{rating};\"{comment}\";{createdAt}" );
        }
        var utf8WithBom = new UTF8Encoding( true );
        var bytes = utf8WithBom.GetBytes( sb.ToString() );
        return Result<byte[]>.FromSuccess( bytes );
    }
}
