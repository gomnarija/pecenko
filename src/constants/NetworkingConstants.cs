class NetworkingConstants{
    public const int TIMEOUT = 5000;
    public const int MAX_MESSAGE_SIZE = int.MaxValue/32; 
    public const char MESSAGE_END = '\0';
    public const string MESSAGE_CODE_OK = "OK";
    public const string MESSAGE_CODE_FAIL = "FAIL";

    public const string ACTION_GET_INVOICE = "GET_INVOICE";
    public const string ACTION_GET_INSERTED = "GET_INSERTED";
    public const string ACTION_INSERT = "INSERT";
    public const string ACTION_EDIT = "EDIT";
    public const string ACTION_REMOVE = "REMOVE";
    public const string ACTION_GET_QUANTITIES = "GET_QUANTITIES";
}