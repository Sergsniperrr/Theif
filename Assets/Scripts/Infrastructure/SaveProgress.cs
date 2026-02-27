public static class SaveProgress
{
    public static class FilePath
    {
        public static readonly string LastScene = "LastScene.es3";
        public static readonly string Items = "Items.es3";
        public static readonly string Slots = "Slots.es3";
        public static readonly string Money = "Money.es3";
        public static readonly string PurchasedLevels = "PurchasedLevels.es3";
        public static readonly string IdleZoneTutorial = "IdleZoneTutorial";
        public static readonly string Load = "Load";
        public static readonly string Purchases = "Purchases";
    }

    public static class TitleKey
    {
        public static readonly string LastScene = nameof(LastScene);
        public static readonly string IsFirstLoad = nameof(IsFirstLoad);
        public static readonly string Items = nameof(Items);
        public static readonly string Money = nameof(Money);
        public static readonly string PurchasedLevels = nameof(PurchasedLevels);
        public static readonly string ShowIdleZone = "IdleZoneTutorial";
        public static readonly string PurchasesNumber = "PurchasesNumber";
    }
}
