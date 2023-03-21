using System.Management.Automation;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Возвращает статус обязательной двухфакторной аутентификации (2FA) для пользователей домена.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Status2FA", HelpUri = "https://github.com/witomin/Y360Management#get-status2fa---получить-статус-2fa"), OutputType(typeof(DomainStatus2FA))]
    public class GetStatus2FaCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            DomainStatus2FA result = APIClient.GetStatus2faAsync().Result;
            WriteObject(result);
            base.EndProcessing();
        }
    }
    [Cmdlet(VerbsCommon.Set, "Status2FA")]
    public class SetStatus2FaCmdlet : PSCmdlet {
        /// <summary>
        /// Период (в секундах), в течение которого при включенной 2FA пользователю в процессе авторизации
        /// предлагается настроить 2FA с возможностью пропустить этот шаг. По истечении периода 
        /// возможность отложить настройку 2FA отключается.
        /// </summary>
        [Parameter(Position = 0)]
        public int? Duration { get; set; }
        /// <summary>
        /// Признак, что при включении 2FA всех пользователей домена нужно единоразово разлогинить.
        /// Если использовались пароли приложений, потребуется создать их заново.
        /// </summary>
        [Parameter(Position = 1)]
        public SwitchParameter LogoutUsers { get; set; }
        /// <summary>
        /// Выключить обязательную двухфакторную аутентификации (2FA) для пользователей домена
        /// не работает совместно с параметрами Duration и LogoutUsers
        /// </summary>
        [Parameter(Position = 2)]
        public SwitchParameter Disable { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            DomainStatus2FA result;
            if (Duration != null) {
                result = APIClient.Enable2faAsync(new EnableDomainStatus2FA { duration = (int)Duration, logoutUsers = LogoutUsers}).Result; 
            }
            else 
            if (Disable) {
                result = APIClient.Disable2faAsync().Result;
            }
            base.EndProcessing();
        }
    }
}
