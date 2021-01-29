using System.ComponentModel;

namespace WindowsFileDirManager.Models
{
    public enum ActionType
    {
        [Description("Rename")]
        Rename,
        [Description("Delete")]
        Delete
    }
}
