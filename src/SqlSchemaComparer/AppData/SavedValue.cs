using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.AppData
{
    internal class SavedValue
    {
        /// <summary>
        /// The unique ID of the saved value
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The name of the saved value
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; set; }

        public SavedValue()
        {
            Id.SetRandom();
            Name = string.Empty;
            Value = string.Empty;
        }
    }
}
