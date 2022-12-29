using System.Collections.Generic;

namespace Y360Management.Types {
    /// <summary>
    /// Класс для управления коллекциями
    /// </summary>
    public class UlongCollection {
        /// <summary>
        /// Списко элементов, которые требуется добавить
        /// </summary>
        public List<ulong> Add { get; set; }
        /// <summary>
        /// Списко элементов, которые требуется удалить
        /// </summary>
        public List<ulong> Remove { get; set; }
    }
}
