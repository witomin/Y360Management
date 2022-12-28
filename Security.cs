using System.Management.Automation;
using Yandex.API360.Models;

namespace Y360Management {
    [Cmdlet(VerbsCommon.Get, "Status2FA"), OutputType(typeof(DomainStatus2FA))]
    public class GetStatus2FaCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            DomainStatus2FA result = APIClient.GetStatus2faAsync().Result;
            WriteObject(result);
            base.EndProcessing();
        }
    }
}
