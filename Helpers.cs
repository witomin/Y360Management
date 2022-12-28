using System;
using System.Management.Automation;
using Yandex.API360;

namespace Y360Management {
    public static class Helpers {
        public static Client GetApiClient(PSCmdlet Cmdlet) {
            Client APIClient = (Client)Cmdlet.SessionState.PSVariable.GetValue("APIClient");
            if (APIClient is null) {
                throw new Exception("Не установлено соединение с сервисом Y360. Выполните команду Connect-Y360");
            }
            return APIClient;
        }
    }
}
