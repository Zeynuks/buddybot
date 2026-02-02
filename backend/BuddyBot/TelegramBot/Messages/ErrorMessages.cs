namespace TelegramBot.Messages;
public static class ErrorMessages
{
    public const string TelegramUserNotFound = "🛑 Не удалось определить пользователя Telegram.";
    public const string RegistrationTokenRequired = "🔒 Привет! Похоже, ты пришёл без приглашения 😊 Попроси HR выдать тебе токен, и я сразу всё расскажу!";
    public const string InvalidTokenFormat = "🛑 Указан некорректный формат токена. Проверь ссылку-приглашение, или попроси помощи у HR.";
    public const string ActivationFailed = "🛑 Не получилось активировать приглашение. Проверь, что у тебя правильный токен, или попроси помощи у HR.";
    public const string CandidateNotFound = "🔒 Кажется, я тебя не нашёл в системе. Если уверен, что всё сделал правильно, напиши HR — они помогут разобраться! 😊";
    public const string HeadNotFound = "🛑 К сожалению, мне не удалось найти информацию о твоём руководителе. Пожалуйста, свяжись с HR или напиши в поддержку — мы всё быстро исправим! 😊";
    public const string MentorNotFound = "🛑 К сожалению, мне не удалось найти информацию о твоём наставнике. Пожалуйста, свяжись с HR или напиши в поддержку — мы всё быстро исправим! 😊";

    public const string FeedbackCreationFailed = "🛑 К сожалению, не удалось создать черновик отзыва. Попробуй ещё раз или напиши HR, если ошибка повторится!";
    public const string FeedbackConfirmationFailed = "🛑 Не удалось подтвердить отзыв. Попробуй ещё раз или обратись к HR за помощью!";
}
