class ProgramConstans{

    public const string PROGRAM_NAME = "pečenko";
    public const string PROGRAM_VERSION = "0.52";
    public static readonly string APP_DATA_DIR_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROGRAM_NAME);
    public static int PORT = 2626;
    public static string IP = "127.0.0.1";
}