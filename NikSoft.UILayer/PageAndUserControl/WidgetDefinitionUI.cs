using NikSoft.NikModel;
using System;

namespace NikSoft.UILayer
{
    public delegate void SkinChangedEventHandler();

    public delegate void WidgetTitleChangedEventHandler();

    public delegate void WidgetDeletionEventHandler(object sender, WidgetDeletionEventArgs e);

    public delegate void ShowWidgetSettingEvent();

    /// <summary>
    /// Provides the EventArgs class for the WidgetDeleted event.
    /// This EventArgs provides a single string parameter: WidgetID
    /// </summary>
    public class WidgetDeletionEventArgs : EventArgs
    {
        private string _widgetID;

        /// <summary>
        /// Describes which Widget was closed by returning the WidgetID property.
        /// </summary>
        public WidgetDeletionEventArgs(string widgetID)
        {
            _widgetID = widgetID;
        }

        /// <summary>
        /// Readonly access to WidgetID parameter of EventArgs class
        /// </summary>
        public string WidgetID
        {
            get
            {
                return _widgetID;
            }
        }
    }

    public class WidgetDefinitionUI : WidgetDefinition
    {

        public event WidgetDeletionEventHandler widgetDeleted;

        public event SkinChangedEventHandler SkinChanged;

        public event WidgetTitleChangedEventHandler WidgetTitleChanged;

        public event ShowWidgetSettingEvent ShowWidgetSetting;

        public void ShowPanelSetting()
        {
            if (null != ShowWidgetSetting)
                ShowWidgetSetting();
            //ShowWidgetSetting?.Invoke();
        }

        public virtual void OnWidgetDeletionEventHandler(object sender, WidgetDeletionEventArgs e)
        {
            if (widgetDeleted != null)
            {
                widgetDeleted(sender, e);
            }
            //widgetDeleted?.Invoke(sender, e);
        }

        public virtual void OnSkinChangedEventHandler()
        {
            if (SkinChanged != null)
            {
                SkinChanged();
            }
            //SkinChanged?.Invoke();
        }

        public virtual void OnWidgetTitleChangedEventHandler()
        {
            if (WidgetTitleChanged != null)
            {
                WidgetTitleChanged();
            }
        }
    }
}