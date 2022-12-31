using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Y360Management.Types;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить информацию о группах
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Groups"), OutputType(typeof(List<Group>))]
    public class GetGroupsCmdlet : PSCmdlet {
        /// <summary>
        /// Параметр Identity определяет группу, которую требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - name
        /// - email
        /// </summary>
        [Parameter(Position = 0)]
        public string? Identity { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Group> result = APIClient.GetAllGroupsAsync().Result;
            if (Identity != null) {
                result = result.Where(u => u.id.ToString().Equals(Identity) || u.name.ToLower().Equals(Identity.ToLower()) || u.email.ToLower().Equals(Identity.ToLower())).ToList();
            }
            WriteObject(result);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Получить участников группы
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GroupMembers"), OutputType(typeof(MembersList))]
    public class GetGroupMembersCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр Identity определяет группу, которую требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - name
        /// - email
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Identity { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Group> groups = APIClient.GetAllGroupsAsync().Result;
            Group group = groups.SingleOrDefault(u => u.id.ToString().Equals(Identity) || u.name.ToLower().Equals(Identity.ToLower()) || u.email.ToLower().Equals(Identity.ToLower()));
            MembersList result = null;
            if (group != null) {
                result = APIClient.GetGroupMembersAsync(group.id).Result;
            }
            WriteObject(result);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Создать новую группу.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "Group"), OutputType(typeof(Group))]
    public class NewGroupCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр Название группы
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Name { get; set; }
        /// <summary>
        /// Описание группы
        /// </summary>
        [Parameter(Position = 1)]
        public string? Description { get; set; }
        /// <summary>
        /// Произвольный внешний идентификатор группы
        /// </summary>
        [Parameter(Position = 2)]
        public string? ExternalId { get; set; }
        /// <summary>
        /// Имя почтовой рассылки группы. Например, для адреса new-group@ваш-домен.ru имя почтовой рассылки — это new-group
        /// </summary>
        [Parameter(Position = 3)]
        public string? Label { get; set; }
        /// <summary>
        /// Руководители группы
        /// </summary>
        [Parameter(Position = 4)]
        public List<string>? Admins { get; set; }
        /// <summary>
        /// Участники группы
        /// </summary>
        [Parameter(Position = 5)]
        public List<string>? Members { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            if (Members != null || Admins!=null) {
                Helpers.allUsers = APIClient.GetAllUsersAsync().Result;
                Helpers.allGroups = APIClient.GetAllGroupsAsync().Result;
                Helpers.allDepartments = APIClient.GetAllDepartmentsAsync().Result;
            }
            BaseGroup newGroup = new BaseGroup {
                adminIds = Admins is null ? null : Helpers.GetMembersByIdentityList(Admins).Select(a=>a.id).ToList(),
                description = Description,
                externalId = ExternalId,
                label = Label,
                name = Name,
                members = Members is null ? null : Helpers.GetMembersByIdentityList(Members)
            };
            var result = APIClient.AddGroupAsync(newGroup).Result;
            WriteObject(result);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Управлять группами
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "Groups"), OutputType(typeof(Group))]
    public class SetGroupsCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр Identity определяет группу, которую требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - name
        /// - email
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Identity { get; set; }
        /// <summary>
        /// Название группы
        /// </summary>
        [Parameter(Position = 1)]
        public string? Name { get; set; }
        /// <summary>
        /// Описание группы
        /// </summary>
        [Parameter(Position = 2)]
        public string? Description { get; set; }
        /// <summary>
        /// Произвольный внешний идентификатор группы
        /// </summary>
        [Parameter(Position = 3)]
        public string? ExternalId { get; set; }
        /// <summary>
        /// Параметр для удаления или добавления учасников группы (нельзя использовать совместно с параметром MemberList)
        /// </summary>
        [Parameter(Position = 4)]
        public StringCollection? Members { get; set; }
        /// <summary>
        /// Параметр для замены всего списка участников группы (нельзя использовать совместно с параметром Members)
        /// </summary>
        [Parameter(Position = 5)]
        public List<string>? MemberList { get; set; }
        /// <summary>
        /// Параметр для удаления или добавления администраторов группы (нельзя использовать совместно с параметром AdminList)
        /// </summary>
        [Parameter(Position = 6)]
        public StringCollection? Admins { get; set; }
        /// <summary>
        /// Параметр для замены всего списка администраторов группы (нельзя использовать совместно с параметром Admins)
        /// </summary>
        [Parameter(Position = 7)]
        public List<string>? AdminList { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            Helpers.allGroups = APIClient.GetAllGroupsAsync().Result;
            Group group = Helpers.allGroups.SingleOrDefault(u => u.id.ToString().Equals(Identity) || u.name.ToLower().Equals(Identity.ToLower()) || u.email.ToLower().Equals(Identity.ToLower()));
            if (group == null) {
                base.EndProcessing();
                return;
            }
            if (Members != null || MemberList != null || Admins != null || AdminList != null) {
                Helpers.allUsers = APIClient.GetAllUsersAsync().Result;
                Helpers.allDepartments = APIClient.GetAllDepartmentsAsync().Result;
            }
            if (Name != null) group.name = Name;
            if (Description != null) group.description = Description;

            if (ExternalId != null) group.externalId = ExternalId;
            if (MemberList != null) {
                group.members = Helpers.GetMembersByIdentityList(MemberList);
            }
            else if (Members != null) {
                if (Members.Add != null) {
                    group.members.AddRange(Helpers.GetMembersByIdentityList(Members.Add));
                    group.members.Distinct();
                }
                if (Members.Remove != null) {
                    var removable = Helpers.GetMembersByIdentityList(Members.Remove);
                    if (removable.Count == 1) {
                        var res = APIClient.DeleteMemderFromGroupAsync(group.id, removable[0]).Result;
                        group.members = null;
                    }
                    else {
                        group.members = group.members.Where(m => !removable.Select(m => m.id).Contains(m.id)).ToList();
                    }
                }
            }
            if (AdminList != null) {
                group.adminIds = AdminList is null ? null : Helpers.GetMembersByIdentityList(AdminList).Select(a => a.id).ToList();
            }
            else if (Admins != null) {
                if (Admins.Add != null) {
                    group.adminIds.AddRange(Helpers.GetMembersByIdentityList(Admins.Add).Select(a => a.id).ToList());
                    group.adminIds.Distinct();
                }
                if (Admins.Remove != null) {
                    var removable = Helpers.GetMembersByIdentityList(Admins.Remove).Select(a=>a.id);
                    group.adminIds = group.adminIds.Where(m => !removable.Contains(m)).ToList();
                }
            }
            //!!!!!!!!!!

            group.adminIds = null; // Не удается менять adminIds, приходит ошибка сервера код 500

            //!!!!!!!!!!!
            var result = APIClient.EditGroupAsync(group).Result;
            WriteObject(result);

            base.EndProcessing();
        }
    }
    [Cmdlet(VerbsCommon.Remove, "Group"), OutputType(typeof(List<Group>))]
    public class RemoveGroupCmdlet : PSCmdlet {
        /// <summary>
        /// Параметр Identity определяет группу, которую требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - name
        /// - email
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Identity { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Group> allGroups = APIClient.GetAllGroupsAsync().Result;
            Group group = allGroups.SingleOrDefault(u => u.id.ToString().Equals(Identity) || u.name.ToLower().Equals(Identity.ToLower()) || u.email.ToLower().Equals(Identity.ToLower()));
            var res = APIClient.DeleteGroupAsync(group.id).Result;
            base.EndProcessing();
        }
    }
}
