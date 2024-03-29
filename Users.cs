﻿using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Y360Management.Types;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить информацию о сотрудниках
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Users", HelpUri = "https://github.com/witomin/Y360Management#get-users---получить-информацию-о-сотрудниках"), OutputType(typeof(List<User>))]
    public class GetUsersCmdlet : PSCmdlet {
        /// <summary>
        /// Параметр Identity определяет пользователя, которого требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - nickname
        /// - email
        /// </summary>
        [Parameter(Position = 0)]
        public string? Identity { get; set; }
        /// <summary>
        /// Фильтр. Выполняется поиск по вхождению строки в свойствах name, nickname, position, email.
        /// </summary>
        [Parameter(Position = 1)]
        public string? Filter { get; set; }
        /// <summary>
        /// Параметр EnableOnly указывает, что нужно показать только активных пользователей
        /// </summary>
        [Parameter(Position = 2)]
        public SwitchParameter EnableOnly { get; set; }
        /// <summary>
        /// Параметр DisableOnly указывает, что нужно показать только неактивных пользователей
        /// </summary>
        [Parameter(Position = 3)]
        public SwitchParameter DisableOnly { get; set; }
        /// <summary>
        /// Параметр AdminOnly указывает, что нужно показать только администраторов
        /// </summary>
        [Parameter(Position = 4)]
        public SwitchParameter AdminOnly { get; set; }
        /// <summary>
        /// Параметр RobotOnly указывает, что нужно показать только пользователей, явзяющихся роботами
        /// </summary>
        [Parameter(Position = 5)]
        public SwitchParameter RobotOnly { get; set; }
        /// <summary>
        /// Параметр DismissedOnly указывает, что нужно показать только уволенных сотрудников
        /// </summary>
        [Parameter(Position = 6)]
        public SwitchParameter DismissedOnly { get; set; }
        /// <summary>
        /// Параметр ResultSize указывает максимальное число возвращаемых результатов
        /// По умолчанию выводится полный список сотрудников
        /// </summary>
        [Parameter(Position = 7)]
        public int? ResultSize { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<User> result = APIClient.GetAllUsersAsync().GetAwaiter().GetResult();
            if (Identity != null) {
                result = result.Where(u => u.id.ToString().Equals(Identity) ||
                u.nickname.ToLower().Equals(Identity.ToLower()) ||
                u.email.ToLower().Equals(Identity.ToLower())).ToList();
            }
            if (Filter != null) {
                result = result.Where(u => u.nickname.ToLower().Contains(Filter.ToLower()) ||
                u.name.first.ToLower().Contains(Filter.ToLower()) ||
                u.name.last.ToLower().Contains(Filter.ToLower()) ||
                u.name.middle.ToLower().Contains(Filter.ToLower()) ||
                u.nickname.ToLower().Contains(Filter.ToLower()) ||
                u.position.ToLower().Contains(Filter.ToLower()) ||
                u.email.ToLower().Contains(Filter.ToLower())
                ).ToList();
            }
            if (EnableOnly) {
                result = result.Where(u => u.isEnabled).ToList();
            }
            else
            if (DisableOnly) {
                result = result.Where(u => !u.isEnabled).ToList();
            }
            if (AdminOnly) {
                result = result.Where(u => u.isAdmin).ToList();
            }
            if (RobotOnly) {
                result = result.Where(u => u.isRobot).ToList();
            }
            if (DismissedOnly) {
                result = result.Where(u => u.isDismissed).ToList();
            }
            if (ResultSize != null) {
                result = result.Take((int)ResultSize).ToList();
            }
            WriteObject(result, true);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Изменить информацию о сотруднике
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "User", HelpUri = "https://github.com/witomin/Y360Management#set-user---изменить-информацию-о-сотруднике"), OutputType(typeof(List<User>))]
    public class SetUserCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр Identity определяет пользователя, который требуется изменить
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - nickname
        /// - email
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Identity { get; set; }
        /// <summary>
        /// Описание сотрудника
        /// </summary>
        [Parameter(Position = 1)]
        public string? About { get; set; }
        /// <summary>
        /// Дата рождения сотрудника
        /// </summary>
        [Parameter(Position = 2)]
        public string? Birthday { get; set; }
        /// <summary>
        /// Список контактов сотрудника
        /// </summary>
        [Parameter(Position = 3)]
        public List<BaseContact>? Contacts { get; set; }
        /// <summary>
        /// Идентификатор подразделения сотрудника
        /// </summary>
        [Parameter(Position = 4)]
        public ulong? DepartmentId { get; set; }
        /// <summary>
        /// Произвольный внешний идентификатор сотрудника
        /// </summary>
        [Parameter(Position = 5)]
        public string? ExternalId { get; set; }
        /// <summary>
        /// Пол сотрудника
        /// </summary>
        [Parameter(Position = 6)]
        public string? Gender { get; set; }
        /// <summary>
        /// Признак администратора организации
        /// </summary>
        [Parameter(Position = 7)]
        public bool? isAdmin { get; set; }
        /// <summary>
        /// Включить пользователя, нельзя использовать этот параметр вместе с параметром Disable
        /// </summary>
        [Parameter(Position = 8)]
        public SwitchParameter Enable { get; set; }
        /// <summary>
        /// Отключить пользователя, нельзя использовать этот параметр вместе с параметром Enable
        /// </summary>
        [Parameter(Position = 9)]
        public SwitchParameter Disable { get; set; }
        /// <summary>
        /// Язык сотрудника
        /// </summary>
        [Parameter(Position = 10)]
        public string? Language { get; set; }
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [Parameter(Position = 11)]
        public string? FirstName { get; set; }
        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        [Parameter(Position = 12)]
        public string? LastName { get; set; }
        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        [Parameter(Position = 13)]
        public string? MiddleName { get; set; }
        /// <summary>
        /// Пароль сотрудника
        /// </summary>
        [Parameter(Position = 14)]
        public string? Password { get; set; }
        /// <summary>
        /// Должность сотрудника
        /// </summary>
        [Parameter(Position = 15)]
        public string? Position { get; set; }
        /// <summary>
        /// Часовой пояс сотрудника
        /// </summary>
        [Parameter(Position = 16)]
        public string? Timezone { get; set; }
        /// <summary>
        /// Параметр для управления алиасами
        /// </summary>
        [Parameter(Position = 17)]
        public StringCollection? Aliases { get; set; }
        /// <summary>
        /// Обязательность изменения пароля при первом входе
        /// </summary>
        //[Parameter(Position = 17)]
        //public SwitchParameter ChangePassword { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            var allUsers = APIClient.GetAllUsersAsync().GetAwaiter().GetResult();
            var user = allUsers.SingleOrDefault(u => u.id.ToString().Equals(Identity) ||
            u.nickname.ToLower().Equals(Identity.ToLower()) ||
            u.email.ToLower().Equals(Identity.ToLower()));
            if (user is null) {
                base.EndProcessing();
                return;
            }
            UserEdit userEdit = new UserEdit {
                id = user.id,
                about = About ?? user.about,
                birthday = Birthday ?? user.birthday,
                contacts = Contacts?.Select(c => new BaseContact { type = c.type, value = c.value }).ToList(),
                departmentId = DepartmentId ?? user.departmentId,
                externalId = ExternalId ?? user.externalId,
                gender = Gender ?? user.gender,
                isAdmin = isAdmin ?? user.isAdmin,
                language = Language ?? user.language,
                name = new Name {
                    first = FirstName ?? user.name.first,
                    last = LastName ?? user.name.last,
                    middle = MiddleName ?? user.name.middle
                },
                password = Password,
                //passwordChangeRequired = ChangePassword,
                position = Position ?? user.position,
                timezone = Timezone ?? user.timezone
            };
            if (Enable) {
                userEdit.isEnabled = true;
            }
            else if (Disable) {
                userEdit.isEnabled = false;
            }
            else {
                userEdit.isEnabled = user.isEnabled;
            }
            if (Aliases != null) {
                if (Aliases.Add != null) {
                    foreach (var alias in Aliases.Add) {
                        if (!user.aliases.Contains(alias)) {
                            var res = APIClient.AddAliasToUserAsync(user.id, alias).GetAwaiter().GetResult();
                        }
                    }
                }
                if (Aliases.Remove != null) {
                    foreach (var alias in Aliases.Remove) {
                        if (user.aliases.Contains(alias)) {
                            var res = APIClient.DeleteAliasFromUserAsync(user.id, alias).GetAwaiter().GetResult();
                        }
                    }
                }
            }
            if (Contacts?.Count == 0) {
                var res = APIClient.DeleteContactsFromUserAsync(user.id).GetAwaiter().GetResult();
            }
            var result = APIClient.EditUserAsync(userEdit).GetAwaiter().GetResult();
            WriteObject(result, true);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Создать сотрудника
    /// </summary>
    [Cmdlet(VerbsCommon.New, "User", HelpUri = "https://github.com/witomin/Y360Management#new-user---создать-сотрудника"), OutputType(typeof(User))]
    public class NewUserCmdlet : PSCmdlet {
        /// <summary>
        /// Логин сотрудника
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string NickName { get; set; }
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        [Parameter(Position = 2, Mandatory = true)]
        public string LastName { get; set; }
        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        [Parameter(Position = 3)]
        public string? MiddleName { get; set; }
        /// <summary>
        /// Пароль сотрудника
        /// </summary>
        [Parameter(Position = 4, Mandatory = true)]
        public string Password { get; set; }
        /// <summary>
        /// Описание сотрудника
        /// </summary>
        [Parameter(Position = 5)]
        public string? About { get; set; }
        /// <summary>
        /// Дата рождения сотрудника
        /// </summary>
        [Parameter(Position = 6)]
        public string? Birthday { get; set; }
        /// <summary>
        /// Список контактов сотрудника
        /// </summary>
        [Parameter(Position = 7)]
        public List<BaseContact>? Contacts { get; set; }
        /// <summary>
        /// Идентификатор подразделения сотрудника
        /// </summary>
        [Parameter(Position = 8, Mandatory = true)]
        public ulong? DepartmentId { get; set; }
        /// <summary>
        /// Произвольный внешний идентификатор сотрудника
        /// </summary>
        [Parameter(Position = 9)]
        public string? ExternalId { get; set; }
        /// <summary>
        /// Пол сотрудника
        /// </summary>
        [Parameter(Position = 10)]
        public string? Gender { get; set; }
        /// <summary>
        /// Признак администратора организации
        /// </summary>
        [Parameter(Position = 11)]
        public SwitchParameter isAdmin { get; set; }
        /// <summary>
        /// Язык сотрудника
        /// </summary>
        [Parameter(Position = 12)]
        public string? Language { get; set; }
        /// <summary>
        /// Должность сотрудника
        /// </summary>
        [Parameter(Position = 13)]
        public string? Position { get; set; }
        /// <summary>
        /// Часовой пояс сотрудника
        /// </summary>
        [Parameter(Position = 14)]
        public string? Timezone { get; set; }
        protected override void EndProcessing() {
            if (string.IsNullOrEmpty(NickName) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Password)) {
                base.EndProcessing();
                return;
            }
            var APIClient = Helpers.GetApiClient(this);
            UserAdd userEdit = new UserAdd {
                nickname = NickName,
                about = About,
                birthday = Birthday,
                contacts = Contacts,
                departmentId = (ulong)DepartmentId,
                externalId = ExternalId,
                gender = Gender,
                isAdmin = isAdmin,
                language = Language,
                name = new Name {
                    first = FirstName,
                    last = LastName,
                    middle = MiddleName
                },
                password = Password,
                position = Position,
                timezone = Timezone
            };
            var result = APIClient.AddUserAsync(userEdit).GetAwaiter().GetResult();
            WriteObject(result);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Просмотреть статус 2FA сотрудника
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Status2FAUser", HelpUri = "https://github.com/witomin/Y360Management#get-status2fauser---просмотреть-статус-2fa-сотрудника"), OutputType(typeof(bool))]
    public class GetStatus2FAUserCmdlet : PSCmdlet {
        /// <summary>
        /// Параметр Identity определяет пользователя, который требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - nickname
        /// - email
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string? Identity { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            var allUsers = APIClient.GetAllUsersAsync().GetAwaiter().GetResult();
            var user = allUsers.SingleOrDefault(u => u.id.ToString().Equals(Identity) ||
            u.nickname.ToLower().Equals(Identity.ToLower()) ||
            u.email.ToLower().Equals(Identity.ToLower()));
            if (user != null) {
                var result = APIClient.GetStatus2FAUserAsync(user.id).GetAwaiter().GetResult();
                WriteObject(result);
            }
            base.EndProcessing();
        }
    }
}
