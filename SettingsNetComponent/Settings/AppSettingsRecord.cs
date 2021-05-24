using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SettingsNetComponent
{
    [DataContract]
    public class AppSettingsRecord
    {
        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
        /// <summary>
        /// Настройки приложения в Json формате
        /// </summary>
        [DataMember]
        public string JsonData { get; set; }
    }
}
