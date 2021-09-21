using System;
using CMS.DocumentEngine;

namespace XperienceCommunity.PageCustomDataControlExtender
{
    /// <summary>
    /// Enables versioning for <see cref="TreeNode.DocumentCustomData"/> and <see cref="TreeNode.NodeCustomData"/> fields
    /// </summary>
    /// <remarks>
    /// To register this manager, see https://docs.xperience.io/custom-development/customizing-providers/registering-providers-via-the-web-config
    /// </remarks>
    public class CustomDataVersionManager : VersionManager
    {
        public CustomDataVersionManager() : base(new TreeProvider()) { }

        protected CustomDataVersionManager(TreeProvider tree) : base(tree) { }

        protected override bool IsVersionedDocumentColumnInternal(string columnName)
        {
            if (string.Equals(columnName, nameof(TreeNode.DocumentCustomData), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return base.IsVersionedDocumentColumnInternal(columnName);
        }
    }
}
