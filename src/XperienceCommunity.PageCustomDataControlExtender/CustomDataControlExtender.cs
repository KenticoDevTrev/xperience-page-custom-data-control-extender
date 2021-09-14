using System;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;

namespace XperienceCommunity.PageCustomDataControlExtender
{
    /// <summary>
    /// A Form Control extender that redirects the Form Control value to/from
    /// the <see cref="TreeNode.NodeCustomData"/> or <see cref="TreeNode.DocumentCustomData"/> fields
    /// of the Page being edited
    /// </summary>
    public class CustomDataControlExtender : ControlExtender<FormEngineUserControl>
    {
        private IEventLogService Log => Service.Resolve<IEventLogService>();

        public string CustomDataColumnName => Control.GetValue("CustomDataColumnName", "");
        public bool UseDocumentCustomData => Control.GetValue("UseDocumentCustomData", true);

        public override void OnInit() => Control.Init += Control_Init;

        private void Control_Init(object sender, EventArgs e)
        {
            bool isExtenderValid = true;

            if (string.IsNullOrWhiteSpace(CustomDataColumnName))
            {
                isExtenderValid = false;

                Log.LogError(
                    nameof(CustomDataControlExtender),
                    "INVALID_CONFIGURATION",
                    $"The Form Control must have a {nameof(CustomDataColumnName)} property with a non-empty value");
            }
            if (Control.Form is null)
            {
                isExtenderValid = false;

                Log.LogError(
                    nameof(CustomDataControlExtender),
                    "INVALID_CONFIGURATION",
                    $"The Form Control must be used in a Form");
            }
            if (!(Control.Data is TreeNode page))
            {
                isExtenderValid = false;

                Log.LogError(
                    nameof(CustomDataControlExtender),
                    "INVALID_CONFIGURATION",
                    $"The Form Control must be used for Page Type fields only");
            }

            if (!isExtenderValid)
            {
                Control.Visible = false;

                return;
            }

            Control.Form.OnGetControlValue += Form_OnGetControlValue;
            Control.Form.OnAfterDataLoad += Form_OnAfterDataLoad;
            Control.Changed += Control_Changed;
        }

        private void Control_Changed(object sender, EventArgs e)
        {
            if (!(Control.Data is TreeNode page))
            {
                return;
            }

            SyncValueToPage(Control.Value, page);
        }

        private void Form_OnAfterDataLoad(object sender, EventArgs e)
        {
            if (!(Control.Data is TreeNode page))
            {
                return;
            }

            SyncPageToValue(page);
        }

        private void Form_OnGetControlValue(object sender, FormEngineUserControlEventArgs e)
        {
            if (!(Control.Data is TreeNode page))
            {
                return;
            }

            SyncValueToPage(Control.Value, page);
        }

        private void SyncValueToPage(object value, TreeNode page)
        {
            bool _ = UseDocumentCustomData
                ? page.DocumentCustomData.SetValue(CustomDataColumnName, value)
                : page.NodeCustomData.SetValue(CustomDataColumnName, value);
        }

        private void SyncPageToValue(TreeNode page) =>
            Control.Value = UseDocumentCustomData
                ? page.DocumentCustomData.GetValue(CustomDataColumnName)
                : page.NodeCustomData.GetValue(CustomDataColumnName);
    }
}
