using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить информацию о подразделениях
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Departments"), OutputType(typeof(List<Department>))]
    public class GetDepartmentsCmdlet : PSCmdlet {
        /// <summary>
        /// Параметр Identity определяет подразделение, которое требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - email
        /// </summary>
        [Parameter(Position = 0)]
        public string? Identity { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Department> result = APIClient.GetAllDepartmentsAsync().Result;
            if (Identity != null) {
                result = result.Where(u => u.id.ToString().Equals(Identity) || u.email.ToLower().Equals(Identity.ToLower())).ToList();
            }
            WriteObject(result);
            base.EndProcessing();
        }
    }
}
