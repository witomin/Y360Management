using System.Collections.Generic;
using System.Management.Automation;
using Yandex.API360.Enums;
using Yandex.API360.Models;

namespace Y360Management {
    /// <summary>
    /// Получить правила обработки почты
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Rules"), OutputType(typeof(List<Rule>))]
    public class GetRulesCmdlet : PSCmdlet {
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            List<Rule> result = APIClient.GetRulesAsync().GetAwaiter().GetResult();
            WriteObject(result, true);
            base.EndProcessing();
        }
    }
    /// <summary>
    /// Задать правила обработки почты
    /// </summary>
    [Cmdlet(VerbsCommon.New, "Rule")]
    public class SetRulesCmdlet : PSCmdlet {
        /// <summary>
        /// Обязательный параметр направление
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public DirectionTypes Direction { get; set; }
        /// <summary>
        /// Список действий, которые необходимо выполнить при срабатывании правила.
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public List<Action> Actions { get; set; }
        /// <summary>
        /// Условия
        /// </summary>
        [Parameter(Position = 2, Mandatory = true)]
        public dynamic Condition { get; set; }
        /// <summary>
        /// Флаг-признак необходимости прекратить применение последующих правил при срабатывании данного.
        /// </summary>
        [Parameter(Position = 3)]
        public SwitchParameter Terminal { get; set; }
        protected override void EndProcessing() {
            var APIClient = Helpers.GetApiClient(this);
            var RuleList = new RulesList {
                rules = APIClient.GetRulesAsync().GetAwaiter().GetResult()
            };
            var newRule = new Rule {
                actions = Actions,
                condition = Condition,
                scope = new Scope {
                    direction = Direction
                },
                terminal = Terminal
            };
            RuleList.rules.Add(newRule);
            List<Rule> result = APIClient.SetRulesAsync(RuleList).GetAwaiter().GetResult();
            base.EndProcessing();
        }
    }
}
