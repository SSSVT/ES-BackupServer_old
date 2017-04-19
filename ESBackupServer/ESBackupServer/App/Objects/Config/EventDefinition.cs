using ESBackupServer.Database.Objects;
using System.Runtime.Serialization;

namespace ESBackupServer.App.Objects.Config
{
    [DataContract]
    public class EventDefinition
    {
        #region Event type
        [DataMember]
        private bool _IsBefore;
        [DataMember]
        public bool IsBeforeEvent
        {
            get
            {
                return this._IsBefore;
            }
            set
            {
                this.IsAfterEvent = !value;
                this._IsBefore = value;
            }
        }

        [DataMember]
        private bool _IsAfter;
        [DataMember]
        public bool IsAfterEvent
        {
            get
            {
                return this._IsAfter;
            }
            set
            {
                this.IsBeforeEvent = !value;
                this._IsAfter = value;
            }
        }
        #endregion

        [DataMember]
        public SettingTypeNames CommandType { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}