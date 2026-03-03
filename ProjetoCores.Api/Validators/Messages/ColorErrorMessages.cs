namespace ProjetoCores.Api.Validators.Messages
{
    public static class ColorErrorMessages
    {
        public const string InvalidRed = "Red must be between 0 and 255";
        public const string InvalidGreen = "Green must be between 0 and 255";
        public const string InvalidBlue = "Blue must be between 0 and 255";
        public const string HexRequired = "Hex cannot be empty";
        public const string InvalidHexFormat = "Hex must be in format #RRGGBB";
        public const string MergeListTooShort = "At least 2 colors are required to merge";
    }
}
