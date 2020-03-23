namespace HelloOrleans.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ShoppingCart" />.
    /// </summary>
    [Serializable]
    public class ShoppingCart
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Content
        /// Defines the Content...
        /// </summary>
        public IList<string> Content { get; set; }

        #endregion
    }
}
