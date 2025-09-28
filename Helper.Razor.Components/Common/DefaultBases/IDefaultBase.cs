
using Microsoft.AspNetCore.Components.Forms;

namespace Helper.Razor.Components.Common.DefaultBases;

public interface IDefaultBase
{
    abstract string ComponentId { get; init; }
    string Tooltip { get; set; }
}

public class test : ValidationSummary
{

}