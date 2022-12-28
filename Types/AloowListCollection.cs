using System.Collections.Generic;

namespace Y360Management.Types {
    /// <summary>
    /// Коллекция значений для управления белым списком
    /// </summary>
    public class AloowListCollection {
        /// <summary>
        /// Списко элементов, которые требуется добавить
        /// </summary>
        public List<string> Add { get; set; }
        /// <summary>
        /// Списко элементов, которые требуется удалить
        /// </summary>
        public List<string> Remove { get; set; }
    }
}
