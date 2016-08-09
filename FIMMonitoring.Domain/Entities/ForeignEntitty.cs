
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class ForeignEntitty : EntityBase
    {
        #region Properties
        [MaxLength(200)]
        public string Name { get; set; }

        public int ForeignId { get; set; }
        #endregion Properties

        #region ForeignKeys
        public int SystemId { get; set; }
        #endregion ForeignKeys

        #region Virtuals
        public virtual FimSystem System { get; set; }
        #endregion Virtuals
    }
}
