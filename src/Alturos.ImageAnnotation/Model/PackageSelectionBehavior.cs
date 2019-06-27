namespace Alturos.ImageAnnotation.Model
{
    public enum PackageSelectionBehavior
    {
        /// <summary>
        /// Always select the package.
        /// </summary>
        ForceSelect = 0,
        /// <summary>
        /// Refresh the current package, but don't switch to other packages.
        /// </summary>
        RefreshOnly,
        /// <summary>
        /// Switch to other packages, but don't refresh the current package.
        /// </summary>
        SwitchOnly
    }
}
