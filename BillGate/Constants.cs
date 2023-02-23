namespace BillGate;

public static class Constants
{
    public const string VERSION = "0.1"
#if DEBUG
                                  + "-DEV"
#endif
        ;

    public static class Help
    {
        public const string HELPFUL = "💁 Helpful";
        public const string FUN = "😆 Fun";
    }
}