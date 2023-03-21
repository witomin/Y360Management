using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Y360Management.Types;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить информацию о подразделениях
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Departments"), OutputType(typeof(List<Department>))]
    public class GetDepartmentsCmdlet : PSCmdlet {
        /// <summary>
        /// Параметр Identity определяет подразделение, которое требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет подразделение:
        /// - Id
        /// - email
        /// </summary>
        [Parameter(Position = 0)]
        public string? Identity { get; set; }
        /// <summary>
        /// Фильтр. Выполняется поиск по вхождению строки в свойствах name, email, description.
        /// </summary>
        [Parameter(Position = 1)]
        public string? Filter { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Department> Departments = APIClient.GetAllDepartmentsAsync().Result;
            if (Identity != null) {
                Departments = Departments.Where(d => d.id.ToString().Equals(Identity) || d.email.ToLower().Equals(Identity.ToLower())).ToList();
            }
            if (Filter != null) {
                Departments = Departments.Where(d => d.email.ToLower().Contains(Filter.ToLower()) ||
                d.name.ToLower().Contains(Filter.ToLower()) ||
                d.description.ToLower().Contains(Filter.ToLower())
                ).ToList();
            }
            WriteObject(Departments, true);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Изменить параметры подразделения
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "Department"), OutputType(typeof(Department))]
    public class SetDepartmentCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр Identity определяет подразделение, которое требуется изменить.
        /// Можно использовать любое значение, которое однозначно определяет подразделение:
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
        /// <summary>
        /// Параметр для управления алиасами
        /// </summary>
        [Parameter(Position = 7)]
        public StringCollection? Aliases { get; set; }
        /// <summary>
        /// Список алиасов, не работает совместно с параметром Aliases
        /// </summary>
        [Parameter(Position = 8)]
        public List<string>? AliasList { get; set; }
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
                department.parentId = allDepartments.SingleOrDefault(d => d.id.ToString().Equals(Parent) || d.email.Equals(Parent)).id;
            }
            if (Aliases != null) {
                if (Aliases.Add != null) {
                    foreach (var alias in Aliases.Add) {
                        if (!department.aliases.Contains(alias)) {
                            var result = APIClient.AddAliasToDepartmentAsync(department.id, alias).Result;
                        }
                    }
                }
                if (Aliases.Remove != null) {
                    foreach (var alias in Aliases.Remove) {
                        if (department.aliases.Contains(alias)) {
                            var result = APIClient.DeleteAliasFromDepartmentAsync(department.id, alias).Result;
                        }
                    }
                }
            }
            else if (AliasList != null) {
                var add = AliasList.Where(a => !department.aliases.Contains(a));
                var remove = department.aliases.Where(a => !AliasList.Contains(a));
                foreach (var alias in add) {
                    var result = APIClient.AddAliasToDepartmentAsync(department.id, alias).Result;
                }
                foreach (var alias in remove) {
                    var result = APIClient.DeleteAliasFromDepartmentAsync(department.id, alias).Result;
                }
            }
            var res = APIClient.EditDepartmentAsync(department).Result;
            WriteObject(department);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Удалить подразделение
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "Department")]
    public class RemoveDepartmentCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр Identity определяет подразделение, которое требуется удалить.
        /// Можно использовать любое значение, которое однозначно определяет подразделение:
        /// - Id
        /// - email
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Identity { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Department> allDepartments = APIClient.GetAllDepartmentsAsync().Result;
            var department = allDepartments.SingleOrDefault(u => u.id.ToString().Equals(Identity) || u.email.ToLower().Equals(Identity.ToLower()));
            if (department != null) {
                var res = APIClient.DeleteDepartmentAsync(department.id).Result;
            }
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Создать подразделение
    /// </summary>
    [Cmdlet(VerbsCommon.New, "Department"), OutputType(typeof(Department))]
    public class NewDepartmentCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр название подразделения
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string? Name { get; set; }
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
        /// Родительское подразделение
        /// </summary>
        [Parameter(Position = 5)]
        public string? Parent { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            BaseDepartment department = new BaseDepartment {
                name = Name,
                description = Description,
                externalId = ExternalId,
                label = Label,
            };
            if (Head != null) {
                var allUsers = APIClient.GetAllUsersAsync().Result;
                department.headId = allUsers.SingleOrDefault(u => u.id.ToString().Equals(Head) || u.nickname.Equals(Head) || u.email.Equals(Head)).id;
            }

            if (Parent != null) {
                List<Department> allDepartments = APIClient.GetAllDepartmentsAsync().Result;
                department.parentId = allDepartments.SingleOrDefault(d => d.id.ToString().Equals(Parent) || d.email.Equals(Parent)).id;
            }
            var result = APIClient.AddDepartmentAsync(department).Result;
            WriteObject(result);
            base.EndProcessing();
        }
    }
}
