using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Y360Management.Types;

namespace Y360Management {
    /// <summary>
    /// Получить список разрешенных IP-адресов и CIDR-подсетей.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AllowList", HelpUri = "https://github.com/witomin/Y360Management#get-allowlist---получить-информацию-о-белом-списке"), OutputType(typeof(List<string>))]
    public class GetAllowListCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<string> result = APIClient.GetAllowListAsync().GetAwaiter().GetResult();
            WriteObject(result, true);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Задать список разрешенных IP-адресов и CIDR-подсетей.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AllowList", HelpUri = "https://github.com/witomin/Y360Management#set-allowlist---редактировать-белый-список")]
    public class SetAllowListCmdlet : PSCmdlet {
        /// <summary>
        /// Управление списком разрешенных IP-адресов и CIDR-подсетей
        /// </summary>
        [Parameter(Position = 0)]
        public StringCollection? Items { get; set; }
        /// <summary>
        /// Список разрешенных IP-адресов и CIDR-подсетей
        /// </summary>
        [Parameter(Position = 1)]
        public List<string>? AllowList { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            if (Items != null) {
                var allowList = APIClient.GetAllowListAsync().GetAwaiter().GetResult();
                Thread.Sleep(1000);
                if (Items.Add != null) {
                    allowList.AddRange(Items.Add);
                    allowList = allowList.Distinct().ToList();
                }
                if (Items.Remove != null) {
                    allowList = allowList.Where(i => !Items.Remove.Contains(i)).ToList();
                }
                if (allowList.Count == 0) {
                    var result = APIClient.DeleteAllowListAsync().GetAwaiter().GetResult();
                }
                else {
                    var result = APIClient.SetAllowListAsync(allowList).GetAwaiter().GetResult();
                }
            }
            else 
                if (AllowList != null) {
                var result = APIClient.SetAllowListAsync(AllowList).GetAwaiter().GetResult();
            }
            base.EndProcessing();
        }
    }

    /// <summary>
    /// Удалить список разрешенных IP-адресов и CIDR-подсетей.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AllowList", HelpUri = "https://github.com/witomin/Y360Management#remove-allowlist---удалить-белый-список")]
    public class RemoveAllowListCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            var result = APIClient.DeleteAllowListAsync().GetAwaiter().GetResult();
            base.EndProcessing();
        }
    }
}
