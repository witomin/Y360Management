using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
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
}
