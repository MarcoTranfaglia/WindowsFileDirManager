using System.ComponentModel;

namespace WindowsFileDirManager.Models
{
    public enum FilterType
    {
        [Description("Starts With")]
        StartsWith,
        [Description("Ends With")]
        EndsWith,
        [Description("Contains")]
        Contains,
        [Description("ExtensionIs")]
        ExtensionIs
    }
}
