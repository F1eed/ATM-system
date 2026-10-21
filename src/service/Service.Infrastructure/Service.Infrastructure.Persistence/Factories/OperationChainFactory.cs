using Service.Infrastructure.Persistence.Mapping.Operation;

namespace Service.Infrastructure.Persistence.Factories;

public static class OperationChainFactory
{
    public static IOperationLink Create()
    {
        var head = new CheckBalanceOperationLink();

        head.Add(new WithdrawOperationLink())
            .Add(new DepositOperationLink());

        return head;
    }
}