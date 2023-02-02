using System.Collections.Generic;
using System.Management.Automation;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить правила обработки почты
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Rules"), OutputType(typeof(List<Rule>))]
    public class GetRulesCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Rule> result = APIClient.GetRulesAsync().Result;
            WriteObject(result);
            base.EndProcessing();
        }
    }
}
