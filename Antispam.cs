using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Y360Management.Types;

namespace Y360Management {
    /// <summary>
    /// Получить список разрешенных IP-адресов и CIDR-подсетей.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AllowList"), OutputType(typeof(List<string>))]
    public class GetAllowListCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<string> result = APIClient.GetAllowListAsync().Result;
            WriteObject(result);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Задать список разрешенных IP-адресов и CIDR-подсетей.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AllowList")]
    public class SetAllowListCmdlet : PSCmdlet {
        [Parameter(Position = 0)]
        public AloowListCollection? Items { get; set; }
        [Parameter(Position = 1)]
        public List<string>? AllowList { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            if (Items != null) {
                var allowList = APIClient.GetAllowListAsync().Result;
                Thread.Sleep(1000);
                if (Items.Add != null) {
                    allowList.AddRange(Items.Add);
                    allowList = allowList.Distinct().ToList();
                }
                if (Items.Remove != null) {
                    allowList = allowList.Where(i => !Items.Remove.Contains(i)).ToList();
                }
                if (allowList.Count == 0) {
                    var result = APIClient.DeleteAllowListAsync().Result;
                }
                else {
                    var result = APIClient.SetAllowListAsync(allowList).Result;
                }
            }
            else 
                if (AllowList != null) {
                var result = APIClient.SetAllowListAsync(AllowList).Result;
            }
            base.EndProcessing();
        }
    }

    /// <summary>
    /// Удалить список разрешенных IP-адресов и CIDR-подсетей.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AllowList")]
    public class RemoveAllowListCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            var result = APIClient.DeleteAllowListAsync().Result;
            base.EndProcessing();
        }
    }
}
