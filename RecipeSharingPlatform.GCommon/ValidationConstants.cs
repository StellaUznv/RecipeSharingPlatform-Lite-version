namespace RecipeSharingPlatform.GCommon
{
    public class ValidationConstants
    {
        public static class Recipe
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 80;

            public const int InstructionsMinLength = 10;
            public const int InstructionsMaxLength = 250;
        }

        public static class Category
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 20;
        }
    }
}
