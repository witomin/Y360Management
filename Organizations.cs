using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить информацию об организациях
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Organizations"), OutputType(typeof(List<Organization>))]
    public class GetOrganizationsCmdlet : PSCmdlet {
        /// <summary>
        /// Фильтр. Выполняется поиск по вхождению строки в свойствах name, email.
        /// </summary>
        [Parameter(Position = 0)]
        public string? Filter { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Organization> result = APIClient.GetOrganizationsAsync(pageSize: 100).Result;
            if (Filter != null) {
                result = result.Where(o => o.name.ToLower().Contains(Filter.ToLower()) ||
                o.email.ToLower().Contains(Filter.ToLower())
                ).ToList();
            }
            WriteObject(result);
            base.EndProcessing();
        }
    }
}
