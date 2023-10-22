namespace RegistrantApplication.Server.Configs;

public static class ConfigMsg
{
    /* SECURITY */
    public const string UnauthorizedInvalidToken = "Срок действия токена истёк. Обратитесь к сис.админу";

    /* PAGINATION  */
    public const string PaginationError = "Текущая страница за пределами диапозона. Обратитесь к сис.админу";
    
    /* VALIDATION */
    public const string ValidationTextEmpty = "Обязательные поля оказались пустыми";
    public const string ValidationElementNotFound = "Элемент не найден";
    public const string ValidationElementtExist = "Элемент уже существует";
}