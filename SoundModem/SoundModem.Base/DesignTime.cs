using System.ComponentModel;
using System.Windows;

namespace SoundModem.Base
{
    public static class DesignTime
    {
        public static bool IsDesignTime
        {
            get
            {
                return (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);
            }
        }
    }
}
