namespace MixedMedia.Domain.BusinessRules.ImageRules
{
    public static class ImageRulesBase
    {
        public static bool RunAllValidations(List<IFormFile> images)
        {
            if(!ImageTypeRule.AreValidImageTypes(images))
                return false;
            return true;
        }
    }
}
