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
        public const string ControlUseDocumentCustomDataPropertyName = "UseDocumentCustomData";

        private IEventLogService log;

        private IEventLogService Log
        {
            get
            {
                if (log is object)
                {
                    return log;
                }

                log = Service.Resolve<IEventLogService>();

                return log;
            }
        }

        /// <summary>
        /// If true, <see cref="TreeNode.DocumentCustomData"/> will store the <see cref="FormEngineUserControl.Value"/>, otherwise
        /// the value will be stored in <see cref="TreeNode.NodeCustomData"/>. Defaults to <see cref="TreeNode.DocumentCustomData"/>.
        /// </summary>
        public bool UseDocumentCustomData => Control.GetValue(ControlUseDocumentCustomDataPropertyName, true);

        public override void OnInit() => Control.Init += Control_Init;

        private void Control_Init(object sender, EventArgs e)
        {
            bool isExtenderValid = true;

            if (string.IsNullOrWhiteSpace(Control.Field))
            {
                isExtenderValid = false;

                Log.LogError(
                    nameof(CustomDataControlExtender),
                    "INVALID_CONFIGURATION",
                    $"The Form Control must have a {nameof(Control.Field)} with a non-empty value");
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

            Control.Form.FieldControls.Add(Control.Field, Control);
            Control.Data.ColumnNames.Add(Control.Field);
        }

        private void Form_OnAfterDataLoad(object sender, EventArgs e)
        {
            if (!(Control.Data is TreeNode page))
            {
                return;
            }

            Control.Value = UseDocumentCustomData
                ? page.DocumentCustomData.GetValue(Control.Field)
                : page.NodeCustomData.GetValue(Control.Field);
        }

        private void Form_OnGetControlValue(object sender, FormEngineUserControlEventArgs e)
        {
            if (!(Control.Data is TreeNode page))
            {
                return;
            }

            if (e.ColumnName.Equals(Control.Field, StringComparison.InvariantCultureIgnoreCase) && UseDocumentCustomData)
            {
                page.DocumentCustomData.SetValue(Control.Field, Control.Value);
            }
            else if (e.ColumnName.Equals(Control.Field, StringComparison.InvariantCultureIgnoreCase) && !UseDocumentCustomData)
            {
                page.NodeCustomData.SetValue(Control.Field, Control.Value);
            }
        }
    }
}
