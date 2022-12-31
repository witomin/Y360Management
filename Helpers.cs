using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Yandex.API360;
using Yandex.API360.Models;

namespace Y360Management {
    public static class Helpers {
        public static List<User> allUsers = new List<User>();
        public static List<Group> allGroups = new List<Group>();
        public static List<Department> allDepartments = new List<Department>();

        public static Client GetApiClient(PSCmdlet Cmdlet) {
            Client APIClient = (Client)Cmdlet.SessionState.PSVariable.GetValue("APIClient");
            if (APIClient is null) {
                throw new Exception("Не установлено соединение с сервисом Y360. Выполните команду Connect-Y360");
            }
            return APIClient;
        }

        public static List<Member> GetMembers(List<string> source) {
            var members = new List<Member>();
            members.AddRange(allUsers.Where(u => source.Contains(u.id.ToString()) || source.Contains(u.nickname) || source.Contains(u.email)).Select(x => new Member { id = x.id, type = Yandex.API360.Enums.MemberTypes.user }).ToList());
            members.AddRange(allGroups.Where(u => source.Contains(u.id.ToString()) || source.Contains(u.name) || source.Contains(u.email)).Select(x => new Member { id = x.id, type = Yandex.API360.Enums.MemberTypes.group }).ToList());
            members.AddRange(allDepartments.Where(u => source.Contains(u.id.ToString()) || source.Contains(u.name) || source.Contains(u.email)).Select(x => new Member { id = x.id, type = Yandex.API360.Enums.MemberTypes.department }).ToList());
            members.Distinct();
            return members;
        }
    }
}
