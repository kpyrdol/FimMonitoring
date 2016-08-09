using FIMMonitoring.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FIMMonitoring.Domain.ViewModels.WCF
{
    [DataContract]
    public class ErrorPack
    {
        [DataMember]
        public int SystemId { get; set; }

        [DataMember]
        public List<Error> Errors { get; set; }
    }

    [DataContract]
    public class Error
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public KeyValueClass Customer { get; set; }

        [DataMember]
        public KeyValueClass Carrier { get; set; }

        [DataMember]
        public KeyValueClass ImportSource { get; set; }

        [DataMember]
        public bool IsValidated { get; set; }

        [DataMember]
        public bool IsParsed { get; set; }

        [DataMember]
        public bool IsDownloaded { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public ErrorSource ErrorSource { get; set; }

        [DataMember]
        public ErrorLevel ErrorLevel { get; set; }

        [DataMember]
        public ErrorType? ErrorType { get; set; }

        [DataMember]
        public DateTime? ErrorDate { get; set; }

        [DataMember]
        public DateTime? ErrorsSendDate { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public string Filename { get; set; }

        [DataMember]
        public int? ImportId { get; set; }

        [DataMember]
        public bool IsBusinessValidated { get; set; }
    }


    [DataContract]
    public class KeyValueClass
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
