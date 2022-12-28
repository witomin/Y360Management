using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Users"), OutputType(typeof(List<User>))]
    public class GetUsersCmdlet : PSCmdlet {
        /// <summary>
        /// Параметр Identity определяет пользователя, который требуется просмотреть.
        /// Можно использовать любое значение, которое однозначно определяет пользователя:
        /// - Id
        /// - nickname
        /// - email
        /// </summary>
        [Parameter(Position = 0)]
        public string? Identity { get; set; }
        /// <summary>
        /// Параметр ResultSize указывает максимальное число возвращаемых результатов
        /// По умолчанию выводится полный список сотрудников
        /// </summary>
        [Parameter(Position = 1)]
        public int? ResultSize { get; set; }
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
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<User> result = null;
            if (Identity != null) {
                result = APIClient.GetAllUsersAsync().Result;
                result = result.Where(u => u.id.ToString().Equals(Identity) ||
                u.nickname.ToLower().Equals(Identity.ToLower()) ||
                u.email.ToLower().Equals(Identity.ToLower())).ToList();
            }
            else
            if (ResultSize != null) {
                result = APIClient.GetUsersAsync(perPage: (int)ResultSize).Result;
            }
            else {
                result = APIClient.GetAllUsersAsync().Result;
            }

            if (EnableOnly) {
                result = result.Where(u => u.isEnabled).ToList();
            } else
            if (DisableOnly) {
                result = result.Where(u => !u.isEnabled).ToList();
            }
            WriteObject(result);
            base.EndProcessing();
        }
    }
}
