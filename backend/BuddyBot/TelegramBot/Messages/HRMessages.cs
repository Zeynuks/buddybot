namespace TelegramBot.Messages;
public static class HRMessages
{
    public static string Welcome( string name )
    {
        return $"Привет, <b>{name}</b>! Рад тебя видеть здесь, в роли HR! 😊";
    }

    public static string NewCandidateNotification( string candidateName )
    {
        return $"👤➕ Новый кандидат: <b>{candidateName}</b> только что зарегистрировался и начал пребординг.";
    }

    public static string OnboardingAccessRequestedNotification( string candidateName )
    {
        return $"🔔 Новый сотрудник <b>{ candidateName }</b> завершил процесс пребординга и запросил доступ к этапу онбординга. Пожалуйста, подтвердите доступ.";
    }

    public static string OfferAcceptedNotification( string candidateName )
    {
        return $"✅ Кандидат <b>{candidateName}</b> принял оффер!";
    }
}
