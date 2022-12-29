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
        /// Идентификаторы руководителей группы
        /// </summary>
        [Parameter(Position = 4)]
        public List<ulong>? AdminIds { get; set; }
        /// <summary>
        /// Участники группы
        /// </summary>
        [Parameter(Position = 5)]
        public List<Member>? Members { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            BaseGroup newGroup = new BaseGroup {
                adminIds = AdminIds,
                description = Description,
                externalId = ExternalId,
                label = Label,
                name = Name,
                members = Members
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
        public UlongCollection? Members { get; set; }
        /// <summary>
        /// Параметр для замены всего списка участников группы (нельзя использовать совместно с параметром Members)
        /// </summary>
        [Parameter(Position = 5)]
        public List<ulong>? MemberList { get; set; }
        /// <summary>
        /// Параметр для удаления или добавления администраторов группы (нельзя использовать совместно с параметром AdminList)
        /// </summary>
        [Parameter(Position = 6)]
        public UlongCollection? Admins { get; set; }
        /// <summary>
        /// Параметр для замены всего списка администраторов группы (нельзя использовать совместно с параметром Admins)
        /// </summary>
        [Parameter(Position = 7)]
        public List<ulong>? AdminList { get; set; }

        private List<User> allUsers = new List<User>();
        private List<Group> allGroups = new List<Group>();
        private List<Department> allDepartments = new List<Department>();

        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Group> groups = APIClient.GetAllGroupsAsync().Result;
            Group group = groups.SingleOrDefault(u => u.id.ToString().Equals(Identity) || u.name.ToLower().Equals(Identity.ToLower()) || u.email.ToLower().Equals(Identity.ToLower()));
            if (group == null) {
                base.EndProcessing();
                return;
            }

            if (Members != null || MemberList != null || Admins != null || AdminList != null) {
                allUsers = APIClient.GetAllUsersAsync().Result;
                allGroups = APIClient.GetAllGroupsAsync().Result;
                allDepartments = APIClient.GetAllDepartmentsAsync().Result;
            }
            if (Name != null) group.name = Name;
            if (Description != null) group.description = Description;

            if (ExternalId != null) group.externalId = ExternalId;
            if (MemberList != null) {
                group.members = GetMembers(MemberList);
            }
            else if (Members != null) {
                if (Members.Add != null) {
                    group.members.AddRange(GetMembers(Members.Add));
                    group.members.Distinct();
                }
                if (Members.Remove != null) {
                    var removable = GetMembers(Members.Remove);
                    group.members = group.members.Where(m => !removable.Select(m => m.id).Contains(m.id)).ToList();
                }
            }
            if (AdminList != null) {
                group.adminIds = AdminList;
            }
            else if (Admins != null) {
                if (Admins.Add != null) {
                    group.adminIds.AddRange(Admins.Add);
                    group.adminIds.Distinct();
                }
                if (Admins.Remove != null) {
                    group.adminIds = group.adminIds.Where(a => !Admins.Remove.Contains(a)).ToList();
                }
            }
            //!!!!!!!!!!

            group.adminIds = null; // Не удается менять adminIds, приходит ошибка сервера код 500

            //!!!!!!!!!!!
            var result = APIClient.EditGroupAsync(group).Result;
            WriteObject(result);

            base.EndProcessing();
        }
        private List<Member> GetMembers(List<ulong> source) {
            var members = new List<Member>();
            members.AddRange(allUsers.Where(u => source.Contains(u.id)).Select(x => new Member { id = x.id, type = Yandex.API360.Enums.MemberTypes.user }).ToList());
            members.AddRange(allGroups.Where(u => source.Contains(u.id)).Select(x => new Member { id = x.id, type = Yandex.API360.Enums.MemberTypes.group }).ToList());
            members.AddRange(allDepartments.Where(u => source.Contains(u.id)).Select(x => new Member { id = x.id, type = Yandex.API360.Enums.MemberTypes.department }).ToList());
            members.Distinct();
            return members;
        }
    }
}
