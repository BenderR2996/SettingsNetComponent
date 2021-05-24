using System.Runtime.Serialization;

namespace SettingsNetComponent
{
    /// <summary>
    /// Идентификатор приложения
    /// </summary>
    [DataContract]
    public class AppIdentifier
    {
        /// <summary>
        /// Название приложения
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// Компьютер (хост) на котором выполняется данное приложение
        /// </summary>
        [DataMember]
        public string Mac { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is AppIdentifier appId)
            {
                return appId.Id.Equals(this.Id) && appId.Mac.Equals(this.Mac);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Возвращает массив [Id,Mac] для поиска составного ключа
        /// </summary>
        /// <returns></returns>
        public object[] GetKey()
        {
            return new[] { Id, Mac };
        }
        public override string ToString()
        {
            return $@"{Id}/{Mac}";
        }
    }
}
