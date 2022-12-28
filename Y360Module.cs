using System.Management.Automation;
using Yandex.API360;

namespace Y360Management {
    /// <summary>
    /// Подключиться к сервису Y360
    /// </summary>
    [Cmdlet("Connect", "Y360"), OutputType(typeof(Client))]
    public class ConnectY360Cmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр идентификатор организации
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string OrgId { get; set; }
        /// <summary>
        /// Обязательный параметр токен авторизации
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string APIToken { get; set; }
        protected override void EndProcessing() {
            var APIClient = new Client(new Api360Options(OrgId, APIToken));
            PSVariable APIClientVariable = new PSVariable("APIClient", APIClient, ScopedItemOptions.Private);
            APIClientVariable.Visibility = SessionStateEntryVisibility.Private;
            SessionState.PSVariable.Set(APIClientVariable);
            base.EndProcessing();
        }
    }
}
