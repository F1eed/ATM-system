namespace ConsoleClient.Application.Model;

public sealed record Account(
    string Number, string PinCode, decimal Balance);
