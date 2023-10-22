using RegistrantApplication.Shared.Contragents;

namespace RegistrantApplication.Shared;

public interface IModal
{
    
    /// <summary>
    /// ID модального окна
    /// </summary>
    string IdModal { get; set; }
    /// <summary>
    /// Показано ли окно
    /// </summary>
    bool IsDisplay { get; set; }
    
    /// <summary>
    /// Заголовок модального окна
    /// </summary>
    string PropertyTitle { get; set; }
    /// <summary>
    /// Атрибуты заднего фона (анимация, появления TailWind CSS
    /// </summary>
    string PropertySmoothShowBackground { get; set; }
    /// <summary>
    /// Атрибуты модального окна (анимация, появления TailWind CSS
    /// </summary>
    string PropertySmoothShowModal{ get; set; }
    /// <summary>
    /// Атрибуты для размытия заднего фона (анимация, появления TailWind CSS
    /// </summary>
    string PropertyBlur { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    string PropertyErrorMsg { get; set; }
    
    /// <summary>
    /// Отправка формы
    /// </summary>
    void SubmitAsync();

    /// <summary>
    /// Показать форму формы
    /// </summary>
    /// <param name="contragent"></param>
    /// <param name="value"></param>
    Task<bool> ShowAsync();
    Task<bool> ShowAsync(object data);
    /// <summary>
    /// Ожидание результата диалога
    /// </summary>
    /// <returns></returns>
    Task<bool> WaitForResult();
    /// <summary>
    /// Закрыть
    /// </summary>
    void CloseAsync();



    
}