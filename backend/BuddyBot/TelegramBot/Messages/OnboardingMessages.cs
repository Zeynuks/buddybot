namespace TelegramBot.Messages;
public static class OnboardingMessages
{
    public static string OnboardingPendingStart( string name )
    {
        return $@"<b>{name}</b>, привет! 👋
Приветствую тебя в твой первый рабочий день в TL! Добро пожаловать на онбординг. Теперь я помогу тебе погрузиться в рабочие процессы, познакомиться с командой и освоить все корпоративные инструменты.";
    }

    public static string MeetTeamIntro()
    {
        return @"В компании TravelLine работает множество специалистов, и чтобы у тебя было понимание, к кому можно обращаться, я поделюсь информацией о ключевых коллегах.";
    }

    public static string MeetHead( string headName, string teamsUrl )
    {
        if ( Uri.IsWellFormedUriString( teamsUrl, UriKind.Absolute ) )
        {
            return $@"Твой руководитель: <b>{headName}</b>
<a href=""{teamsUrl}"">Перейти в Microsoft Teams</a>";
        }
        else
        {
            return $@"Твой руководитель: <b>{headName}</b>";
        }
    }

    public static string MeetMentor( string mentorName, string teamsUrl )
    {
        if ( Uri.IsWellFormedUriString( teamsUrl, UriKind.Absolute ) )
        {
            return $@"Твой наставник: <b>{mentorName}</b>
<a href=""{teamsUrl}"">Перейти в Microsoft Teams</a>";
        }
        else
        {
            return $@"Твой наставник: <b>{mentorName}</b>";
        }
    }


    public static string CompanyPolicies()
    {
        return @"Чтобы работать в команде было комфортно, важно знать наши ценности и основные корпоративные регламенты:

1. Для начала работы ознакомься с памяткой нового сотрудника техотдела: <a href=""https://confluence.travelline.lan/pages/viewpage.action?pageId=167249484"">ссылка на Confluence</a>.

2. Сегодня на твою рабочую почту придет ссылка на общие курсы в <a href=""https://learn.travelline.ru/courses"">iLearn</a> (охрана труда, безопасное обращение с паролями и др.) — <b>пройди их в течение <u>2 рабочих дней</u></b>.

3. В ближайшие дни на твою рабочую почту также придет ссылка в <a href=""https://learn.travelline.ru/courses"">iLearn</a> для прохождения вводного курса по техотделу.

Если возникнут вопросы, не стесняйся обращаться к своему наставнику или коллегам из отдела. Удачи в новой роли!";
    }

    public static string OnboardingReminder( string name )
    {
        return $@"Привет, <b>{name}</b>! 👋
Ты ещё не начал(а) онбординг, а это важный шаг для твоего старта в нашей команде!  
Не переживай, у тебя есть возможность начать прямо сейчас.

Просто нажми кнопку <b>«Начать онбординг»</b>, и я всё подготовлю для тебя!";
    }

}
