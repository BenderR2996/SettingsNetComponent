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
        public string Id { get; set; }
        [DataMember]
        public string Mac { get; set; }

        /// <summary>
        /// Декоратор над (Id,Mac)
        /// </summary>
        [NotMapped]
        public AppIdentifier AppId
        {
            get => new AppIdentifier() { Id = Id, Mac = Mac };
            set
            {
                this.Mac = value.Mac;
                this.Id = value.Id;
            }
        }

        /// <summary>
        /// Настройки приложения в Json формате
        /// </summary>
        [DataMember]
        public string JsonData { get; set; }
    }
}
