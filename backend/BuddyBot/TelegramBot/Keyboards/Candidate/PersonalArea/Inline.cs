using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Keyboards.Candidate.PersonalArea;
public static class Inline
{
    public static InlineKeyboardMarkup PersonalAreaHome()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Основные контакты",
                    callbackData: "PersonalAreaContacts"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "FAQ",
                    callbackData: "PersonalAreaFAQ"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Полезные ссылки",
                    callbackData: "PersonalAreaLinks"
                )
            }
        } );
    }

    public static InlineKeyboardMarkup PersonalAreaLinks()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "💼 HR-платформа Пульс",
                    url: "https://live.travelline.ru/"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🛠 Центр поддержки по рабочим вопросам",
                    url: "https://jira.travelline.ru/servicedesk/customer/portals"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🖥 Поставить задачу системному админу",
                    url: "https://admindesk.travelline.lan/"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🔐 Телеграм-бот 'Дежурный сисадмин'",
                    url: "https://t.me/tl_admin_on_duty_bot"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🔑 Сервис по сохранению паролей",
                    url: "https://passwork.travelline.lan/"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🥗 Сервис заказа еды (офис)",
                    url: "http://food.travelline.lan/?source=email"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🤖 Корпоративный ChatGPT",
                    url: "https://chat.travelline.lan/"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🛍 Маркетплейс мерча",
                    url: "https://tlmarket.travelline.lan/"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🌐 Наша группа в VK",
                    url: "https://vk.com/yourtravelline"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "🚫 Запрещенная соцсеть",
                    url: "https://www.instagram.com/tl_people_"
                )
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "⬅️ Назад",
                    callbackData: "PersonalAreaHome"
                )
            }
    } );
    }

    public static InlineKeyboardMarkup PersonalAreaFaq()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("💰", "FaqSalary"),
                InlineKeyboardButton.WithCallbackData("💸", "FaqSalaryReview"),
                InlineKeyboardButton.WithCallbackData("📈", "FaqPromotion")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🎁", "FaqBenefitsSelectLocation"),
                InlineKeyboardButton.WithCallbackData("🏠", "FaqRemote"),
                InlineKeyboardButton.WithCallbackData("🏢", "FaqOffice")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("💻", "FaqEquipment"),
                InlineKeyboardButton.WithCallbackData("🗣", "FaqCommunication"),
                InlineKeyboardButton.WithCallbackData("🌴", "FaqVacation")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🤒", "FaqSickLeave"),
                InlineKeyboardButton.WithCallbackData("🎉", "FaqCorporate"),
                InlineKeyboardButton.WithCallbackData("✈️", "FaqBusinessTrip")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("⬅️ Назад", "PersonalAreaHome")
            }
    } );
    }

    public static InlineKeyboardMarkup FaqBenefitsSelectLocation()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🏢 Я работаю в Йошкар-Оле", "FaqBenefitsOffice"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🏡 Я работаю удалённо", "FaqBenefitsRemote"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("⬅️ Назад", "PersonalAreaFAQ"),
            }

        } );
    }

    public static InlineKeyboardMarkup PersonalAreaFaqBackHome()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("⬅️ Назад", "PersonalAreaFAQ"),
                InlineKeyboardButton.WithCallbackData("🏠 В личный кабинет", "PersonalAreaHome")
            }
        } );
    }

    public static InlineKeyboardMarkup FaqBenefitsBackHome()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("⬅️ Назад", "FaqBenefitsSelectLocation"),
                InlineKeyboardButton.WithCallbackData("🏠 Личный кабинет", "PersonalAreaHome")
            }
        } );
    }

    public static InlineKeyboardMarkup PersonalAreaContacts()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Твой руководитель", "ContactHead") },
            new[] { InlineKeyboardButton.WithCallbackData("Твой наставник", "ContactMentor") },
            new[] { InlineKeyboardButton.WithCallbackData("По расчетным листкам", "ContactSalary") },
            new[] { InlineKeyboardButton.WithCallbackData("По премиям", "ContactBonuses") },
            new[] { InlineKeyboardButton.WithCallbackData("По обучению", "ContactLearning") },
            new[] { InlineKeyboardButton.WithCallbackData("По курьерской доставке", "ContactDelivery") },
            new[] { InlineKeyboardButton.WithCallbackData("По вопросам воинского учёта и отпусков", "ContactArmyVacation") },
            new[] { InlineKeyboardButton.WithCallbackData("По оформлению командировки", "ContactBusinessTrip") },
            new[] { InlineKeyboardButton.WithCallbackData("Бенефиты: спорт, ДМС и питание", "ContactBenefits") },
            new[] { InlineKeyboardButton.WithCallbackData("⬅️ Назад", "PersonalAreaHome") }
        } );
    }

    public static InlineKeyboardMarkup ContactCardKeyboard( string? teamsUrl = null )
    {
        if ( !string.IsNullOrWhiteSpace( teamsUrl ) && Uri.IsWellFormedUriString( teamsUrl, UriKind.Absolute ) && !teamsUrl.Contains( "localhost" ) )
        {
            return new InlineKeyboardMarkup( new[]
            {
            new[]
            {
                InlineKeyboardButton.WithUrl("✉️ Написать в Teams", teamsUrl)
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("⬅️ Назад", "PersonalAreaContacts"),
                InlineKeyboardButton.WithCallbackData("🏠 Личный кабинет", "PersonalAreaHome")
            }
        } );
        }

        return new InlineKeyboardMarkup( new[]
        {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("⬅️ Назад", "PersonalAreaContacts"),
            InlineKeyboardButton.WithCallbackData("🏠 Личный кабинет", "PersonalAreaHome")
        }
    } );
    }

}
