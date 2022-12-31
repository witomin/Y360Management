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
    /// <summary>
    /// Изменить параметры подразделения
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "Department"), OutputType(typeof(Department))]
    public class SetDepartmentCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр Identity определяет подразделение, которое требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - email
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Identity { get; set; }
        /// <summary>
        /// Описание подразделения
        /// </summary>
        [Parameter(Position = 1)]
        public string? Description { get; set; }
        /// <summary>
        /// Произвольный внешний идентификатор подразделения
        /// </summary>
        [Parameter(Position = 2)]
        public string? ExternalId { get; set; }
        /// <summary>
        /// Сотрудник-руководител подразделения
        /// </summary>
        [Parameter(Position = 3)]
        public string? Head { get; set; }
        /// <summary>
        /// Имя почтовой рассылки подразделения. Например, для адреса new-department@ваш-домен.ru имя почтовой рассылки — это new-department.
        /// </summary>
        [Parameter(Position = 4)]
        public string? Label { get; set; }
        /// <summary>
        /// Название подразделения
        /// </summary>
        [Parameter(Position = 5)]
        public string? Name { get; set; }
        /// <summary>
        /// Родительское подразделение
        /// </summary>
        [Parameter(Position = 6)]
        public string? Parent { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Department> allDepartments = APIClient.GetAllDepartmentsAsync().Result;
            Department department = allDepartments.SingleOrDefault(u => u.id.ToString().Equals(Identity) || u.email.ToLower().Equals(Identity.ToLower()));
            if (department is null) {
                base.EndProcessing();
                return;
            }
            if (Description != null) department.description = Description;
            if (ExternalId != null) department.externalId = ExternalId;
            if (Head != null) {
                var allUsers = APIClient.GetAllUsersAsync().Result;
                department.headId = allUsers.SingleOrDefault(u => u.id.ToString().Equals(Head) || u.nickname.Equals(Head) || u.email.Equals(Head)).id;
            }
            if (Label != null) department.label = Label;
            if (Name != null) department.name = Name;
            if (Parent != null) {
                department.headId = allDepartments.SingleOrDefault(d => d.id.ToString().Equals(Parent) || d.email.Equals(Parent)).id;
            }
            var res = APIClient.EditDepartmentAsync(department).Result;
            WriteObject(department);
            base.EndProcessing();
        }
    }
}
