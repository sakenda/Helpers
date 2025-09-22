
namespace Helper.Razor.Components.Common.Models;

public class Icon
{
    public string IconChar { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public string CssClass { get; private set; } = string.Empty;
    public string Color { get; private set; } = "currentColor";
    public string Size { get; set; } = "1rem";

    public bool IsCharIcon => !string.IsNullOrWhiteSpace(IconChar) && string.IsNullOrWhiteSpace(ImageUrl) && string.IsNullOrWhiteSpace(CssClass);
    public bool IsImageIcon => !string.IsNullOrWhiteSpace(ImageUrl) && string.IsNullOrWhiteSpace(IconChar) && string.IsNullOrWhiteSpace(CssClass);
    public bool IsCssIcon => !string.IsNullOrWhiteSpace(CssClass) && string.IsNullOrWhiteSpace(ImageUrl) && string.IsNullOrWhiteSpace(IconChar);

    public string Value
    {
        get
        {
            if (IsCharIcon) return IconChar;
            if (IsImageIcon) return ImageUrl;
            if (IsCssIcon) return CssClass;
            return "";
        }
    }

    private Icon() { }

    public static Icon FromChar(string iconChar, string color = "currentColor", string size = "1rem")
    {
        ArgumentException.ThrowIfNullOrEmpty(iconChar, nameof(iconChar));

        return new Icon
        {
            IconChar = iconChar,
            Color = color
        };
    }

    public static Icon FromImageUrl(string imageUrl, string color = "currentColor", string size = "1rem")
    {
        ArgumentException.ThrowIfNullOrEmpty(imageUrl, nameof(imageUrl));

        return new Icon
        {
            ImageUrl = imageUrl,
            Color = color
        };
    }

    public static Icon FromCssClass(string cssClass, string color = "currentColor", string size = "1rem")
    {
        ArgumentException.ThrowIfNullOrEmpty(cssClass, nameof(cssClass));

        return new Icon
        {
            CssClass = cssClass,
            Color = color
        };
    }

}
