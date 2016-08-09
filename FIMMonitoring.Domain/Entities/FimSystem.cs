using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class FimSystem : EntityBase
    {

        #region Properties

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Url { get; set; }

        [MaxLength(200)]
        public string CssStyle{ get; set; }

        #endregion Properties

        #region ForeignKeys
        #endregion ForeignKeys

        #region Virtuals
        #endregion Virtuals
    }
}
